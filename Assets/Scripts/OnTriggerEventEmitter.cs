using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEventEmitter : MonoBehaviour {
    public GameObject target;
    public UnityEvent onTriggerEvent;

    private void OnTriggerEnter(Collider other) {
        // Quick way to confirm the object is what we expect
        // Better way would be to compare tag, or layer
        if (other.gameObject == target) {
            onTriggerEvent.Invoke();
        }
    }
}
