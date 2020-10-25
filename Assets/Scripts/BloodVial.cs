using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A glass bottle that breaks on collision, alerting nearby sharks. Nearby sharks will enter the Distracted state, chasing after the bottle.
/// 
/// TODO: Currently this only alerts sharks at the moment the bottle breaks. It should also alert sharks while it's broken and leaking blood
/// </summary>
public class BloodVial : MonoBehaviour {
    private Renderer m_renderer;

    [Tooltip("Invoked when the vial collides and breaks")]
    public UnityEvent onVialBreak;

    [Tooltip("Invoked after the bottle collides, and $lifespan seconds have elapsed")]
    public UnityEvent onVialDissipate;

    private bool isBroken;

    public float breakVelocity;
    public float alertRadius;
    public float lifetime;
    public float vfxLifetime;
    [Tooltip("Extra seconds appended to the lifetime amount, to avoid nullreference exceptions in downstream scripts while we destroy this")]
    public float lifetimeDestroyBuffer;
    public LayerMask enemyLayerMask;

    private void Start() {
        m_renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision) {
        // Can only break once
        if (!isBroken) {
            // Don't break when colliding with the player
            if (collision.collider.CompareTag("Player")) {
                return;
            }

            if (collision.relativeVelocity.magnitude > breakVelocity) {
                isBroken = true;

                m_renderer.enabled = false;
                onVialBreak.Invoke();
                StartCoroutine(InvokeOnVialDissipateAfterDelay());

                AlertNearbySharks();
                Destroy(gameObject, lifetime + lifetimeDestroyBuffer);
            }
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

    private IEnumerator InvokeOnVialDissipateAfterDelay() {
        yield return new WaitForSeconds(vfxLifetime);
        onVialDissipate.Invoke();
    }
}
