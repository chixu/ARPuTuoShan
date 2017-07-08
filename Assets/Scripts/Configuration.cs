using System.Collections;
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
		StartCoroutine (readConfig ());
	}

	IEnumerator readConfig ()
	{
		yield return Config.LoadConfig ("ui/config.xml");
		yield return I18n.Initialise (language);
		GetComponent<StartSceneController> ().loaded = true;
	}
}
