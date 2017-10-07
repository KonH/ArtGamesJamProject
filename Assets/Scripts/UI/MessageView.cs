using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;

public class MessageView : MonoBehaviour {
	
	class CaseSetup {
		public string Text;
		public Action Callback;

		public CaseSetup(string text, Action callback) {
			Text = text;
			Callback = callback;
		}	
	}

	public GameEndMessageSetup EndSetup;
	public Text Text;
	public List<CaseControl> Cases;

	List<CaseSetup> _emptyCases = new List<CaseSetup>();

	void Awake() {
		Events.Subscribe<Game_End>(this, OnGameEnd);
		Events.Subscribe<Event_New>(this, OnNewEvent);
		ClearMessage();
	}

	void ClearMessage() {
		SetMessage("", _emptyCases);
	}

	void OnDestroy() {
		Events.Unsubscribe<Game_End>(OnGameEnd);
		Events.Unsubscribe<Event_New>(OnNewEvent);
	}

	void OnGameEnd(Game_End e) {
		foreach ( var endMessage in EndSetup.Messages ) {
			if ( endMessage.Resource == e.Resource ) {
				var cases = new List<CaseSetup>();
				cases.Add(new CaseSetup("Restart", () => Events.Fire(new User_Restart())));
				cases.Add(new CaseSetup("Menu", () => Events.Fire(new User_Menu())));
				SetMessage(endMessage.Message, cases);
			}
		}
	}

	void OnNewEvent(Event_New e) {
		var ev = e.Event;
		var cases = new List<CaseSetup>();
		foreach ( var cs in ev.Cases ) {
			var selection = cs;
			cases.Add(new CaseSetup(cs.Message, () => Events.Fire(new User_Case(ev, selection))));
		}
		SetMessage(ev.Message, cases);
	}
	Action WrapSelect(Action action) {
		return () => {
			ClearMessage();
			action();
		};
	}

	void SetMessage(string text, List<CaseSetup> cases) {
		Text.text = text;
		for ( var i = 0; i < Cases.Count; i++ ) {
			Cases[i].gameObject.SetActive(i < cases.Count);
			if ( i < cases.Count ) {
				Cases[i].Init(cases[i].Text, cases[i].Callback);
			}
		}
	}
}
