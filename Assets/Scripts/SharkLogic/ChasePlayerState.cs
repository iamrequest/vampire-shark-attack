using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// Agent has spotted the player, and is chasing the last seen position
/// </summary>
public class ChasePlayerState : BaseState {
    public UnityEvent onPlayerKill;
    public Shark shark;

    public Vector3 lastSeenPosition;

    [Tooltip("The amount of time elapsed without seeing the player, for the shark to return to its default state")]
    public float giveUpTime;
    private float giveUpTimeElapsed;

    public SharkMotorSettings motorSettings;

    public override void OnStateEnter(BaseState previousState) {
        base.OnStateEnter(previousState);

        lastSeenPosition = Player.instance.hmdTransform.position;
        giveUpTimeElapsed = 0f;
    }

    public override void OnStateExit(BaseState nextState) {
        base.OnStateExit(nextState);
    }


    private void FixedUpdate() {
        // Update the last seen position
        if (shark.vision.IsInSight(Player.instance.hmdTransform.position, "Player")) {
            lastSeenPosition = Player.instance.hmdTransform.position;
            giveUpTimeElapsed = 0f;
        } else {
            giveUpTimeElapsed += Time.deltaTime;
        }

        // If we haven't seen the player in some time, give up and revert to the last state.
        if (giveUpTimeElapsed > giveUpTime) {
            parentFSM.TransitionTo(shark.resetState);
        }


        // Lerp to the last seen position
        Vector3 dirToLastSeenPosition = lastSeenPosition - shark.transform.position;
        shark.rb.MoveRotation(Quaternion.Lerp(shark.transform.rotation, 
            Quaternion.LookRotation(dirToLastSeenPosition),
            motorSettings.rotationSpeed * Time.deltaTime));

        // Lerp forward, if we're still sufficiently far enough away from it
        if (dirToLastSeenPosition.magnitude > motorSettings.stopDistance) {
            float forwardSpeed = motorSettings.GetMotorSpeed(GetSqrDistanceTo(lastSeenPosition));
            shark.rb.MovePosition(shark.transform.position + (shark.transform.forward * forwardSpeed));
        }

        // If within stopping range, then do damage to the player
        if (GetSqrDistanceTo(Player.instance.hmdTransform.position) < motorSettings.stopDistance) {
            onPlayerKill.Invoke();

            // Kill the player
            VRPlayer.instance.KillPlayer();

            // Restore this shark to its reset state/transform. See Shark.cs.
            // TODO: Reset all sharks?
            shark.DoResetShark();
        }
    }

    private float GetSqrDistanceTo(Vector3 targetPosition) {
        return (targetPosition - shark.transform.position).magnitude;
    }
}
