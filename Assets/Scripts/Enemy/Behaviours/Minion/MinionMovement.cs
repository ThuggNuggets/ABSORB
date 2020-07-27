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
    public float sprintMaxVelocity = 15.0f;
    public float rangeOfSprint = 7.5f;

    [Header("Dodge")]
    [Range(0.0f, 1.0f)]
    public float dodgeChance = 1.0f;
    public float dodgeAcceleration = 5.0f;
    public float dodgeMaxVelocity = 15.0f;
    public float dodgeDistance = 10.0f;

    [Header("Attack Transition")]
    public float attackRange = 2.0f;

    private bool _shouldDodge = false;

    public override void OnEnter() {}

    public override void OnExit() {}

    public override void OnFixedUpdate() 
    {
        // Get distance and direction towards player
        float dist = brain.GetDistanceToPlayer();
        Vector3 dir = brain.GetDirectionToPlayer();

        // Rotate to face direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed);

        // Check if close enough to attack
        if(dist <= attackRange && !_shouldDodge)
        {
            // Check if we should attack, or avoid.
            if(Random.value <= dodgeChance)
                _shouldDodge = true;
            else
            {
                brain.SetBehaviour("Attack");
                _shouldDodge = true;
            }
        }

        // Checking if the minion should dodge
        if(_shouldDodge)
        {
            // Move towards player faster if out of sprint range
            if (dist < dodgeDistance && rigidbody.velocity.magnitude < dodgeMaxVelocity)
            {
                rigidbody.AddForce(-transform.forward * dodgeAcceleration * Time.fixedDeltaTime, ForceMode.Impulse);
            }

            // Check if we should stop dodging
            if(dist >= dodgeDistance)
                _shouldDodge = false;

            return;
        }

        // Move towards player faster if out of sprint range
        if(dist > rangeOfSprint && rigidbody.velocity.magnitude < sprintMaxVelocity)
        {
            rigidbody.AddForce(transform.forward * sprintAcceleration * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        // Move towards player slower if within sprint range
        else if(rigidbody.velocity.magnitude < maxVelocity)
        {
            rigidbody.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    public override void OnUpdate() {}
}
