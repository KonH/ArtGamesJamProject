using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizeText : MonoBehaviour {
	Text _text;

	public string Key;

	void Awake() {
		_text = GetComponent<Text>();
	}

	void Start() {
		_text.text = Localization.Localize(Key);
	}
}
