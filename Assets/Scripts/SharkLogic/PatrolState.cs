using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple lerp towards target for a shark
/// Since this is a state for a shark that can move in 3 dimensions (ie: we're not bounding it by gravity since it's underwater), we cannot use Navmeshes.
/// Therefore, we're using a simple lerp for patroling.
/// </summary>
public class PatrolState : BaseState {
    public Transform sharkTransform;
    public Rigidbody sharkRigidbody;

    public Transform currentTravelPoint;
    public List<Transform> travelPoints;
    private int travelPointIndex;

    [Tooltip("The squared distance between the agent and the target, must be passed before the agent can stop at this waypoint")]
    public float sqrStopDistance;
    public float sqrSlowDistance;
    public float rotationSpeed;
    public float regularSpeed, slowSpeed;

    public override void OnStateEnter(BaseState previousState) {
        base.OnStateEnter(previousState);

        currentTravelPoint = travelPoints[travelPointIndex];
    }
    public override void OnStateExit(BaseState nextState) {
        base.OnStateExit(nextState);
    }

    private void FixedUpdate() {
        // TODO: Check for player

        if (travelPoints.Count > 0) {
            // Check if we've arrived
            if (GetSqrDistanceToCurrentWaypoint() < sqrStopDistance) {
                travelPointIndex = (travelPointIndex + 1) % travelPoints.Count;
                currentTravelPoint = travelPoints[travelPointIndex];
            }

            // Lerp rotation towards target
            Vector3 dirToTarget = (currentTravelPoint.position - transform.position);
            sharkRigidbody.MoveRotation(Quaternion.Lerp(sharkTransform.rotation, 
                Quaternion.LookRotation(dirToTarget),
                rotationSpeed * Time.deltaTime));

            // Determine if we're close enough to the point to slow down
            float speed;
            if (GetSqrDistanceToCurrentWaypoint() < sqrSlowDistance) {
                speed = slowSpeed;
            } else {
                speed = regularSpeed;
            }

            // Lerp forward 
            sharkRigidbody.MovePosition(sharkTransform.position + (sharkTransform.forward * speed));
        }
    }

    private float GetSqrDistanceToCurrentWaypoint() {
        if (travelPoints.Count == 0) {
            Debug.LogError("No travel points configured");
            return 0f;
        }

        return (currentTravelPoint.position - transform.position).sqrMagnitude;
    }
}
