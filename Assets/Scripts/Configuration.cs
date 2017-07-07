﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Configuration : MonoBehaviour {
	public string remoteUrl;
	public string language = Language.Chinese;

	// Use this for initialization
	void Start () {
		Request.RemoteUrl = remoteUrl;
		if (!Directory.Exists (Path.Combine (Application.persistentDataPath, "ui"))) {
			string[] ignores = new string[1];
			ignores[0] = ".meta";
			FileUtils.CopyDirectory (Path.Combine (Application.streamingAssetsPath, "ui"), Path.Combine (Application.persistentDataPath, "ui"), ignores);
		}
		StartCoroutine (readConfig ());
	}

	IEnumerator readConfig ()
	{
		yield return Config.LoadConfig ("ui/config.xml");
		yield return I18n.Initialise (language);
		GetComponent<StartSceneController> ().loaded = true;
	}
}