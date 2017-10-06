using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;

public class TurnView : MonoBehaviour {
	public Text Text;

	void Awake() {
		Events.Subscribe<Turn_New>(this, OnNewTurn);
	}

	void OnDestroy() {
		Events.Unsubscribe<Turn_New>(OnNewTurn);
	}

	void OnNewTurn(Turn_New e) {
		Text.text = e.Number.ToString();
	}
}
