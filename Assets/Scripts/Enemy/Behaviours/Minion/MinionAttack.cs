using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAttack : AIBehaviour
{
    [Header("General Movement")]
    public bool lookAtPlayer = true;
    public float turnSpeed = 0.20f;

    [Header("References")]
    public Trigger weaponTrigger;
    public GameObject weaponToEnable;

    [Header("Timers")]
    public float transitionTime = 2.0f;
    public float timeBeforeAttack = 0.5f;

    [Header("Attack Properties")]
    public float attackForce = 10.0f;

    private bool _hasAttacked = false;
    private bool _canAttack = false;

    public override void OnEnter()
    {
        weaponToEnable.SetActive(true);
        _hasAttacked = false;
        StartCoroutine(TransitionTimer());
        StartCoroutine(BeforeAttackTimer());
    }

    public override void OnExit()
    {
        weaponToEnable.SetActive(false);
        _hasAttacked = false;
        _canAttack = false;
    }

    public override void OnFixedUpdate() {}

    public override void OnUpdate() 
    {
        // Get direction to player
        Vector3 dir = brain.GetDirectionToPlayer();

        // Rotate to face direction
        if(lookAtPlayer)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed);

        if (weaponTrigger.Enabled && weaponTrigger.Collider != null && _canAttack)
        {
            if (weaponTrigger.Collider.gameObject.CompareTag("Player") && !_hasAttacked)
            {
                brain.playerTransform.GetComponent<Rigidbody>().AddForce(dir * attackForce, ForceMode.Impulse);
                _hasAttacked = true;
                _canAttack = false;
            }

        }
    }

    private IEnumerator TransitionTimer()
    {
        yield return new WaitForSecondsRealtime(transitionTime);
        brain.SetBehaviour("Idle");
    }

    private IEnumerator BeforeAttackTimer()
    {
        yield return new WaitForSecondsRealtime(timeBeforeAttack);
        _canAttack = true;
    }
}
