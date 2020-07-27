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
    private Animator _animator;

    [Header("Timers")]
    public float transitionTime = 2.0f;
    public float timeBeforeAttack = 0.5f;

    [Header("Attack Properties")]
    public float attackForce = 10.0f;

    private bool _hasAttacked = false;
    private bool _canAttack = false;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        weaponToEnable.SetActive(true);
        _animator.SetBool("Attacking", true);
        _hasAttacked = false;
    }

    public override void OnExit()
    {
        weaponToEnable.SetActive(false);
        _animator.SetBool("Attacking", false);
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
                brain.playerTransform.GetComponent<Health>().TakeDamage(enemyHandler.GetDamage());
                _hasAttacked = true;
                _canAttack = false;
            }
        }
    }

    public void ActivateCheck()
    {
        _canAttack = true;
    }

    public void DeactivateCheck()
    {
        _canAttack = false;
        _animator.SetBool("Attacking", false);
        brain.SetBehaviour("Movement");
    }
}
