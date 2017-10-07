using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.UserSystem;

public class MenuUI : MonoBehaviour {
	public Text BestResult;
	public Text AchievementCounter;

	public ResourceSetup Resources;

	void Start() {
		UpdateResult();
	}

	void UpdateResult() {
		var save = Save.GetNode<GameSave>();
		BestResult.text = save.BestResult.ToString();
		AchievementCounter.text = string.Format("{0}/{1}", save.Endings.Count, Resources.Resources.Count);
	}

	public void OnPlay() {
		if ( string.IsNullOrEmpty(User.Name) ) {
			Scene.LoadSceneByName("Login");
		} else {
			Scene.LoadSceneByName("Game");
		}
	}

	public void OnLeaderboards() {
		Scene.LoadSceneByName("Leaderboards");
	}

	public void OnCredits() {
		Scene.LoadSceneByName("Credits");
	}
}
