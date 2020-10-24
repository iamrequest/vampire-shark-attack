using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

public class Key : MonoBehaviour {
    public Transform target;
    public float unlockDistance;
    private Interactable m_interactable;
    private Collider m_collider;

    public UnityEvent onUnlock;

    public float lerpToTargetTime;

    // Start is called before the first frame update
    void Start() {
        m_interactable = GetComponent<Interactable>();
        m_collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        float distanceToTarget = (target.position - transform.position).magnitude;
        if (distanceToTarget < unlockDistance) {
            UnlockTarget();
        }
    }

    public void UnlockTarget() {
        // Detach the object from the hand, and disable the collider component to prevent the player from grabbing it again
        if (m_interactable.attachedToHand != null) {
            m_interactable.attachedToHand.DetachObject(gameObject);

            m_collider.enabled = false;
        }

        onUnlock.Invoke();
    }

}
