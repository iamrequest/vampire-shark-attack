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

    public bool isLocomotionActive;

    // Start is called before the first frame update
    void Awake() {
        hand = GetComponentInParent<Hand>();
    }

    private void Start() {
        gripAction.AddOnChangeListener(SetIsLocomotionActive, hand.handType);
        isLocomotionActive = true;
    }

    private void SetIsLocomotionActive(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState) {
        isLocomotionActive = !newState;
    }

    private void Update() {
        Debug.DrawRay(transform.position, transform.forward);
        Debug.DrawRay(transform.position, swimmingLocomotion.transform.rotation * skeletonAction.velocity, Color.cyan);
    }

    /// <summary>
    /// Calculates how much this hand is propelling the player.
    /// </summary>
    /// <returns>A Vector3 representing how much this hand is propelling the player</returns>
    public Vector3 CalculateMotion() {
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
