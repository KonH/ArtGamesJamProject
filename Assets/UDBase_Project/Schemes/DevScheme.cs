#if Scheme_DevScheme
using UnityEngine;
using System.Collections;
using UDBase.Common;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SaveSystem;

public class ProjectScheme : Scheme {

	public ProjectScheme() {
		AddController(new Log(), new UnityLog());
		AddController(new Scene(), new DirectSceneLoader());
		AddController(new Events(), new EventController());
		var save = new FsJsonDataSave();
		AddController(new Save(), save);
		save.AddNode<GameSave>("game_save");
	}
}
#endif
