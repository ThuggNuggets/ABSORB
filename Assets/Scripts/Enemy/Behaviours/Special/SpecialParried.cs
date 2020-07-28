using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialParried : AIBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Properties")]
    public float exitTime = 3.0f;

    private AbilityManager playerAbililtyManager;

    private bool _isAbsorbable = false;

    private void Start()
    {
        playerAbililtyManager = brain.playerTransform.GetComponent<AbilityManager>();
    }

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

    public override void OnUpdate() 
    {
        if(!_isAbsorbable)
        {
            brain.SetBehaviour("Idle");
            playerAbililtyManager.LastParriedEnemy = null;
            playerAbililtyManager.SetAbsorbTarget(null);
        }
    }

    private IEnumerator ExitSequence()
    {
        yield return new WaitForSecondsRealtime(exitTime);
        _isAbsorbable = false;
    }

    public bool GetAbsorbable()
    {
        return _isAbsorbable;
    }
}
