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
        /*
         * 
         * This is fairly jank, need to implement some "steps" to take rather than just checking distance.
         * 
         * Also the attack / avoid RNG is stupid. Should probably just check if the player is within a "danger range" and make the minion
         * flee from that distance.
         * 
         * 
         * TODO:
         *  Sprint from a distance - Have the minion move faster towards the player when at a distance.
         *  Move slower towards player when out of sprint range.
         *  Enter attack phase when within a attack range.
         *  
         *  Optional:
         *      Have the minion move more randomly than just in a straight line.
         *      
         *      
         */

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

        //// Jump randomly left and right - This is gross atm, need t-o think of a better way to implement.
        //if(dist > attackDistance)
        //{
        //    _jumpRng = Random.Range(0, 10);
        //    if(_jumpRng >= 4)
        //    {
        //        _jumpRng = Random.Range(0, 4);
        //        if (_jumpRng >= 2)
        //            rigidbody.AddForce((transform.forward + -transform.right).normalized * randomJumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        //        else
        //            rigidbody.AddForce((transform.forward + transform.right).normalized * randomJumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        //    }
        //}

        //// Move rigidbody forwards
        //else if (rigidbody.velocity.magnitude < maxVelocity && dist < rangeOfSprint)
        //    rigidbody.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse); 

        //else if (rigidbody.velocity.magnitude < sprintMaxVelocity)
        //    rigidbody.AddForce(transform.forward * sprintAcceleration * Time.fixedDeltaTime, ForceMode.Impulse);

        //// Check if close enough to enter attack state
        //if (brain.GetDistanceToPlayer() < attackDistance)
        //{
        //    if(_attackRng > 1)
        //    {
        //        brain.SetBehaviour("Attack");
        //        StartCoroutine(PushBackSequence(dir));
        //        _attackRng = Random.Range(0, 20);
        //    }
        //    else
        //    {
        //        rigidbody.AddForce(-dir * avoidPushbackForce, ForceMode.Impulse);
        //        _attackRng = Random.Range(0, 20);
        //    }
        //}
    }

    public override void OnUpdate() {}

    //public IEnumerator PushBackSequence(Vector3 dir)
    //{
    //    yield return new WaitForSecondsRealtime(attackPushBackTimer);
    //    rigidbody.AddForce(-dir * attackPushback, ForceMode.Impulse);
    //}
}
