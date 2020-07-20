using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMovement : AIBehaviour
{
    [Header("General Movement")]
    public float acceleration = 50.0f;
    public float maxVelocity = 10.0f;
    public float turnSpeed = 0.20f;

    [Header("Sprinting")]
    public float sprintAcceleration = 75.0f;
    public float rangeOfSprint = 7.5f;

    [Header("Attack Transition / Avoid")]
    public float avoidPushbackForce = 5.0f;
    public float attackPushback = 2.5f;
    public float attackDistance = 2.0f;

    // Attack RNG
    private int _attackRng = 0;

    public override void OnEnter() {}

    public override void OnExit() {}

    public override void OnFixedUpdate() 
    {
        // Get direction towards player
        Vector3 dir = brain.GetDirectionToPlayer();

        // Rotate to face direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed);

        // Move rigidbody forwards
        if (rigidbody.velocity.magnitude < maxVelocity)
            rigidbody.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse);

        // Check if close enough to enter attack state
        if (brain.GetDistanceToPlayer() < attackDistance)
        {
            if(_attackRng < 1)
            {
                brain.SetBehaviour("Attack");
                rigidbody.AddForce(-dir * attackPushback, ForceMode.Impulse);
                _attackRng = Random.Range(0, 3);
            }
            else
            {
                rigidbody.AddForce(-dir * avoidPushbackForce, ForceMode.Impulse);
                _attackRng = Random.Range(0, 3);
            }
        }
    }

    public override void OnUpdate() {}
}
