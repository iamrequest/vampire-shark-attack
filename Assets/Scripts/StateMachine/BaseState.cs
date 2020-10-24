using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseState : MonoBehaviour {
    public FiniteStateMachine parentFSM;
    public UnityEvent onStateEnter, onStateExit;

    /// <summary>
    /// Disable the state when the game starts, to prevent multiple states from being on at the same time
    /// The initial state will be enabled in FiniteStateMachine.Start()
    /// </summary>
    public virtual void Awake() {
        enabled = false;
    }

    public virtual void OnStateEnter(BaseState previousState) {
        onStateEnter.Invoke();
    }
    public virtual void OnStateExit(BaseState nextState) {
        onStateExit.Invoke();
    }
}
