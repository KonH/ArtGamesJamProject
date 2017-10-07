using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.SceneSystem;

public class MenuUI : MonoBehaviour {
	public Text BestResult;

	void Start() {
		UpdateResult();
	}

	void UpdateResult() {
		// TODO
	}

	public void OnPlay() {
		Scene.LoadSceneByName("Game");
	}

	public void OnLeaderboards() {
		// TODO
	}

	public void OnCredits() {
		// TODO
	}
}
