using System;
using System.Collections;
using System.Collections.Generic;
using SO.Events;
using UnityEngine;
using Utility;

public class LocationLoadTrigger : MonoBehaviour {
    [SerializeField] Trigger trigger;
    [SerializeField] SceneLoadRequestEventSO loadRequestEvent;
    [SerializeField] SceneLoadRequestEventSO.SceneLoadRequestData data;

    void Start() {
        trigger.Enter += RequestLocationLoad;
    }

    void RequestLocationLoad(Collider other) {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        
        loadRequestEvent.Invoke(data);
    }
}
