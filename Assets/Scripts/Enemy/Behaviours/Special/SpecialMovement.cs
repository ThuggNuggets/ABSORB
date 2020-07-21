using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMovement : AIBehaviour
{
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
    [Header("General Movement")]
    public float acceleration = 25.0f;
    public float maxVelocity = 10.0f;
    public float turnSpeed = 0.20f;

    [Header("Attack")]
    public float attackDistance = 10.0f;
    public float beforeAttackTimer = 0.7f;
    private bool _isWaitingToAttack = false;
    
    public override void OnEnter() {}

    public override void OnExit() 
    {
        _isWaitingToAttack = false;
    }

    public override void OnFixedUpdate() 
    {
        // If the enemy is already waiting to attack, then exit this out of this function
        if (_isWaitingToAttack)
            return;

        // Get direction and distance from player
        Vector3 dir = brain.GetDirectionToPlayer();
        float dist  = brain.GetDistanceToPlayer();

        // Rotate to face direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed);

        // Only adding force if velocity is under max velocity
        if (rigidbody.velocity.magnitude < maxVelocity)
        {
            // Moving towards player if out of attack distance
            if (dist > attackDistance)
                rigidbody.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse);
            else
                rigidbody.AddForce(-transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse);
        }

        // If the enemy is at the optimal attack range, enter the attack state
        if (Mathf.Ceil(dist) == attackDistance)
            StartCoroutine(BeforeAttackTimer());
    }

    public override void OnUpdate() { }
    
    public IEnumerator BeforeAttackTimer()
    {
        _isWaitingToAttack = true;
        yield return new WaitForSecondsRealtime(beforeAttackTimer);
        brain.SetBehaviour("Attack");
        _isWaitingToAttack = false;
    }
}
