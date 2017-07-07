using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour {

	public bool loaded = false;
	// Use this for initialization
	void Start () {
		StartCoroutine (NextSceneAfterSeconds (5));
	}

	public void NextScene(){
		if (loaded)
			SceneManager.LoadScene ("Selection");
	}
	
	IEnumerator NextSceneAfterSeconds(int second){
		yield return new WaitForSeconds (second);
		NextScene ();
	}
}
