using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BloodVialSpawner : MonoBehaviour {
    public float maxGrabDistance;
    public SteamVR_Action_Boolean grabAction;
    public GameObject bloodVialPrefab;


    private void Start() {
        // There's likely a better way of getting the hand, but it's late and the last day of the jam
        grabAction.AddOnStateDownListener(GrabBloodVialLeft, SteamVR_Input_Sources.LeftHand);
        grabAction.AddOnStateDownListener(GrabBloodVialRight, SteamVR_Input_Sources.RightHand);
    }

    private void GrabBloodVialLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        AttachBloodVial(Player.instance.leftHand);
    }
    private void GrabBloodVialRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        AttachBloodVial(Player.instance.rightHand);
    }

    private void AttachBloodVial(Hand hand) {
        if (hand.currentAttachedObject == null) {
            if ((transform.position - hand.transform.position).magnitude < maxGrabDistance) {
                GameObject bloodVial = Instantiate(bloodVialPrefab);
                hand.AttachObject(bloodVial, hand.GetBestGrabbingType());
            }
        }
    }
}
