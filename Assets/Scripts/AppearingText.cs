using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// On trigger, play an animation.
/// </summary>
[RequireComponent(typeof(Collider))]
public class AppearingText : MonoBehaviour {
    private Animator animator;
    public UnityEvent onTriggerEnter;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            animator.SetTrigger("ShowText");
            onTriggerEnter.Invoke();
        }
    }
}
