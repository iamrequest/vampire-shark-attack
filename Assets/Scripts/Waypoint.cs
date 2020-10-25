using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Waypoint : MonoBehaviour {
    [Tooltip("")]
    public int priority;
    public UnityEvent onWaypointSet;

    private void OnTriggerEnter(Collider other) {
        // Confirm that whatever entered the waypoint's trigger space is the player
        if (other.gameObject.CompareTag("Player")) {
            // If the player doesn't already have a waypoint, then set it.
            if (VRPlayer.instance.currentWaypoint == null) {
                VRPlayer.instance.currentWaypoint = this;
                onWaypointSet.Invoke();
            } else {
                // If the existing waypoint has lower priority than this one, then overwrite it
                if (VRPlayer.instance.currentWaypoint.priority < priority) {
                    onWaypointSet.Invoke();
                }
            }
        }
    }
}
