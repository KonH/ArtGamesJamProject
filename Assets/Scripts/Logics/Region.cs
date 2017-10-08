using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;

public class Region : MonoBehaviour {
	public string Name;
	public List<ResourceChange> Changes;
	public int Level;
	public List<GameObject> LevelItems;

	public void UpdateLevel(int level) {
		var prevLevel = Level;
		Level = level;
		for ( int i = 0; i < LevelItems.Count; i++ ) {
			if ( LevelItems[i] ) {
				LevelItems[i].SetActive(i <= Level);
			}
		}
		if ( Level != prevLevel ) {
			Events.Fire(new Region_Update(this));
		}
	}

	public bool CanLevelUp() {
		return Level < LevelItems.Count - 1;
	}

	public bool CanLevelDown() {
		return Level > 0;
	}

	public void TryLevelUp() {
		if ( CanLevelUp() ) {
			UpdateLevel(Level + 1);
		}
	}

	public void TryLevelDown() {
		if ( CanLevelDown() ) {
			UpdateLevel(Level - 1);
		}
	}
}
