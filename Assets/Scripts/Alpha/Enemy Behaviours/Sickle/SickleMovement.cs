﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SickleMovement : AIBehaviour
{
    [Header("Properties")]
    public float destinationPadding = 1.0f;
    public float avoidDistance = 30.0f;

    // A flag to determine if the enemy should avoid the player or not
    private bool _isAvoiding = false;

    // The inital speed; set from within the nav mesh component
    private float _initialSpeed = 0.0f;

    // The attack transition range; set from "stopping distance" within the nav mesh component
    private float _attackRange = 0.0f;

    // Called before first frame
    private void Start()
    {
        // Getting the initial speed from the nav mesh component
        _initialSpeed = brain.GetNavMeshAgent().speed;

        // Getting the attack range from the nav mesh component
        _attackRange = brain.GetNavMeshAgent().stoppingDistance;
    }

    public override void OnStateEnter()
    {
        // Checking if the state we came from was the attack behaviour
        if (brain.GetLastStateID() == "Attack")
        {
            Vector3 avoidDirection = (brain.PlayerTransform.position - transform.position).normalized;
            Vector3 avoidDestination = transform.position - (avoidDirection * avoidDistance);
            this.OverrideDestination(avoidDestination, 1.0f);
            _isAvoiding = true;
        }
        else
        {
            // Currently setting the on enter destination to the player; in the future we'll have to set the destination from a "EnemyAI Controller"
            this.LockDestinationToPlayer(destinationPadding);
        }
    }

    public override void OnStateUpdate()
    {
        // Checking if we should be locked onto the player or not...
        if (this.destinationLockedToPlayer)
            this.currentDestination = brain.PlayerTransform.position;

        // Updating the target destination every frame
        brain.UpdateTargetDestination(this.currentDestination, destinationPadding);

        // If player is within attack range;
        if (brain.GetNavMeshAgent().remainingDistance <= _attackRange)
        {
            // Enemy will enter attack phase if locked onto player:
            if (this.destinationLockedToPlayer)
            {
                brain.SetBehaviour("Attack");
                return;
            }
            else
            {
                if (_isAvoiding)
                {
                    this.LockDestinationToPlayer(destinationPadding);
                    _isAvoiding = false;
                }

                // Here is what they'll do when they aren't locked on
                // so general movement, stuff will go here when
                // the group system has been worked out
            }

        }
    }

    public override void OnStateFixedUpdate() { }

    public override void OnStateExit() { }

    // Returns a randomized position from the radius around the center of an object
    // This function will be replaced when "Unit Slotting" or "AI Group Control" gets implemented.
    private Vector3 GetRandomizedPositionAroundCenter(Vector3 center, float radius)
    {
        // create random angle between 0 to 360 degrees 
        float ang = Random.value * 360;
        Vector3 pos = Vector3.zero;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    /*
        General Movement: 
            - Circulates player at an average distance. (Enemy will just seek towards player and when at the attack distance of 
              the attack, they will circulate them at that distance before using the special attack.
              After attacking, they will jump back out to average distance and start ciruclating again.
              (example; refer to zelda BOTW AI attacks.)

        Attack:
            VARIENT (-"Hammer") Pretty much a lunge attack with a "hammer" swinging from overhead and hitting the player.
                       The effect will look pretty much identical to the players ability.

        When hit by player:
            - Doesn't stagger when hit by basic attacks, and damaged when hit by the hammer ability.

        On death:
            - Destoryed on death.
    */
    // [Header("General Movement")]
    // public float acceleration = 25.0f;
    // public float maxVelocity = 10.0f;
    // public float turnSpeed = 0.20f;

    // [Header("Attack")]
    // public float attackDistance = 10.0f;
    // public float beforeAttackTimer = 0.7f;
    // private bool _isWaitingToAttack = false;

    // public override void OnStateEnter() {}

    // public override void OnStateExit() 
    // {
    //     _isWaitingToAttack = false;
    // }

    // public override void OnStateFixedUpdate() 
    // {
    //     // If the enemy is already waiting to attack, then exit this out of this function
    //     if (_isWaitingToAttack)
    //         return;

    //     // Get direction and distance from player
    //     Vector3 dir = brain.GetDirectionToPlayer();
    //     float dist  = brain.GetDistanceToPlayer();

    //     // Rotate to face direction
    //     transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed);

    //     // Only adding force if velocity is under max velocity
    //     if (rigidbody.velocity.magnitude < maxVelocity)
    //     {
    //         // Moving towards player if out of attack distance
    //         if (dist > attackDistance)
    //             rigidbody.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse);
    //         else
    //             rigidbody.AddForce(-transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse);
    //     }

    //     // If the enemy is at the optimal attack range, enter the attack state
    //     if (Mathf.Ceil(dist) == attackDistance)
    //         StartCoroutine(BeforeAttackTimer());
    // }

    // public override void OnStateUpdate() { }

    // public IEnumerator BeforeAttackTimer()
    // {
    //     _isWaitingToAttack = true;
    //     yield return new WaitForSecondsRealtime(beforeAttackTimer);
    //     brain.SetBehaviour("Attack");
    //     _isWaitingToAttack = false;
    // }
}
