using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;

public class ResourceViewManager : MonoBehaviour {
	public ResourceView Prefab;
	public List<ResourceView> Views; 

	void Awake() {
		Events.Subscribe<Resource_New>(this, OnNewResource);
		Events.Subscribe<Resource_Update>(this, OnUpdateResource);
		Prefab.gameObject.SetActive(false);
	}

	void OnDestroy() {
		Events.Unsubscribe<Resource_New>(OnNewResource);
		Events.Unsubscribe<Resource_Update>(OnUpdateResource);
	}

	void OnNewResource(Resource_New e) {
		var instance = Instantiate(Prefab.gameObject, Vector3.zero, Quaternion.identity, transform);
		instance.SetActive(true);
		var view = instance.GetComponent<ResourceView>();
		Views.Add(view);
		view.Init(e.Holder, e.MaxValue);
		view.UpdateStatus(e.Holder);
	}

	void OnUpdateResource(Resource_Update e) {
		foreach ( var view in Views ) {
			view.UpdateStatus(e.Holder);
		}
	}
}
