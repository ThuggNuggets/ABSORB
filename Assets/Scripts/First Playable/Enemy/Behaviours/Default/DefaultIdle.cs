using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultIdle : AIBehaviour
{
    [Header("Properties")]
    public float initialDetectionDistance = 15.0f;

    public override void OnStateEnter() { }

    public override void OnStateExit() { }

    public override void OnStateFixedUpdate() { }

    public override void OnStateUpdate()
    {
        if (brain.GetDistanceToPlayer() < initialDetectionDistance)
            brain.SetBehaviour("Movement");
    }
}
