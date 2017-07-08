using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class Logger
{
	public static Text text;

	public static void Log (string str){
		if (text) {
			text.text += str+"/n";
		} else
			Debug.Log ("<color=yellow>" + str + "</color>");
	}
}
