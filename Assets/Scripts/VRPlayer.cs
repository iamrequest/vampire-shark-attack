using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour {
    // Singleton pattern
    private static VRPlayer _instance;
    public static VRPlayer instance {
        get {
            return _instance;
        }
    }

    public Waypoint currentWaypoint;
    private CharacterController characterController;

    private void Awake() {
        if (_instance != null) {
            Debug.LogError("There can only be one instance of VRPlayer!");
            Destroy(this);
        }
        _instance = this;
    }

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    public void SendToLastWaypoint(float delay) {
        StartCoroutine(DoSendToLastWaypoint(delay));
    }

    private IEnumerator DoSendToLastWaypoint(float delay) {
        yield return new WaitForSeconds(delay);

        // Prevent the CC from overwriting our movement
        characterController.enabled = false;

        transform.position = currentWaypoint.transform.position;
        transform.rotation = currentWaypoint.transform.rotation;

        characterController.enabled = true;
    }
}
