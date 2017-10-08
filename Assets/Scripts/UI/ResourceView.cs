using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UDBase.Utils;

public class ResourceView : MonoBehaviour {
	const int MaxChange = 5;
	
	public ResourceHolder Owner;
	public Text SampleText;
	public Image Slider;
	public Image Icon;

	int _maxValue;
	int _prevChange;

	public void Init(ResourceHolder holder, int maxValue) {
		Owner = holder;
		_maxValue = maxValue;
	}

	public void UpdateStatus(ResourceHolder holder) {
		if ( holder == Owner ) {
			if ( SampleText.isActiveAndEnabled ) {
				var changeValue = "";
				if ( holder.Change > 0 ) {
					changeValue = "+" + holder.Change;
				} else if ( holder.Change < 0 ) {
					changeValue = holder.Change.ToString();
				}
				SampleText.text = string.Format("{0}: {1} {2}", holder.Resource.Name, holder.Count, changeValue);
			}
			Icon.color = GetColor(holder.Change);
			Icon.transform.localScale = Vector3.one;
			if ( holder.Change != _prevChange ) {
				Icon.transform.DOShakeScale(0.33f, Vector2.one, 10, 0).OnComplete(() => Icon.transform.localScale = Vector3.one);
				_prevChange = holder.Change;
			}
			Slider.fillAmount = (float)holder.Count / _maxValue;
		}
	}

	Color GetColor(int change) {
		if ( change != 0 ) {
			if ( change > 0 ) {
				return Color.Lerp(Color.white, Color.green, (float)change / MaxChange);
			} else {
				return Color.Lerp(Color.white, Color.red, (float)Mathf.Abs(change) / MaxChange);
			}
		}
		return Color.white;
	}
}
