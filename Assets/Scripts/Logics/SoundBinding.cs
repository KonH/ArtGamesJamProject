using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.ContentSystem;
using UDBase.Controllers.SoundSystem;

public class SoundBinding : MonoBehaviour {
	
	public AudioClipHolder Click;
	public AudioClipHolder End;

	void Start() {
		Events.Subscribe<Audio_Click>(this, OnClick);
		Events.Subscribe<Game_End>(this, OnEnd);
	}

	void OnDestroy() {
		Events.Unsubscribe<Game_End>(OnEnd);
	}

	void OnClick(Audio_Click e) {
		Sound.Play(Click.Id);
	}

	void OnEnd(Game_End e) {
		Sound.Play(End.Id);
	}
}
