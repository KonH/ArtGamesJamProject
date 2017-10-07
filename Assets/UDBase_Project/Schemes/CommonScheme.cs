using UnityEngine;
using System.Collections;
using UDBase.Common;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.UserSystem;
using UDBase.Controllers.LeaderboardSystem;
using UDBase.Controllers.ContentSystem;
using UDBase.Controllers.AudioSystem;
using UDBase.Controllers.SoundSystem;
using UDBase.Controllers.MusicSystem;
using UDBase.Controllers.ConfigSystem;
using UDBase.Utils;

public class CommonScheme : Scheme {

	public CommonScheme() {
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
		AddController<Audio>(new AudioController("AudioMixer"));
		AddController<Sound>(new SoundController());
		AddController<Music>(new MusicController());
		var config = new FsJsonResourcesConfig();
		config.AddNode<TextConfig>("text");
		AddController<Config>(config);

		UnityHelper.LoadPersistant<SoundBinding>("SoundBinding");
	}
}
