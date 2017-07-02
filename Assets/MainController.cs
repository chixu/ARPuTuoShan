using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class MainController : MonoBehaviour {
	public string assetBundleFolderName = "assetbundle";
	public string remoteUrl = "";
	public string fileName = "myassets.dlc";
	public string configName = "config.json";
	public Text text;


	private Config localConfig;
	private Config remoteConfig;

	public class Config{
		public string version = "";
		public string[] files;
	}


//	IEnumerator ReadLocal  (bool isRemote = false){
//		Log ("loading " + GetConfigUrl (isRemote));
//		UnityWebRequest www = UnityWebRequest.Get ((isRemote?"":"file:///")+GetConfigUrl(isRemote));
//		Log (GetConfigUrl(isRemote));
//		yield return www.Send ();
//		if (www.isError) {
//			Log (www.error);
//			Log ("unabled to load " + GetConfigUrl (isRemote));
//		} else {
//			if(isRemote)
//				remoteConfig = JsonUtility.FromJson<Config> (www.downloadHandler.text);
//			else
//				localConfig = JsonUtility.FromJson<Config> (www.downloadHandler.text);
//		}
//	}

	IEnumerator ReadLocalConfig(){
		string url = GetConfigUrl ();
		WWW www = new WWW("file:///"+GetConfigUrl(false));
		Log ("loading " + url);
		yield return www;
		if (!String.IsNullOrEmpty(www.error)) {
			Log (www.error);
			Log ("unabled to load " + url);
		} else {
			localConfig = JsonUtility.FromJson<Config> (www.text);
		}
	}

	IEnumerator ReadRemoteConfig(){
		string url = GetConfigUrl (true);
		Log ("loading " + url);
		UnityWebRequest www = UnityWebRequest.Get (url);
		yield return www.Send ();
		if (www.isError) {
			Log (www.error);
			Log ("unabled to load " + url);
		} else {
			remoteConfig = JsonUtility.FromJson<Config> (www.downloadHandler.text);
		}
	}

	IEnumerator DownloadFile(string url, string dest){
		WWW www = new WWW (remoteUrl + "/" + url);
		yield return www;
		dest = Application.persistentDataPath + "/" + dest;
		Log (dest);
		Log (Path.GetDirectoryName (dest));
		if (!Directory.Exists (Path.GetDirectoryName (dest)))
			Directory.CreateDirectory (Path.GetDirectoryName (dest));
		File.WriteAllBytes ( dest, www.bytes);
	}


	public string platformName{
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

	string GetConfigUrl(bool isRemote = false){
		return (isRemote? remoteUrl : Application.persistentDataPath )+  "/" + configName;
	}

	string GetAssetBundleFolderName(bool isRemote = false){
		string platform;
		if (Application.platform == RuntimePlatform.Android)
			platform = "Android";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			platform = "IOS";
		else
			platform = "Windows";
		return ( isRemote? remoteUrl : Application.persistentDataPath)+ "/" + assetBundleFolderName  + "/" + platform +"/" + fileName;
	}

	IEnumerator StartGame(){
		using (WWW www = new WWW ("file:///"+GetAssetBundleFolderName())) {
			yield return www;
			if (www.error != null) {
				Log ("Network error");
				yield break;
			} else {

				AssetBundle bundle = www.assetBundle;
				Log ("===================assetNames==============");
				string[] assetNames;
				try{
					Log ("====11111====");
					Log ("=======" + (bundle == null).ToString());
					Log ("=======" + bundle.ToString());
					assetNames = bundle.GetAllAssetNames();
					Log ("=======" + (assetNames == null).ToString());
					Log ("====22222====");
					Log (assetNames.Length.ToString());
					foreach (string name in assetNames) {
						Log (name);
						Instantiate(bundle.LoadAsset(Path.GetFileNameWithoutExtension(name)));
					}
					Log ("===================scenePath==============");
					string[] scenePath = bundle.GetAllScenePaths();
					foreach (string path in scenePath) {
						Log (path);
					}

				}catch(Exception e){
					Log (e.StackTrace);
				}

			}
		}
	}

	IEnumerator Start(){
		yield return StartCoroutine (ReadRemoteConfig());
		yield return StartCoroutine (ReadLocalConfig());
		if (remoteConfig == null) {
			//yield break;
		} else if(localConfig == null || localConfig.version != remoteConfig.version){
			Log ("Overwrite localConfig");
			File.WriteAllText (GetConfigUrl (), JsonUtility.ToJson (remoteConfig));
			if (remoteConfig.files != null) {
				for(int i=0; i<remoteConfig.files.Length; i++){
					string fileName = assetBundleFolderName + "/" + platformName + "/" + remoteConfig.files [i];
					Log ("Downloading ..." + fileName);
					yield return DownloadFile (fileName, fileName);
				}
			}
		}
		//yield return StartGame ();
	}

	void Log(string str){
		text.text += "\n" + str;
	}
}
