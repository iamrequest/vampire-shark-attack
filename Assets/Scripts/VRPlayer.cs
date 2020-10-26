using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRPlayer : MonoBehaviour {
    // Singleton pattern
    private static VRPlayer _instance;
    public static VRPlayer instance {
        get {
            return _instance;
        }
    }

    public Animator deathFadeAnimator;
    public Waypoint currentWaypoint;
    private CharacterController characterController;
    public float deathAnimationDuration;
    private bool isDying;

    private void Awake() {
        if (_instance != null) {
            Debug.LogError("There can only be one instance of VRPlayer!");
            Destroy(this);
        }
        _instance = this;
    }

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    public void KillPlayer() {
        // Avoid doing onDeath tasks while the player is already being killed
        if (!isDying) {
            isDying = true;
            deathFadeAnimator.SetTrigger("KillPlayer");
            StartCoroutine(DoSendToLastWaypoint(deathAnimationDuration));
        }
    }

    private IEnumerator DoSendToLastWaypoint(float delay) {
        yield return new WaitForSeconds(delay);

        isDying = false;

        // Prevent the CC from overwriting our movement
        characterController.enabled = false;

        transform.position = currentWaypoint.transform.position;
        transform.rotation = currentWaypoint.transform.rotation;

        characterController.enabled = true;
    }
}
