using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialParried : AIBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Properties")]
    public float exitTime = 3.0f;

    private bool _isAbsorbable = false;

    public override void OnEnter() 
    {
        _isAbsorbable = true;
        animator.SetBool("Parried", true);
        animator.SetBool("Attacking", false);
        StartCoroutine(ExitSequence());
    }

    public override void OnExit() 
    {
        animator.SetBool("Parried", false);
        _isAbsorbable = false;
    }

    public override void OnFixedUpdate() {}

    public override void OnUpdate() {}

    private IEnumerator ExitSequence()
    {
        yield return new WaitForSecondsRealtime(exitTime);
        _isAbsorbable = false;
        brain.SetBehaviour("Idle");
        brain.playerTransform.GetComponentInChildren<AbilityManager>().LastParriedEnemy = null;
    }

    public bool GetAbsorbable()
    {
        return _isAbsorbable;
    }
}
