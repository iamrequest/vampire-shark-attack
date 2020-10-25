using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SharkMotorSettings")]
public class SharkMotorSettings: ScriptableObject {
    [Tooltip("The distance between the agent and the target, must be passed before the agent can slow/stop at this waypoint")]
    public float stopDistance, slowDistance;

    public float regularSpeed, slowSpeed;
    public float rotationSpeed;

    public float GetMotorSpeed(float distanceToTarget) {
        if (distanceToTarget < slowDistance) {
            return slowSpeed;
        } else {
            return regularSpeed;
        }
    }
}
