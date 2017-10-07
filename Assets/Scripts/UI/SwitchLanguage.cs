using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.SceneSystem;

public class SwitchLanguage : MonoBehaviour {
	public bool IsRussian;

	void Start() {
		gameObject.SetActive(Localization.IsRussian() != IsRussian);
	}

	public void Activate() {
		Localization.SetLanguage(IsRussian);
		Scene.ReloadScene();
	}
}
