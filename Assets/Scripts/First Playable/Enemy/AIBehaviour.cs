using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBehaviour : MonoBehaviour
{
    protected AIBrain brain;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    protected Rigidbody rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    protected Transform player;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    protected Transform transform;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    protected EnemyHandler enemyHandler;

    public void InitialiseState(AIBrain brain)
    {
        this.brain = brain;
        this.player = brain.playerTransform;
        this.rigidbody = brain.GetRigidbody();
        this.transform = brain.GetTransform();
        this.enemyHandler = brain.GetHandler();
    }

    abstract public void OnEnter();
    abstract public void OnUpdate();
    abstract public void OnFixedUpdate();
    abstract public void OnExit();
}
