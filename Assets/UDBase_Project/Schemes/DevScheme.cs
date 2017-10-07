#if Scheme_DevScheme
using UnityEngine;
using System.Collections;
using UDBase.Common;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.UserSystem;
using UDBase.Controllers.LeaderboardSystem;
using UDBase.Controllers.ContentSystem;
using UDBase.Controllers.SoundSystem;
using UDBase.Controllers.MusicSystem;
using UDBase.Utils;

public class ProjectScheme : Scheme {

	public ProjectScheme() {
		AddController(new Log(), new UnityLog());
		AddController(new Scene(), new DirectSceneLoader());
		AddController(new Events(), new EventController());
		var save = new FsJsonDataSave();
		AddController(new Save(), save);
		save.AddNode<GameSave>("game_save");
		save.AddNode<UserSaveNode>("user");
		AddController<User>(new SaveUser());
		AddController<Leaderboard>(
			new WebLeaderboard("https://konhit.xyz/lbservice/", "cityBuilder", "1.0.0", "cityBuilder", "cityBuilder"));
		AddController<Content>(new DirectContentController());
		AddController<Sound>(new SoundController());
		AddController<Music>(new MusicController());

		UnityHelper.LoadPersistant<SoundBinding>("SoundBinding");
	}
}
#endif
