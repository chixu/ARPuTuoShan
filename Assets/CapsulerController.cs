using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsulerController : MonoBehaviour {

	public float speed = 10;
	Transform transform;
	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Time.deltaTime * speed, Time.deltaTime * speed, Time.deltaTime*speed);
	}
}
