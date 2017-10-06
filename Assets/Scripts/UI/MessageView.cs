using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;

public class MessageView : MonoBehaviour {
	public List<GameEndMessage> EndMessages;
	public Text Text;

	void Awake() {
		Events.Subscribe<Game_End>(this, OnGameEnd);
		SetMessage("");
	}

	void OnDestroy() {
		Events.Unsubscribe<Game_End>(OnGameEnd);
	}

	void OnGameEnd(Game_End e) {
		foreach ( var endMessage in EndMessages ) {
			if ( endMessage.Resource == e.Resource ) {
				SetMessage(endMessage.Message);
			}
		}
	}

	void SetMessage(string text) {
		Text.text = text;
	}
}
