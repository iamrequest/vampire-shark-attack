using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Agent is blind to the target for some duration, and will move towards the target game object
/// </summary>
public class DistractedState : BaseState {
    public Shark shark;
    public Transform target;

    [Tooltip("The state that the agent will go to next, after the distraction delay has elapsed")]
    public BaseState postDelayState;

    [Tooltip("How long the agent will stay distracted for, in seconds")]
    public float distractionTime;

    public SharkMotorSettings motorSettings;


    public override void OnStateEnter(BaseState previousState) {
        base.OnStateEnter(previousState);
        StartCoroutine(StartStateAfterDelay());
    }

    public override void OnStateExit(BaseState nextState) {
        base.OnStateExit(nextState);
    }

    private void FixedUpdate() {
        // Rotate to face the target
        Vector3 dirToTarget = (target.position - shark.transform.position);
        shark.rb.MoveRotation(Quaternion.Lerp(shark.transform.rotation, 
            Quaternion.LookRotation(dirToTarget),
            motorSettings.rotationSpeed * Time.deltaTime));

        // Lerp forward, if we're still sufficiently far enough away from it
        if (dirToTarget.sqrMagnitude > motorSettings.sqrStopDistance) {
            float forwardSpeed = motorSettings.GetMotorSpeed(GetSqrDistanceToCurrentWaypoint());
            shark.rb.MovePosition(shark.transform.position + (shark.transform.forward * forwardSpeed));
        }
    }

    public IEnumerator StartStateAfterDelay() {
        yield return new WaitForSeconds(distractionTime);
        parentFSM.TransitionTo(postDelayState);
    }

    private float GetSqrDistanceToCurrentWaypoint() {
        return (target.position - shark.transform.position).sqrMagnitude;
    }
}
