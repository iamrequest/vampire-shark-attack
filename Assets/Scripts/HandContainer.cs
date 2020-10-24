using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// Half Life Alyx hand containers.
/// </summary>
public class HandContainer : MonoBehaviour {
    private Hand parentHand, targetHand;
    public SteamVR_Action_Boolean grabAction;
    public float maxGrabDistance;

    public GameObject storedObject;
    public GameObject storedObjectIcon;
    // Used for animating the stored object icon
    public Vector3 storedObjectRotationAngle;

    private void Start() {
        parentHand = GetComponentInParent<Hand>();
        targetHand = parentHand.otherHand;

        grabAction.AddOnStateDownListener(RetrieveItem, targetHand.handType);
        grabAction.AddOnStateUpListener(StoreItem, targetHand.handType);

        storedObjectIcon.SetActive(false);
    }

    private void FixedUpdate() {
        // Animate the stored icon object
        storedObjectIcon.transform.Rotate(storedObjectRotationAngle);
    }

    private void RetrieveItem(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        // Nothing to retrieve
        if (storedObject == null) return;
        if (!IsTargetHandInRange()) return;

        // Retrieve the object
        storedObject.SetActive(true);
        targetHand.AttachObject(storedObject, targetHand.GetBestGrabbingType());
        storedObject = null;

        // Disable the icon
        storedObjectIcon.SetActive(false);
    }

    private void StoreItem(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        if (storedObject != null) return;
        if (targetHand.currentAttachedObject == null) return;
        if (!IsTargetHandInRange()) return;

        // Store the object
        storedObject = targetHand.currentAttachedObject;
        targetHand.DetachObject(storedObject);
        storedObject.SetActive(false);

        // Enable the icon
        storedObjectIcon.SetActive(true);
    }

    private bool IsTargetHandInRange() {
        return (targetHand.transform.position - transform.position).magnitude < maxGrabDistance;
    }
}
