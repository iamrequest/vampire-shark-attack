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
    private Rigidbody m_rb;

    public UnityEvent onUnlock;

    public float lerpToTargetTime;
    public float postUnlockDestroyTime;
    private bool isUnlocking;

    // Start is called before the first frame update
    void Start() {
        m_interactable = GetComponent<Interactable>();
        m_collider = GetComponent<Collider>();
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!isUnlocking) {
            float distanceToTarget = (target.position - transform.position).magnitude;
            if (distanceToTarget < unlockDistance) {
                UnlockTarget();
            }
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UnlockTarget();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            m_collider.enabled = true;
        }
    }

    public void UnlockTarget() {
        isUnlocking = true;
        // Detach the object from the hand, and disable the collider component to prevent the player from grabbing it again
        if (m_interactable.attachedToHand != null) {
            m_interactable.attachedToHand.DetachObject(gameObject);

            m_collider.enabled = false;
            m_rb.isKinematic = true;
        }

        StartCoroutine(LerpTowardsTarget());
    }

    public IEnumerator LerpTowardsTarget() {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < lerpToTargetTime) {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, target.position, elapsedTime / lerpToTargetTime);
            transform.rotation = Quaternion.Lerp(startRotation, target.rotation, elapsedTime / lerpToTargetTime);

            yield return null;
        }

        onUnlock.Invoke();
        Destroy(gameObject, postUnlockDestroyTime);
    }
}
