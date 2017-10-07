using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.EventSystem;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour {
	Button _button;

	void Awake() {
		GetComponent<Button>().onClick.AddListener(() => Events.Fire(new Audio_Click()));
	}
}
