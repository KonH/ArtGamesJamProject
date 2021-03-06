﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using UDBase.Controllers.SceneSystem;
using UDBase.Controllers.SaveSystem;
using UDBase.Controllers.LeaderboardSystem;
using UDBase.Controllers.UserSystem;
using UDBase.Controllers.LogSystem;
using UDBase.Controllers.AudioSystem;
using UDBase.Utils;

public class GameState : MonoBehaviour {
	public int StartCount;
	public int StartLevel;
	public int MaxResource;
	public int Turn;
	public bool IsEnded;
	public ResourceSetup Resources;
	public EventSetup GameEvents;
	public List<ResourceHolder> Holders;
	public List<Region> Regions;
	public List<EventHistory> DoneEvents;

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

	void Start() {
		InitHolders();
		InitRegions();
		RecalculateResources();
		OnNewTurn();
		UpdateEvent();
		Events.Fire(new Game_Start());
		Audio.UnMuteMusic();
	}

	void OnUserRestart(User_Restart e) {
		Scene.ReloadScene();
	}

	void OnRegionUpdate(Region_Update e) {
		RecalculateResources();
	}

	void OnUserCase(User_Case e) {
		ApplyCase(e.Event, e.Case);
		NextTurn();
	}

	void OnUserMenu(User_Menu e) {
		Scene.LoadSceneByName("Menu");
	}

	void ApplyCase(Event ev, EventCase cs) {
		foreach ( var resChange in cs.Resources ) {
			UpdateResource(resChange.Resource, resChange.Value);
		}
		foreach ( var regChange in cs.Regions ) {
			UpdateRegion(regChange.Name, regChange.UpDown);
		}
		var csIndex = ev.Cases.IndexOf(cs);
		DoneEvents.Add(new EventHistory(ev, csIndex));
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
		var save = Save.GetNode<GameSave>();
		var hasEnding = save.Endings.Contains(resource.Name);
		Events.Fire(new Game_End(resource, !hasEnding));
		Audio.MuteMusic();
		SaveResult(Turn);
		if ( !hasEnding ) {
			save.Endings.Add(resource.Name);
			Save.SaveNode(save);
		}
	}

	void SaveResult(int turn) {
		var save = Save.GetNode<GameSave>();
		if ( turn > save.BestResult ) {
			Leaderboard.PostScore("", User.Name, turn, (_) => {});
		}
		save.BestResult = Math.Max(save.BestResult, turn);
		Save.SaveNode(save);
	}

	Event SelectEvent() {
		var filterByDepend = GameEvents.Events.Where(e => IsDependSelected(e.EventDepend, e.CaseDepend)).ToList();
		DebugEvents("FilterByDepend", filterByDepend);
		var filterByApplyable = FilterByApplyable(filterByDepend);
		DebugEvents("FilterByApplyable", filterByApplyable);
		var filterByUsage = FilterByUsage(SelectNonEmpty(filterByApplyable, filterByDepend));
		DebugEvents("FilterByUsage", filterByUsage);
		var selection = SelectNonEmpty(filterByUsage, filterByApplyable, filterByDepend);
		DebugEvents("Selection", selection);
		return RandomUtils.GetItem(selection);
	}

	List<Event> SelectNonEmpty(params List<Event>[] collections) {
		foreach ( var col in collections ) {
			if ( col.Count > 0 ) {
				return col;
			}
		}
		return null;
	}

	List<Event> FilterByUsage(List<Event> input) {
		var filterByUsage = new List<Event>();
		foreach ( var ev in input ) {
			var exists = false;
			foreach ( var eh in DoneEvents ) {
				if ( eh.Event == ev ) {
					exists = true;
					break;
				}
			}
			if ( !exists ) {
				filterByUsage.Add(ev);
			}
		}
		return filterByUsage;
	}

	bool CanLevelDownRegion(string regName) {
		foreach ( var reg in Regions ) {
			if ( reg.Name == regName ) {
				return reg.Level > 0;
			}
		}
		return false;
	}

	bool CanApplyEvent(Event ev) {
		foreach ( var cs in ev.Cases ) {
			foreach ( var reg in cs.Regions ) {
				if ( !reg.UpDown && !CanLevelDownRegion(reg.Name) ) {
					return false;
				}
			}
		}
		return true;
	}

	List<Event> FilterByApplyable(List<Event> input) {
		var filterByApplyable = new List<Event>();
		foreach ( var ev in input ) {
			if ( CanApplyEvent(ev) ) {
				filterByApplyable.Add(ev);
			}
		}
		return filterByApplyable;
	}

	void DebugEvents(string logName, List<Event> events) {
		if ( Log.IsActive() ) {
			var evStr = logName + " (" + events.Count + "): ";
			foreach ( var ev in events ) {
				evStr += ev.name + "; ";
			}
			Log.Message(evStr, LogTags.State);
		}
	}

	bool IsDependSelected(Event ev, int csIndex) {
		if ( !ev ) {
			return true;
		}
		return DoneEvents.Any(eh => (eh.Event == ev) && (eh.CaseIndex == csIndex));
	}

	void UpdateEvent() {
		var newEvent = SelectEvent();
		Events.Fire(new Event_New(newEvent));
	}

	void UpdateResource(Resource resource, int value) {
		foreach ( var holder in Holders ) {
			if ( holder.Resource == resource ) {
				holder.Count += value;
				holder.Count = Mathf.Min(holder.Count, MaxResource);
				Events.Fire(new Resource_Update(holder));
			}
		}
	}

	void InitHolders() {
		foreach ( var res in Resources.Resources ) {
			var holder = new ResourceHolder(res, StartCount);
			Holders.Add(holder);
			Events.Fire(new Resource_New(holder, MaxResource));
		}
	}

	void InitRegions() {
		foreach ( var region in Regions ) {
			region.UpdateLevel(StartLevel);
		}
	}
}
