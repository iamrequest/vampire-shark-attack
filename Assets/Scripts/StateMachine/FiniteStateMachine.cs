using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour {
    public BaseState enterState;
    private BaseState m_currentState;
    public BaseState currentState {
        get {
            return m_currentState;
        }
    }

    private void Start() {
        m_currentState = enterState;

        enterState.enabled = true;
        enterState.parentFSM = this;
        enterState.OnStateEnter(null);
    }

    public void TransitionTo(BaseState newState) {
        newState.parentFSM = this;

        // Transition out of current state
        currentState.OnStateExit(newState);
        currentState.enabled = false;

        // Transition into new state
        newState.enabled = true;
        newState.OnStateEnter(currentState);
        m_currentState = newState;
    }
}
