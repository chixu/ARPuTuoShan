using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour {

	public Text text;

	void Awake(){
		Logger.text = text;
		Logger.Log ("dddddddddddd");
	}
}
