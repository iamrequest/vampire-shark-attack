using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

/// <summary>
/// Simple lerp towards target for a shark
/// Since this is a state for a shark that can move in 3 dimensions (ie: we're not bounding it by gravity since it's underwater), we cannot use Navmeshes.
/// Therefore, we're using a simple lerp for patroling.
/// </summary>
public class PatrolState : BaseState {
    public UnityEvent onPlayerSpotted;
    public Shark shark;

    public BaseState chaseState;

    public Transform currentTravelPoint;
    public List<Transform> travelPoints;
    private int travelPointIndex;

    public SharkMotorSettings motorSettings;

    public override void OnStateEnter(BaseState previousState) {
        base.OnStateEnter(previousState);

        // TODO: Likely would be best to pick the closest travel point when returning from some state (ie: previous state != null)
        currentTravelPoint = travelPoints[travelPointIndex];
    }
    public override void OnStateExit(BaseState nextState) {
        base.OnStateExit(nextState);
    }

    private void FixedUpdate() {
        // Check for player
        if(shark.vision.IsInSight(Player.instance.transform.position, "Player")) {
            onPlayerSpotted.Invoke();
            parentFSM.TransitionTo(chaseState);
        }

        // If 0 patrol points are specified, this is essentially just an idle state. Likely would look weird with just 0
        if (travelPoints.Count > 1) {
            // Check if we've arrived at the current waypoint. If so, move on to the next one
            if (GetSqrDistanceToCurrentWaypoint() < motorSettings.stopDistance) {
                travelPointIndex = (travelPointIndex + 1) % travelPoints.Count;
                currentTravelPoint = travelPoints[travelPointIndex];
            }
        }

        // Lerp to the current travel point
        if (travelPoints.Count > 0) {
            // Lerp rotation towards target
            Vector3 dirToTarget = (currentTravelPoint.position - shark.transform.position);
            shark.rb.MoveRotation(Quaternion.Lerp(shark.transform.rotation, 
                Quaternion.LookRotation(dirToTarget),
                motorSettings.rotationSpeed * Time.deltaTime));

            // Lerp forward 
            float forwardSpeed = motorSettings.GetMotorSpeed(GetSqrDistanceToCurrentWaypoint());
            shark.rb.MovePosition(shark.transform.position + (shark.transform.forward * forwardSpeed));
        }
    }

    private float GetSqrDistanceToCurrentWaypoint() {
        if (travelPoints.Count == 0) {
            Debug.LogError("No travel points configured");
            return 0f;
        }

        return (currentTravelPoint.position - shark.transform.position).magnitude;
    }
}
