using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour {
	public int Index;
	public List<EventCase> Cases;
	public Event EventDepend;
	public int CaseDepend;

	public string ConvertMessage() {
		return string.Format("event_{0}", Index);
	}

	public string ConvertCase(int caseIndex) {
		return string.Format("event_{0}_q{1}", Index, caseIndex);
	}

	public string ConvertResult(int caseIndex) {
		return string.Format("event_{0}_a{1}", Index, caseIndex);
	}
}
