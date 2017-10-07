using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Utils;

public class GameState : MonoBehaviour {
	public int StartCount;
	public int Turn;
	public bool IsEnded;
	public ResourceSetup Resources;
	public EventSetup GameEvents;
	public List<ResourceHolder> Holders;
	public List<Region> Regions;

	void Awake() {
		Events.Subscribe<Region_Update>(this, OnRegionUpdate);
		Events.Subscribe<User_Restart>(this, OnUserRestart);
		Events.Subscribe<User_Case>(this, OnUserCase);
		Events.Subscribe<User_Menu>(this, OnUserMenu);
	}

	void OnDestroy() {
		Events.Unsubscribe<Region_Update>(OnRegionUpdate);
		Events.Unsubscribe<User_Restart>(OnUserRestart);
		Events.Unsubscribe<User_Case>(OnUserCase);
		Events.Unsubscribe<User_Menu>(OnUserMenu);
	}

	void OnUserRestart(User_Restart e) {
		Scene.ReloadScene();
	}

	void OnRegionUpdate(Region_Update e) {
		RecalculateResources();
	}

	void OnUserCase(User_Case e) {
		var cs = e.Case;
		ApplyCase(cs);
		NextTurn();
	}

	void OnUserMenu(User_Menu e) {
		Scene.LoadSceneByName("Menu");
	}

	void ApplyCase(EventCase cs) {
		foreach ( var resChange in cs.Resources ) {
			UpdateResource(resChange.Resource, resChange.Value);
		}
		foreach ( var regChange in cs.Regions ) {
			UpdateRegion(regChange.Name, regChange.UpDown);
		}
	}

	void UpdateRegion(string regName, bool upDown) {
		foreach ( var reg in Regions ) {
			if ( reg.Name == regName ) {
				if ( upDown ) {
					reg.TryLevelUp();
				} else {
					reg.TryLevelDown();
				}
			}
		}
	}

	ResourceHolder GetHolder(Resource resource) {
		foreach ( var holder in Holders ) {
			if ( holder.Resource == resource ) {
				return holder;
			}
		}
		return null;
	}

	void RecalculateResources() {
		foreach ( var holder in Holders ) {
			holder.Change = 0;
		}
		foreach ( var region in Regions ) {
			foreach ( var change in region.Changes ) {
				var holder = GetHolder(change.Resource);
				holder.Change += change.Value * region.Level;
			}
		}
		foreach ( var holder in Holders ) {
			UpdateResource(holder.Resource, 0);
		}
	}

	void Start() {
		InitHolders();
		InitRegions();
		OnNewTurn();
		UpdateEvent();
	}

	void OnNewTurn() {
		Events.Fire(new Turn_New(Turn));
	}

	public void NextTurn() {
		if ( IsEnded ) {
			return;
		}
		foreach ( var holder in Holders ) {
			if ( holder.Change != 0 ) {
				UpdateResource(holder.Resource, holder.Change);
			}
		}
		Turn++;
		OnNewTurn();
		IsEnded = CheckGameEnd();
		if ( !IsEnded ) {
			UpdateEvent();
		}
	}

	bool CheckGameEnd() {
		foreach ( var holder in Holders ) {
			if ( holder.Count <= 0 ) {
				OnGameEnd(holder.Resource);
				return true;
			}
		}
		return false;
	}

	void OnGameEnd(Resource resource) {
		Events.Fire(new Game_End(resource));
		SaveResult(Turn);
	}

	void SaveResult(int turn) {
		var save = Save.GetNode<GameSave>();
		save.BestResult = Math.Max(save.BestResult, turn);
		Save.SaveNode(save);
	}

	Event SelectEvent() {
		// TODO: Checks and store
		return RandomUtils.GetItem(GameEvents.Events);
	}

	void UpdateEvent() {
		var newEvent = SelectEvent();
		Events.Fire(new Event_New(newEvent));
	}

	void UpdateResource(Resource resource, int value) {
		foreach ( var holder in Holders ) {
			if ( holder.Resource == resource ) {
				holder.Count += value;
				Events.Fire(new Resource_Update(holder));
			}
		}
	}

	void InitHolders() {
		foreach ( var res in Resources.Resources ) {
			var holder = new ResourceHolder(res, StartCount);
			Holders.Add(holder);
			Events.Fire(new Resource_New(holder));
		}
	}

	void InitRegions() {
		foreach ( var region in Regions ) {
			region.UpdateLevel(0);
		}
	}
}
