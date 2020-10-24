using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LookatPlayer : MonoBehaviour {
    public bool constraintRotationToYAxis;

    private void Update() {
        if (constraintRotationToYAxis) {
            transform.forward = Vector3.ProjectOnPlane(transform.position - Player.instance.hmdTransform.position, Vector3.up).normalized;
        } else {
            transform.LookAt(Player.instance.hmdTransform.position);
        }
    }
}
