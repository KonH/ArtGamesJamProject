using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaseControl : MonoBehaviour {
	public Text Text;

	Action _callback;

	public void Init(string text, Action callback) {
		Text.text = text;
		_callback = callback;
	}

	public void Select() {
		_callback.Invoke();
	}
}
