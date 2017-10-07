using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.LogSystem;

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
	bool _skipable;
	Queue<Action> _queuedActions = new Queue<Action>();

	void Awake() {
		Events.Subscribe<Game_End>(this, OnGameEnd);
		Events.Subscribe<Event_New>(this, OnNewEvent);
		ClearMessage();
	}

	void Next() {
		if ( _queuedActions.Count > 0 ) {
			var action = _queuedActions.Dequeue();
			Log.Message("ExecContinue", LogTags.State);
			action();
			if ( _queuedActions.Count == 0 ) {
				_skipable = false;
				Log.Message("ResetContinue", LogTags.State);
			}
		}
	}

	void ClearMessage() {
		SetMessage("", _emptyCases, false);
	}

	void OnDestroy() {
		Events.Unsubscribe<Game_End>(OnGameEnd);
		Events.Unsubscribe<Event_New>(OnNewEvent);
	}

	void OnGameEnd(Game_End e) {
		foreach ( var endMessage in EndSetup.Messages ) {
			if ( endMessage.Resource == e.Resource ) {
				if ( e.FirstTime ) {
					var achieveMessage = endMessage.AchievementMessage;
					SetMessage(endMessage.Message, _emptyCases, true);
					_queuedActions.Enqueue(() => RaiseEndGameMessage(achieveMessage));
				} else {
					RaiseEndGameMessage(endMessage.Message);
				}
			}
		}
	}

	void RaiseEndGameMessage(string message) {
		Log.Message("RaiseEndGameMessage", LogTags.State);
		var cases = new List<CaseSetup>();
		cases.Add(new CaseSetup("button_restart", () => Events.Fire(new User_Restart())));
		cases.Add(new CaseSetup("button_menu", () => Events.Fire(new User_Menu())));
		SetMessage(message, cases, false);
	}

	void OnNewEvent(Event_New e) {
		var ev = e.Event;
		var cases = new List<CaseSetup>();
		foreach ( var cs in ev.Cases ) {
			var selection = cs;
			var action = WrapSelect(ev.ConvertResult(ev.Cases.IndexOf(cs)), () => Events.Fire(new User_Case(ev, selection)));
			cases.Add(new CaseSetup(ev.ConvertCase(ev.Cases.IndexOf(cs)), action));
		}
		SetMessage(ev.ConvertMessage(), cases, false);
	}

	Action WrapSelect(string message, Action action) {
		return () => {
			ClearMessage();
			if ( !string.IsNullOrEmpty(message) ) {
				SetMessage(message, _emptyCases, true);
				_queuedActions.Enqueue(action);
			} else {
				action();
			}
		};
	}

	void SetMessage(string text, List<CaseSetup> cases, bool skipable) {
		Log.MessageFormat("SetMessage: '{0}', {1}, skipable: {2}", LogTags.State, text, cases.Count, skipable);
		if ( !skipable && _skipable && ( _queuedActions.Count > 0 ) ) {
			Log.Message("Enque message", LogTags.State);
			_queuedActions.Enqueue(() => SetMessage(text, cases, skipable));
			return;
		}
		_skipable = skipable;
		if ( _skipable ) {
			cases = new List<CaseSetup>();
			cases.Add(new CaseSetup("button_next", () => Next()));
		}
		Text.text = Localization.Localize(text);
		for ( var i = 0; i < Cases.Count; i++ ) {
			Cases[i].gameObject.SetActive(i < cases.Count);
			if ( i < cases.Count ) {
				Cases[i].Init(cases[i].Text, cases[i].Callback);
			}
		}
	}
}
