using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentVision : MonoBehaviour {
    private const bool DEBUG = true;

    public Transform eyeTransform;
    public LayerMask visibleLayers;
    public float visionDistance, visionRadius;

    public bool IsInSight(Vector3 target, string tag) {
        RaycastHit hit;

        if (Physics.Raycast(eyeTransform.position, target - eyeTransform.position, out hit, visionDistance, visibleLayers)) {
            // Something is in the way
            if (!hit.collider.CompareTag(tag)) {
                DrawDebugRay(hit.point, Color.yellow);
                return false;
            }

            // Within our viewing distance and not occluded, but it's out of our viewing angle
            if (!IsWithinViewingAngle(target)) {
                DrawDebugRay(target, Color.magenta);
                return true;
            }

            // No obstructions, and within our viewing angle
            DrawDebugRay(target, Color.green);
            return false;
        }

        // Target is too far away, or layers aren't configured right
        DrawDebugRay(target, Color.red);
        return false;
    }

    private bool IsWithinViewingAngle(Vector3 target) {
        float angleToTarget = Vector3.Angle(target - eyeTransform.position, eyeTransform.forward);
        return visionRadius > angleToTarget;
    }

    private void DrawDebugRay(Vector3 target, Color color) {
        if (DEBUG) {
            float drawDistance = Mathf.Min(visionDistance, (eyeTransform.position - target).magnitude);
            Debug.DrawRay(eyeTransform.position, (target - eyeTransform.position).normalized * drawDistance, color);
        }
    }
}
