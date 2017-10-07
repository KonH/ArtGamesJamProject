using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.UserSystem;

public class LoginUI : MonoBehaviour {
	public InputField Field;
	public Button GoButton;

	void Awake() {
		Field.onValueChanged.AddListener(OnNameChanged);
		GoButton.onClick.AddListener(Go);
		GoButton.interactable = false;
	}
	
	void OnNameChanged(string name) {
		User.Name = name;
		GoButton.interactable = !string.IsNullOrEmpty(name);
	}

	void Go() {
		Scene.LoadSceneByName("Game");
	}
}
