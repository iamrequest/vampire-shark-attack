using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores common references to components used by states
/// </summary>
public class Shark : MonoBehaviour {
    public PatrolState patrolState;
    public DistractedState distractedState;
    public BaseState resetState;
    public float resetDelay;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private FiniteStateMachine m_fsm;
    public FiniteStateMachine fsm {
        get {
            return m_fsm;
        }
    }

    private Rigidbody m_rb;
    public Rigidbody rb {
        get {
            return m_rb;
        }
    }

    private AgentVision m_vision;
    public AgentVision vision {
        get {
            return m_vision;
        }
    }

    private void Start() {
        m_fsm = GetComponent<FiniteStateMachine>();
        m_rb = GetComponent<Rigidbody>();
        m_vision = GetComponent<AgentVision>();

        resetState = patrolState;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }


    public void EnterDistractedState(BloodVial bloodVial) {
        distractedState.bloodVial = bloodVial;
        fsm.TransitionTo(distractedState);
    }

    public void DoResetShark() {
        StartCoroutine(ResetShark());
    }

    private IEnumerator ResetShark() {
        yield return new WaitForSeconds(resetDelay);

        // Reset position/rotation/motion
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;

        // Go back to the default state
        // If the shark is cured, it'll stay that way, to make things easier for the player
        fsm.TransitionTo(resetState);
    }
}
