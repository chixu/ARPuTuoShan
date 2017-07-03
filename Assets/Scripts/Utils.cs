using System.Collections;
using System.Collections.Generic;
using System;

public class Utils
{
	public static string ReplaceKeyword (string str, Dictionary<string, string> dict)
	{
		foreach (string key in dict.Keys) {
			if (str.Contains ("{%" + key + "%}"))
				str = str.Replace ("{%" + key + "%}", dict [key]);
		}
		return str;
	}
}
