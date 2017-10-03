#if Scheme_DevScheme
using UnityEngine;
using System.Collections;
using UDBase.Common;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.EventSystem;

public class ProjectScheme : Scheme {

	public ProjectScheme() {
		AddController(new Log(), new UnityLog());
		AddController(new Scene(), new DirectSceneLoader());
		AddController(new Events(), new EventController());
	}
}
#endif
