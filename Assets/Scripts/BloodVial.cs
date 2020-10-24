using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BloodVial : MonoBehaviour {
    [Tooltip("Invoked when the vial collides and breaks")]
    public UnityEvent onVialBreak;

    public float breakVelocity;
    public float alertRadius;
    public float lifetime;
    [Tooltip("Extra seconds appended to the lifetime amount, to avoid nullreference exceptions in downstream scripts while we destroy this")]
    public float lifetimeDestroyBuffer;
    public LayerMask enemyLayerMask;

    private void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude > breakVelocity) {
            onVialBreak.Invoke();
            AlertNearbySharks();
            Destroy(gameObject, lifetime + lifetimeDestroyBuffer);
        }
    }

    public List<Shark> FindSharksInRadius() {
        List<Shark> sharksInRadius = new List<Shark>();

        // Find all sharks in some radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, alertRadius, enemyLayerMask);
        Shark tmpShark;
        foreach (Collider c in hitColliders) {
            // Check for a shark component in the collider's gameobject
            tmpShark = c.GetComponent<Shark>();
            if (tmpShark != null) {
                sharksInRadius.Add(tmpShark);
                break;
            }

            // ... and in its parent gameobject
            tmpShark = c.GetComponentInParent<Shark>();
            if (tmpShark != null) {
                sharksInRadius.Add(tmpShark);
                break;
            }
        }

        return sharksInRadius;
    }

    public void AlertNearbySharks() {
        List<Shark> sharks = FindSharksInRadius();

        foreach (Shark s in sharks) {
            s.EnterDistractedState(this);
        }
    }
}
