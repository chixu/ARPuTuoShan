using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
	public Image left;
	public Image middle;
	public Image right;
	public float value = 0;
	public float maxValue = 100;
	public float maxLength = 300;
	// Use this for initialization
	void Start () {
//		float x = right.rectTransform.position.x;
//		float x = right.rectTransform.position.y;
//		right.rectTransform.localScale = new Vector2 (value, 1);;
		SetValue(0);
	}

	public void SetValue(float v = 10){
		float w = v / maxValue * maxLength;
		float x = right.rectTransform.position.x;
		middle.rectTransform.localScale = new Vector2 (w / 10, 1);
		middle.rectTransform.localPosition = new Vector2 (left.rectTransform.localPosition.x + left.rectTransform.rect.width, left.rectTransform.localPosition.y);
		right.rectTransform.localPosition = new Vector2 (middle.rectTransform.localPosition.x + w , middle.rectTransform.localPosition.y);
	}

}
