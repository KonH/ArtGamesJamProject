using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.ContentSystem;
using UDBase.Controllers.SoundSystem;

public class SoundBinding : MonoBehaviour {
	
	public AudioClipHolder Click;

	void Start() {
		Events.Subscribe<Audio_Click>(this, OnClick);
	}

	void OnDestroy() {
		Events.Unsubscribe<Audio_Click>(OnClick);
	}

	void OnClick(Audio_Click e) {
		Debug.Log("ON_CLICK");
		Sound.Play(Click.Id);
	}
}
