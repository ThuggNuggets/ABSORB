using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMovement : AIBehaviour
{
    [Header("General Movement")]
    public float acceleration = 50.0f;
    public float retreatDashAcceleration = 75.0f;
    public float retreatMaxVelocity = 20.0f;
    public float maxVelocity = 10.0f;
    public float turnSpeed = 0.20f;

    [Header("Attack")]
    public float attackDistance = 15.0f;
    public float retreatDistance = 15.0f;
    public float beforeAttackTimer = 0.7f;
    private bool _isWaitingToAttack = false;

    public override void OnEnter() 
    {
        _isWaitingToAttack = false;
    }

    public override void OnExit() {}

    public override void OnFixedUpdate()
    {
        // If the enemy is already waiting to attack, then exit this out of this function
        if (_isWaitingToAttack)
            return;

        // Get direction and distance from player
        Vector3 dir = brain.GetDirectionToPlayer();
        float dist = brain.GetDistanceToPlayer();

        // Rotate to face direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed);

        // Retreat from player if they get too close
        if (dist < attackDistance)
        {
            // Only adding force if velocity is under max velocity
            if (rigidbody.velocity.magnitude < retreatMaxVelocity)
            {
                // Moving towards player if out of attack distance
                rigidbody.AddForce(-transform.forward * retreatDashAcceleration * Time.fixedDeltaTime, ForceMode.Impulse);
            }
        }

        // Only adding force if velocity is under max velocity
        if (rigidbody.velocity.magnitude < maxVelocity && dist > attackDistance)
        {
            // Moving towards player if out of attack distance
            rigidbody.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        // If the enemy is at the optimal attack range, enter the attack state
        if (Mathf.Ceil(dist) == attackDistance)
            StartCoroutine(AttackSequence());
    }

    public override void OnUpdate() {}

    public IEnumerator AttackSequence()
    {
        _isWaitingToAttack = true;
        yield return new WaitForSecondsRealtime(beforeAttackTimer);
        brain.SetBehaviour("Attack");
        _isWaitingToAttack = false;
    }
}
