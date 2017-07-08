using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using System;

public class Config
{

	private static XElement localConfig;
	private static XElement remoteConfig;
	public static bool forceBreak = false;
	private static Action<int, int> fileLoadedHandler;


	public static string GetVersion (XElement node ){
		if (node.Attribute ("version") != null) {
			return node.Attribute ("version").Value;
		}
		return "";
	}


	public static string GetPlatformName () {
		if (Application.platform == RuntimePlatform.Android)
			return "Android";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return "IOS";
		else
			return "Windows";
	}

	public static IEnumerator LoadConfig(string url, Action<int, int> loadedHandler = null){
		localConfig = null;
		remoteConfig = null;
		forceBreak = false;
		fileLoadedHandler = loadedHandler;
		Logger.Log (url);
		yield return Request.ReadPersistent (url, str => Config.localConfig = XDocument.Parse(str).Root);
		yield return Request.ReadRemote (url, str => Config.remoteConfig = XDocument.Parse(str).Root);
		string platform = GetPlatformName ();
		if (remoteConfig == null) {
			Debug.Log ("remoteConfig = null");
		} else if (localConfig == null || Config.GetVersion (localConfig) != Config.GetVersion (remoteConfig)) {
			Debug.Log ("remoteConfig != null");
			var nodes = remoteConfig.Elements ();
			int count = 0;
			foreach (XElement node in nodes)
				count++;
			if (fileLoadedHandler != null) {
				fileLoadedHandler.Invoke (0, count);
			}
			int index = 0;
			foreach (XElement node in nodes) {
				index++;
				yield return Request.DownloadFile (node.Value.Replace ("{%platform%}", platform), node.Value.Replace ("{%platform%}", ""));
				if (forceBreak) {
					yield break;
				} else {
					if (fileLoadedHandler != null) {
						fileLoadedHandler.Invoke (index, count);
					}
				}
			}
			//Debug.Log ("remoteConfig: " + remoteConfig.ToString ());
			File.WriteAllText (Path.Combine(Application.persistentDataPath,  url), remoteConfig.ToString());
		}
	}
}
