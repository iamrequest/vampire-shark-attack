using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores common references to components used by states
/// </summary>
public class Shark : MonoBehaviour {
    public DistractedState distractedState;

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
    }


    public void EnterDistractedState(BloodVial bloodVial) {
        distractedState.bloodVial = bloodVial;
        fsm.TransitionTo(distractedState);
    }
}
