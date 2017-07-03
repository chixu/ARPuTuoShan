using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using Vuforia;

public class Main : MonoBehaviour
{
	public string assetBundleFolderName = "assetbundle";
	public string remoteUrl = "";
	public string fileName = "myassets.dlc";
	public string configName = "config.json";
	public Text text;


	private Config localConfig;
	private Config remoteConfig;
	private Mappings mappings;
	private string dataSetName = "trackings.xml";
	private Dictionary<string, UnityEngine.Object> loadedAssets = new Dictionary<string, UnityEngine.Object> ();
	private Dictionary<string, string> ConfigDict = new Dictionary<string, string> ();

	[System.Serializable]
	public class Config
	{
		public string version = "";
		public string[] files;
	}


	[System.Serializable]
	public struct Mappings
	{
		[System.Serializable]
		public struct Mapping
		{
			public string key;
			public string name;
		}

		public Mapping[] mappings;
	}

	IEnumerator ReadLocalConfig ()
	{
		string url = GetConfigUrl ();
		WWW www = new WWW ("file:///" + GetConfigUrl (false));
		yield return www;
		if (!String.IsNullOrEmpty (www.error)) {
			Log ("unabled to load Local Config");
		} else {
			Log ("Local Config Loaded");
			localConfig = JsonUtility.FromJson<Config> (www.text);
		}
	}

	IEnumerator ReadRemoteConfig ()
	{
		string url = GetConfigUrl (true);
		Log ("loading " + url);
		UnityWebRequest www = UnityWebRequest.Get (url);
		yield return www.Send ();
		if (www.isError) {
			Log ("unabled to load Remote Config");
		} else {
			Log ("Remote Config Loaded");
			remoteConfig = JsonUtility.FromJson<Config> (www.downloadHandler.text);
		}
	}

	IEnumerator DownloadFile (string url, string dest)
	{
		WWW www = new WWW (remoteUrl + "/" + url);
		yield return www;
		dest = Application.persistentDataPath + "/" + dest;
		Log (dest);
		Log (Path.GetDirectoryName (dest));
		if (!Directory.Exists (Path.GetDirectoryName (dest)))
			Directory.CreateDirectory (Path.GetDirectoryName (dest));
		File.WriteAllBytes (dest, www.bytes);
	}


	public string platformName {
		get { 
			if (Application.platform == RuntimePlatform.Android)
				return "Android";
			else if (Application.platform == RuntimePlatform.IPhonePlayer)
				return "IOS";
			else
				return "Windows";
		}
	}

	//	IEnumerator WriteFile(string path, byte[] data){
	//		File.WriteAllBytes(path, data);
	//	}

	string GetConfigUrl (bool isRemote = false)
	{
		return (isRemote ? remoteUrl : Application.persistentDataPath) + "/" + configName;
	}

	string GetLocalAssetName (string fileName)
	{
		return Application.persistentDataPath + "/" + fileName;
	}

	IEnumerator StartGame ()
	{
//		string mapPath = "file:///" + GetLocalAssetName ("mappings.json");
//		WWW www = new WWW (mapPath);
//		yield return www;
//		if (www.error != null) {
//			Log ("Cannot find mappings.json");
//		} else {
//			mappings = JsonUtility.FromJson<Mappings> (www.text);
//		}

		WWW www = new WWW ("file:///" + Application.persistentDataPath + "/" + fileName);
		yield return www;
		if (www.error != null) {
			Log ("Unable to load prefabs");
			yield break;
		} else {
			AssetBundle bundle = www.assetBundle;
			string[] assetNames;
			try {
				assetNames = bundle.GetAllAssetNames ();
				foreach (string name in assetNames) {
					string simpleName = Path.GetFileNameWithoutExtension (name);
					Log (simpleName);
					//Instantiate(bundle.LoadAsset());
					loadedAssets.Add (simpleName, bundle.LoadAsset (simpleName));
				}
//				Log ("===================scenePath==============");
//				string[] scenePath = bundle.GetAllScenePaths();
//				foreach (string path in scenePath) {
//					Log (path);
//				}

			} catch (Exception e) {
				Log (e.StackTrace);
			}

		}
		LoadDataSet ();
	}

	IEnumerator Start ()
	{
		Log (Application.persistentDataPath);
		ConfigDict.Add ("platform", platformName);
		yield return StartCoroutine (ReadRemoteConfig ());
		yield return StartCoroutine (ReadLocalConfig ());
		if (remoteConfig == null) {
			if (localConfig == null) {

			} else {

			}
			//yield break;
		} else if (localConfig == null || localConfig.version != remoteConfig.version) {
			Log ("Overwrite localConfig");
			File.WriteAllText (GetConfigUrl (), JsonUtility.ToJson (remoteConfig));
			if (remoteConfig.files != null) {
				for (int i = 0; i < remoteConfig.files.Length; i++) {
					string source = remoteConfig.files [i].Replace ("{%platform%}", platformName);
					string dest = remoteConfig.files [i].Replace ("{%platform%}/", "");
					Log ("Downloading ..." + source + " to " + dest);
					yield return DownloadFile (source, dest);
				}
			}
		}
		yield return StartGame ();
	}


	void LoadDataSet ()
	{

		ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker> ();

		DataSet dataSet = objectTracker.CreateDataSet ();
		Debug.Log (Application.persistentDataPath + "/" + dataSetName);
		if (dataSet.Load (Application.persistentDataPath + "/" + dataSetName, VuforiaUnity.StorageType.STORAGE_ABSOLUTE)) {
			//if (dataSet.Load (dataSetName)) {             
			objectTracker.Stop ();  // stop tracker so that we can add new dataset

			if (!objectTracker.ActivateDataSet (dataSet)) {
				// Note: ImageTracker cannot have more than 100 total targets activated
				Debug.Log ("<color=yellow>Failed to Activate DataSet: " + dataSetName + "</color>");
			}

			if (!objectTracker.Start ()) {
				Debug.Log ("<color=yellow>Tracker Failed to Start.</color>");
			}

			//int counter = 0;

			IEnumerable<TrackableBehaviour> tbs = TrackerManager.Instance.GetStateManager ().GetTrackableBehaviours ();
			foreach (TrackableBehaviour tb in tbs) {



				if (loadedAssets.ContainsKey (tb.TrackableName)) {
					Log ("DynamicImageTarget-" + tb.TrackableName);
					//				if (tb.name == "New Game Object") {
					//				 
					//					// change generic name to include trackable name
					tb.gameObject.name = "DynamicImageTarget-" + tb.TrackableName;
					//				 
					//					// add additional script components for trackable
					tb.gameObject.AddComponent<DefaultTrackableEventHandler> ();
					tb.gameObject.AddComponent<TurnOffBehaviour> ();
					UnityEngine.Object asset = loadedAssets [tb.TrackableName];
					GameObject obj = (GameObject)GameObject.Instantiate (asset);
					obj.transform.parent = tb.gameObject.transform;
					obj.gameObject.SetActive (true);
				}
			}
		} else {
			Debug.LogError ("<color=yellow>Failed to load dataset: '" + dataSetName + "'</color>");
		}
	}

	void Log (string str)
	{
		if (!String.IsNullOrEmpty (str))
			text.text += "\n" + str;
	}
}
