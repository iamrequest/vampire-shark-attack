using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores common references to components used by states
/// </summary>
public class Shark : MonoBehaviour {
    private FiniteStateMachine fsm;
    public DistractedState distractedState;

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
        fsm = GetComponent<FiniteStateMachine>();
        m_rb = GetComponent<Rigidbody>();
        m_vision = GetComponent<AgentVision>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            fsm.TransitionTo(distractedState);
        }
    }
}
