using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character Controller locomotion based on hand movement. 
/// Propels player away from hand motion, when the normal of the player's palms are near-parallel to the motion dir
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SwimmingLocomotion : MonoBehaviour {
    private CharacterController characterController;
    public List<SwimmingLocomotionHand> swimmingLocomotionHands;

    [Header("Locomotion Speed Settings")]
    private Vector3 motion;
    public float dampeningPerFrame;
    public float baseSpeedMultiplier;

    [Tooltip("How much extra force is applied to the hand's motion, as the palm's normal vector approaches parallel with the hand's motion vector ")]
    public AnimationCurve handLikenessVelocityMultiplier;


    void Awake() {
        characterController = GetComponent<CharacterController>();

        foreach (SwimmingLocomotionHand swimmingHand in swimmingLocomotionHands) {
            swimmingHand.swimmingLocomotion = this;
        }
    }

    void FixedUpdate() {
        ApplyMotionFalloff();

        // Figure out how much each hand is propelling the player
        foreach (SwimmingLocomotionHand swimmingHand in swimmingLocomotionHands) {
            motion += swimmingHand.CalculateMotion();
        }

        // Move the player accordingly
        characterController.Move(motion);
    }

    // TODO: Snap to zero if magnitude is very small?
    private void ApplyMotionFalloff() {
        motion *= dampeningPerFrame;
    }
}
