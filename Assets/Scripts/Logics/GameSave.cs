using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public class GameSave {
	[fsProperty("best_result")]
	public int BestResult;

	[fsProperty("endings")]
	public List<string> Endings = new List<string>();

	[fsProperty("rus")]
	public bool IsRussian;
}
