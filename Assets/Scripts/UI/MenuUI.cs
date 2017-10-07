using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.SaveSystem;

public class MenuUI : MonoBehaviour {
	public Text BestResult;

	void Start() {
		UpdateResult();
	}

	void UpdateResult() {
		var save = Save.GetNode<GameSave>();
		BestResult.text = save.BestResult.ToString();
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
