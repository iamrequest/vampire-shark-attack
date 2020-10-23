using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SwimmingLocomotionSettings")]
public class SwimmingLocomotionSettings : ScriptableObject {
    public float dampeningPerFrame;
    public float baseSpeedMultiplier;

    [Tooltip("How much extra force is applied to the hand's motion, as the palm's normal vector approaches parallel with the hand's motion vector ")]
    public AnimationCurve handLikenessVelocityMultiplier;
}
