using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullSerializer;

public class TextConfig {
	[fsProperty("values_ru")]
	public Dictionary<string, string> ValuesRu;

	[fsProperty("values_en")]
	public Dictionary<string, string> ValuesEn;
}
