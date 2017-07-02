using UnityEngine;
using System.Collections;

using Vuforia;
using System.Collections.Generic;
using System;
using System.IO;

public class DynamicDataSetLoader : MonoBehaviour
{
	// specify these in Unity Inspector
	public GameObject augmentationObject = null;
	// you can use teapot or other object
	public string dataSetName = "";
	//  Assets/StreamingAssets/QCAR/DataSetName
 
	// Use this for initialization
	void Start ()
	{
		// Vuforia 5.0 to 6.1
		//VuforiaBehaviour vb = GameObject.FindObjectOfType<VuforiaBehaviour> ();
		//vb.RegisterVuforiaStartedCallback (LoadDataSet);
	 
		// Vuforia 6.2+
		VuforiaARController.Instance.RegisterVuforiaStartedCallback (LoadDataSet);
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

				Debug.Log ("DynamicImageTarget-" + tb.TrackableName);
//				if (tb.name == "New Game Object") {
//				 
//					// change generic name to include trackable name
					tb.gameObject.name = ++counter + ":DynamicImageTarget-" + tb.TrackableName;
//				 
//					// add additional script components for trackable
					tb.gameObject.AddComponent<DefaultTrackableEventHandler> ();
					tb.gameObject.AddComponent<TurnOffBehaviour> ();
				 
					if (augmentationObject != null) {
						// instantiate augmentation object and parent to trackable
						GameObject augmentation = (GameObject)GameObject.Instantiate (augmentationObject);
						augmentation.transform.parent = tb.gameObject.transform;
						//augmentation.transform.localPosition = new Vector3 (0f, 0f, 0f);
						//augmentation.transform.localRotation = Quaternion.identity;
						//augmentation.transform.localScale = new Vector3 (0.005f, 0.005f, 0.005f);
						augmentation.gameObject.SetActive (true);
					} else {
						Debug.Log ("<color=yellow>Warning: No augmentation object specified for: " + tb.TrackableName + "</color>");
					}
//				}

			}
			StartCoroutine(StartGame());
//			GameObject[] objs = GameObject.FindGameObjectsWithTag("imagetarget");
//			GameObject obj = objs [0];
//			ImageTargetBehaviour itb = obj.GetComponent<ImageTargetBehaviour> ();
//			itb.
//			Debug.Log ("objs " + objs.Length.ToString());
		} else {
			Debug.LogError ("<color=yellow>Failed to load dataset: '" + dataSetName + "'</color>");
		}
	}


	string GetAssetBundleFolderName(bool isRemote = false){
		string platform;
		if (Application.platform == RuntimePlatform.Android)
			platform = "Android";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			platform = "IOS";
		else
			platform = "Windows";
		return Application.persistentDataPath+ "/assetbundle/" + platform +"/" + "myar.dlc";
	}


	IEnumerator StartGame(){
		using (WWW www = new WWW ("file:///"+GetAssetBundleFolderName())) {
			yield return www;
			if (www.error != null) {
				Debug.Log ("Network error");
				yield break;
			} else {

				AssetBundle bundle = www.assetBundle;
				Debug.Log ("===================assetNames==============");
				string[] assetNames;
				try{
					Debug.Log ("====11111====");
					Debug.Log ("=======" + (bundle == null).ToString());
					Debug.Log ("=======" + bundle.ToString());
					assetNames = bundle.GetAllAssetNames();
					Debug.Log ("=======" + (assetNames == null).ToString());
					Debug.Log ("====22222====");
					Debug.Log (assetNames.Length.ToString());
					foreach (string name in assetNames) {
						Debug.Log (name);
						Instantiate(bundle.LoadAsset(Path.GetFileNameWithoutExtension(name)));
					}
					Debug.Log ("===================scenePath==============");
					string[] scenePath = bundle.GetAllScenePaths();
					foreach (string path in scenePath) {
						Debug.Log (path);
					}

				}catch(Exception e){
					Debug.Log (e.StackTrace);
				}

			}
		}
	}
}