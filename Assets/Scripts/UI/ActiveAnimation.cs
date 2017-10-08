using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ActiveAnimation : MonoBehaviour {
	
	Image _img;
	CanvasGroup _group;

	bool _state = false;

	void Awake() {
		_img = GetComponent<Image>();
		_group = GetComponent<CanvasGroup>();
		if( _group ) {
			_group.alpha = 0;
		} else if ( _img ) {
			_img.color = new Color(1, 1, 1, 0);
		}
	}

	public void SetState(bool state) {
		if ( _state != state ) {
			if ( _group ) {
				_group.alpha = state ? 0 : 1;
				_group.DOFade(state ? 1 : 0, 0.33f);
			} else if ( _img ) {
				_img.color = new Color(1, 1, 1, state ? 0 : 1);
				_img.DOFade(state ? 1 : 0, 0.33f);
			}
			_state = state;
		}
	}
}
