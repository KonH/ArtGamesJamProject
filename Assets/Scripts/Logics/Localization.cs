using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.ConfigSystem;
using UDBase.Controllers.LogSystem;

public static class Localization {
	public static string Localize(string key) {
		Log.MessageFormat("Localize: '{0}'", LogTags.Localization, key);
		var save = Save.GetNode<GameSave>();
		var textConfig = Config.GetNode<TextConfig>();
		var collection = save.IsRussian ? textConfig.ValuesRu : textConfig.ValuesEn;
		var value = "NOT FOUND";
		if ( collection.ContainsKey(key) ) {
			value = collection[key];
		}
		return value;
	}

	public static void SetLanguage(bool isRussian) {
		Log.MessageFormat("SetLanguage: '{0}'", LogTags.Localization, isRussian);
		var save = Save.GetNode<GameSave>();
		save.IsRussian = isRussian;
		Save.SaveNode(save);
	}

	public static bool IsRussian() {
		var save = Save.GetNode<GameSave>();
		return save.IsRussian;
	} 
}
