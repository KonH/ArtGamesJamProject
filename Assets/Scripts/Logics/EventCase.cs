using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventCase {
	public string Message;
	public string ResultMessage;
	public List<ResourceChange> Resources;
	public List<RegionChange> Regions;
}
