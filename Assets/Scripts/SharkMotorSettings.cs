using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SharkMotorSettings")]
public class SharkMotorSettings: ScriptableObject {
    [Tooltip("The squared distance between the agent and the target, must be passed before the agent can slow/stop at this waypoint")]
    public float sqrStopDistance, sqrSlowDistance;

    public float regularSpeed, slowSpeed;
    public float rotationSpeed;

    public float GetMotorSpeed(float distanceToTarget) {
        if (distanceToTarget < sqrSlowDistance) {
            return slowSpeed;
        } else {
            return regularSpeed;
        }
    }
}
