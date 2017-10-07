using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.ContentSystem;
using UDBase.Controllers.SoundSystem;

public class SoundBinding : MonoBehaviour {
	
	public AudioClipHolder Click;
	public AudioClipHolder StartSound;
	public AudioClipHolder End;

	void Start() {
		Events.Subscribe<Audio_Click>(this, OnClick);
		Events.Subscribe<Game_Start>(this, OnStart);
		Events.Subscribe<Game_End>(this, OnEnd);
	}

	void OnDestroy() {
		Events.Unsubscribe<Audio_Click>(OnClick);
		Events.Unsubscribe<Game_Start>(OnStart);
		Events.Unsubscribe<Game_End>(OnEnd);
	}

	void OnClick(Audio_Click e) {
		Sound.Play(Click.Id);
	}

	void OnStart(Game_Start e) {
		Sound.Play(StartSound.Id);
	}

	void OnEnd(Game_End e) {
		Sound.Play(End.Id);
	}
}
