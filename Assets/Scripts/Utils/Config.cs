using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class Config
{

	private static XElement localConfig;
	private static XElement remoteConfig;

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

	public static IEnumerator LoadConfig(string url){
		localConfig = null;
		remoteConfig = null;
		yield return Request.ReadPersistent (url, str => Config.localConfig = XDocument.Parse(str).Root);
		yield return Request.ReadRemote (url, str => Config.remoteConfig = XDocument.Parse(str).Root);
		string platform = GetPlatformName ();
		if (remoteConfig == null) {
			Debug.Log ("remoteConfig = null");
		} else if (localConfig == null || Config.GetVersion (localConfig) != Config.GetVersion (remoteConfig)) {
			Debug.Log ("remoteConfig != null");
			var nodes = remoteConfig.Elements ();
			foreach (XElement node in nodes) {
				yield return Request.DownloadFile (node.Value.Replace ("{%platform%}", platform), node.Value.Replace ("{%platform%}", ""));
			}
			//Debug.Log ("remoteConfig: " + remoteConfig.ToString ());
			File.WriteAllText (Path.Combine(Application.persistentDataPath,  url), remoteConfig.ToString());
		}
	}
}
