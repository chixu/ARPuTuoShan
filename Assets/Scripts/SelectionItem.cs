using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SelectionItem : MonoBehaviour {

	public Text text;
	public Image image;
	//private Action<string> onClickHandler;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

//	public void SetOnClick(Action<string> handler){
//		onClickHandler = handler;
//	}
	public void OnClick(){
		StartCoroutine (OnClickHandler ());
	}

	private IEnumerator OnClickHandler(){


		Debug.Log (this.name + " clicked");
		yield return Config.LoadConfig (this.name + "/config.xml");
		Hashtable arg = new Hashtable ();
		arg.Add ("name", this.name);
		SceneManagerExtension.LoadScene ("Scan", arg);
//		if (onClickHandler != null)
//			onClickHandler.Invoke (this.name);
	}

	//public void Init
}
