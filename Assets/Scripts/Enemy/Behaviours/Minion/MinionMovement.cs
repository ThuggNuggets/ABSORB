using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMovement : AIBehaviour
{
    /*
     *  NOTES: Need to make a delay inbetween attacking and avoiding
     */
    [Header("General Movement")]
    public float acceleration = 50.0f;
    public float maxVelocity = 10.0f;
    public float turnSpeed = 0.20f;
    public float randomJumpForce = 10.0f;

    [Header("Sprinting")]
    public float sprintAcceleration = 75.0f;
    public float sprintMaxVelocity = 15.0f;
    public float rangeOfSprint = 7.5f;

    [Header("Attack Transition")]
    public float avoidPushbackForce = 5.0f;
    public float attackPushback = 2.5f;
    public float attackDistance = 2.0f;
    public float attackPushBackTimer = 0.7f;

    // RNG
    private int _attackRng = 0;
    private int _jumpRng = 0;

    public override void OnEnter() {}

    public override void OnExit() {}

    public override void OnFixedUpdate() 
    {
        // Get direction towards player
        Vector3 dir = brain.GetDirectionToPlayer();
        float dist = brain.GetDistanceToPlayer();

        // Rotate to face direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed);

        // Jump randomly left and right - This is gross atm, need to think of a better way to implement.
        if(dist > attackDistance)
        {
            _jumpRng = Random.Range(0, 10);
            if(_jumpRng >= 4)
            {
                _jumpRng = Random.Range(0, 4);
                if (_jumpRng >= 2)
                    rigidbody.AddForce((transform.forward + -transform.right).normalized * randomJumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
                else
                    rigidbody.AddForce((transform.forward + transform.right).normalized * randomJumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
            }
        }

        // Move rigidbody forwards
        else if (rigidbody.velocity.magnitude < maxVelocity && dist < rangeOfSprint)
            rigidbody.AddForce(transform.forward * acceleration * Time.fixedDeltaTime, ForceMode.Impulse); 
        
        else if (rigidbody.velocity.magnitude < sprintMaxVelocity)
            rigidbody.AddForce(transform.forward * sprintAcceleration * Time.fixedDeltaTime, ForceMode.Impulse);

        // Check if close enough to enter attack state
        if (brain.GetDistanceToPlayer() < attackDistance)
        {
            if(_attackRng > 1)
            {
                brain.SetBehaviour("Attack");
                StartCoroutine(PushBackSequence(dir));
                _attackRng = Random.Range(0, 20);
            }
            else
            {
                rigidbody.AddForce(-dir * avoidPushbackForce, ForceMode.Impulse);
                _attackRng = Random.Range(0, 20);
            }
        }
    }

    public override void OnUpdate() {}

    public IEnumerator PushBackSequence(Vector3 dir)
    {
        yield return new WaitForSecondsRealtime(attackPushBackTimer);
        rigidbody.AddForce(-dir * attackPushback, ForceMode.Impulse);
    }
}
