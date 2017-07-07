using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//StartCoroutine (NextScene ());
	}
	
	IEnumerator NextScene(){
		yield return new WaitForSeconds (1);
		SceneManager.LoadScene ("Selection");
	}

	public string RemoteUrl{
		set{
			Request.RemoteUrl = value;
		}
		get{
			return Request.RemoteUrl;
		}
	}
}
