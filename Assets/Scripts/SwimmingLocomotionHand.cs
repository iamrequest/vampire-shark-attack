using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// A hand that propels the player forward. This class can be used to calculate how much this hand is propelling the player.
/// Actual locomotion takes place in SwimmingLocomotion
/// 
/// Assumption: The transform that this component is attached to is rotated in such a way that transform.forward is roughly perpendicular to the palm
/// </summary>
public class SwimmingLocomotionHand : MonoBehaviour {
    private Hand hand;

    [HideInInspector]
    public SwimmingLocomotion swimmingLocomotion;

    [Tooltip("Used to determine whether or not this hand is actively propelling the player")]
    public SteamVR_Action_Boolean gripAction;
    [Tooltip("Used to calculate motion of the hand each frame")]
    public SteamVR_Action_Skeleton skeletonAction;

    public bool isLocomotionActive {
        get {
            return isGripOpen && !isHoldingInteractable;
        }
    }

    public bool isGripOpen;
    public bool isHoldingInteractable;
    public float timeSinceLastObjectRelease;

    // Start is called before the first frame update
    void Awake() {
        hand = GetComponentInParent<Hand>();
    }

    private void Start() {
        gripAction.AddOnChangeListener(SetIsLocomotionActive, hand.handType);
        isGripOpen = true;
        isHoldingInteractable = true;
    }

    private void SetIsLocomotionActive(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState) {
        // If we just started a grip, check if we're holding an object
        if (newState) {
            isHoldingInteractable = (hand.currentAttachedObject != null);
        } else {
            // Check if we just released an object. If so, we need to wait a bit before producing motion
            if (isHoldingInteractable) {
                timeSinceLastObjectRelease = 0f;
                return;
            }
        }

        isGripOpen = !newState;
    }

    private void Update() {
        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, swimmingLocomotion.transform.rotation * skeletonAction.velocity, Color.cyan);

        // If we just released an object
        //if (isHoldingInteractable && hand.currentAttachedObject == null) {
        //    timeSinceLastObjectRelease += Time.deltaTime;

        //}
        //timeSinceLastObjectRelease += Time.deltaTime;

        //// TODO: This section is breaking things, since timeSinceLastObjectRelease is greater than the threshold. Not being reset
        //// If we've waited long enough, clear the "isHoldingObject" flag.
        //// This will let the player move with this hand again
        //if (timeSinceLastObjectRelease > swimmingLocomotion.postObjectReleaseLocomotionDelay) {
        //    isHoldingInteractable = false;
        //}
    }

    /// <summary>
    /// Calculates how much this hand is propelling the player.
    /// </summary>
    /// <returns>A Vector3 representing how much this hand is propelling the player</returns>
    public Vector3 CalculateMotion() {
        // This prevents a bug that sends the player flying, when the controllers aren't connected
        if (!skeletonAction.deviceIsConnected) {
            return Vector3.zero;
        }

        if (isLocomotionActive) {
            // -- Open hand. Move the player with this hand
            // Determine if the hand is moving in the same dir as the palm's dir
            // We multiply the skeleton action by the player transform, to account for the player's rotation
            float handVelocityLikeness = Vector3.Dot(swimmingLocomotion.transform.rotation * skeletonAction.velocity, transform.forward);

            // Multiply this by our animation curve, to get a proper multiplier scaled to our needs
            // Clamp the speed multiplication between [-1 and 1], so we get a scaled/uniform output. Likely not needed, but just in case
            handVelocityLikeness = Mathf.Clamp(swimmingLocomotion.locomotionSettings.handLikenessVelocityMultiplier.Evaluate(handVelocityLikeness), -1, 1);

            // Invert our motion, since we want to propel the player away from the hand's motion
            return -(swimmingLocomotion.transform.rotation * skeletonAction.velocity)
                * handVelocityLikeness
                * swimmingLocomotion.locomotionSettings.baseSpeedMultiplier;
        } else {
            // Closed fist. Do not move the player
            return Vector3.zero;
        }
    }
}
