using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// On trigger, play an animation.
/// </summary>
[RequireComponent(typeof(Collider))]
public class AppearingText : MonoBehaviour {
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            animator.SetTrigger("ShowText");
        }
    }
}
