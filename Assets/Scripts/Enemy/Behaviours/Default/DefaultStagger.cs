using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultStagger : AIBehaviour
{
    [Header("Properties")]
    public float staggerTime = 2.0f;

    private bool _isStaggered = false;

    public override void OnEnter() 
    {
        if(!_isStaggered)
            StartCoroutine(StaggerSequence());
    }

    public override void OnExit(){}

    public override void OnFixedUpdate() {}

    public override void OnUpdate() {}

    private IEnumerator StaggerSequence()
    {
        _isStaggered = true;
        yield return new WaitForSecondsRealtime(staggerTime);
        _isStaggered = false;
        brain.SetBehaviour("Movement");
    }
}
