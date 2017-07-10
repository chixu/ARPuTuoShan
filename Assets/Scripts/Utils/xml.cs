using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using System;

public class xml
{
	public static string Attribute(XElement node, string name){
		if (node.Attribute (name) != null) {
			return node.Attribute (name).Value;
		}
		return "";
	}


	public static string Version (XElement node ){
		return Attribute(node, "version");
	}
}
