using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventHistory {
	public Event Event;
	public int CaseIndex;

	public EventHistory(Event ev, int csIndex) {
		Event = ev;
		CaseIndex = csIndex;
	}
}
