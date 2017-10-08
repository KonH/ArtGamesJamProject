using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;

public class ResourceViewManager : MonoBehaviour {
	public ResourceView Prefab;
	public List<ResourceView> Views;
	public bool UsePrefab;

	void Awake() {
		Events.Subscribe<Resource_New>(this, OnNewResource);
		Events.Subscribe<Resource_Update>(this, OnUpdateResource);
		if ( UsePrefab ) {
			Prefab.gameObject.SetActive(false);
		}
	}

	void OnDestroy() {
		Events.Unsubscribe<Resource_New>(OnNewResource);
		Events.Unsubscribe<Resource_Update>(OnUpdateResource);
	}

	void OnNewResource(Resource_New e) {
		ResourceView view;
		if ( UsePrefab ) {
			var instance = Instantiate(Prefab.gameObject, Vector3.zero, Quaternion.identity, transform);
			instance.SetActive(true);
			view = instance.GetComponent<ResourceView>();
			Views.Add(view);
		} else {
			view = Views.Where(v => v.Owner.Resource == e.Holder.Resource).First();
		}
		view.Init(e.Holder, e.MaxValue);
		view.UpdateStatus(e.Holder);
	}

	void OnUpdateResource(Resource_Update e) {
		foreach ( var view in Views ) {
			view.UpdateStatus(e.Holder);
		}
	}
}
