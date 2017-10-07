#if Scheme_DevScheme
using UnityEngine;
using System.Collections;
using UDBase.Common;
using UDBase.Controllers.LogSystem;

public class ProjectScheme : CommonScheme {

	public ProjectScheme() {
		AddController(new Log(), new UnityLog());
	}
}
#endif
