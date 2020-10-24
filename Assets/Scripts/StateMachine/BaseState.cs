using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour {
    public FiniteStateMachine parentFSM;

    public virtual void OnStateEnter(BaseState previousState) { }
    public virtual void OnStateExit(BaseState nextState) { }
}
