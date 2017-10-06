using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour {
	public ResourceHolder Owner;
	public Text SampleText;

	public void Init(ResourceHolder holder) {
		Owner = holder;
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
		}
	}
}
