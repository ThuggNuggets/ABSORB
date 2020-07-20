using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
    protected AIBrain brain;
    protected Rigidbody rigidbody;
    protected Transform player;
    protected Transform transform;

    public void InitialiseState(AIBrain brain)
    {
        this.brain = brain;
        this.player = brain.playerTransform;
        this.rigidbody = brain.GetRigidbody();
        this.transform = brain.GetTransform();
    }

    abstract public void OnEnter();
    abstract public void OnUpdate();
    abstract public void OnFixedUpdate();
    abstract public void OnExit();
}
