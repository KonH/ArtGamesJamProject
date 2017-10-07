using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.UserSystem;
using UDBase.Controllers.LeaderboardSystem;
using UDBase.Utils;

public class LeaderboardControls : MonoBehaviour {

	public LeaderboardContent Content;
	public GameObject LoadingItem;

	public int Limit = 100;
	public List<string> NameTemplates = new List<string>();

	bool Loading {
		set {
			LoadingItem.SetActive(value);
		}
	}

	void Start() {
		StartRefresh();
	}

	void OnUserChanged(string newValue) {
		User.Name = newValue;
	}

	void OnVersionChanged(string newValue) {
		Leaderboard.Version = newValue;
	}

	void StartRefresh() {
		Loading = true;
		Leaderboard.GetScores(Limit, "", EndRefresh);
	}

	void EndRefresh(List<LeaderboardItem> items) {
		Loading = false;
		Content.Clear();
		if ( items != null ) {
			for ( int i = 0; i < items.Count; i++ ) {
				var item = items[i];
				Content.Add(
					i + 1,
					item.UserName,
					item.Score);
			}
		}
	}
}
