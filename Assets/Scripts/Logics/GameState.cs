using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;

public class GameState : MonoBehaviour {
	public int StartCount;

	public int Turn;

	public bool IsEnded;

	public ResourceSetup Resources;

	public List<ResourceHolder> Holders;

	public List<Region> Regions;

	void Awake() {
		Events.Subscribe<Region_Update>(this, OnRegionUpdate);
	}

	void OnDestroy() {
		Events.Unsubscribe<Region_Update>(OnRegionUpdate);
	}

	void OnRegionUpdate(Region_Update e) {
		RecalculateResources();
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
	}

	bool CheckGameEnd() {
		foreach ( var holder in Holders ) {
			if ( holder.Count <= 0 ) {
				Events.Fire(new Game_End(holder.Resource));
				return true;
			}
		}
		return false;
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
