using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour {
	public ResourceHolder Owner;
	public Text SampleText;
	public Image Slider;

	int _maxValue;

	public void Init(ResourceHolder holder, int maxValue) {
		Owner = holder;
		_maxValue = maxValue;
	}

	public void UpdateStatus(ResourceHolder holder) {
		if ( holder == Owner ) {
			var changeValue = "";
			if ( holder.Change > 0 ) {
				changeValue = "+" + holder.Change;
			} else if ( holder.Change < 0 ) {
				changeValue = holder.Change.ToString();
			}
			SampleText.text = string.Format("{0}: {1} {2}", holder.Resource.Name, holder.Count, changeValue);
			Slider.fillAmount = (float)holder.Count / _maxValue;
		}
	}
}
