using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultIdle : AIBehaviour
{
    [Header("Properties")]
    public float initialDetectionDistance = 15.0f;

    public override void OnEnter() { }

    public override void OnExit() { }

    public override void OnFixedUpdate() { }

    public override void OnUpdate()
    {
        if (brain.GetDistanceToPlayer() < initialDetectionDistance)
            brain.SetBehaviour("Movement");
    }
}
