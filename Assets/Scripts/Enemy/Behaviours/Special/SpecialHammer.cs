﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialHammer : AIBehaviour
{
    [Header("References")]
    public Animator hammerAnimator;
    public string swingAnimation = "Attacking";

    [Header("Properties")]
    public float lungeAcceleration = 100.0f;
    public float lungeMaxVelocity = 20.0f;
    public float turnSpeed = 0.20f;
    public float lungeDistancePadding = 2.0f;
    public float startAnimationDistance = 10.0f;
    public float cancelAndRetreatDistance = 7.0f;

    private bool _isAttacking = false;
    private float _onEnterDistance = 0.0f;
    private Vector3 _onEnterDirection = Vector3.zero;
    private Vector3 _targetPosition = Vector3.zero;


    // Fix for accuracy notes:
    /*
     * Instead of having an "animation sequence", we need to actually make the enemy put 
     * some thought into attacking the player. First thought that comes to mind would be;

     * Have the enemy know the exact distance to lunge to get within attack range of the player.
     * Double check to make sure the player is still in attack range before attacking.
     *  - (Will need to exit the attacking function when this happens, to prevent from misfiring)
     */

    public override void OnEnter() 
    {
        _onEnterDistance = brain.GetDistanceToPlayer();
        _onEnterDirection = brain.GetDirectionToPlayer();
        _targetPosition = brain.playerTransform.position;
    }

    public override void OnExit() 
    {
        _onEnterDistance = 0.0f;
        _onEnterDirection = Vector3.zero;
        _targetPosition = Vector3.zero;
        _isAttacking = false;
    }

    public override void OnFixedUpdate()
    {
        // Gets distance from player realtime
        float distFromTarget = GetDistanceFromTarget();
        float distFromPlayer = brain.GetDistanceToPlayer();

        // Rotate to face player
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(brain.GetDirectionToPlayer()), turnSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(finalDirection), turnSpeed);

        // Check if the enemy isn't already attacking
        if (!_isAttacking)
        {
            // Check if the player is within attacking distance, move away if they are.
            if (distFromPlayer < cancelAndRetreatDistance)
            {
                brain.SetBehaviour("Movement");
                return;
            }

            // Move towards player fast, if within attack range
            if (rigidbody.velocity.magnitude < lungeMaxVelocity && distFromTarget > lungeDistancePadding)
            {
                rigidbody.AddForce(brain.GetDirectionToPlayer() * lungeAcceleration * Time.fixedDeltaTime, ForceMode.Impulse);

                if (distFromTarget <= startAnimationDistance)
                    hammerAnimator.SetBool(swingAnimation, true);
            }

            // Start attacking when at max velocity and close enough
            else if (distFromTarget <= lungeDistancePadding)
            {
                _isAttacking = true;
            }
        }
        else
        {
            // Setting back to movement
            hammerAnimator.SetBool(swingAnimation, false);
            brain.SetBehaviour("Movement");
        }
    }

    private float GetDistanceFromTarget()
    {
        return Vector3.Distance(_targetPosition, transform.position);
    }

    public override void OnUpdate() {}

    public void SetAttackingTrue()
    {
        _isAttacking = true;
    }

    public void SetAttackingFalse()
    {
        hammerAnimator.SetBool(swingAnimation, false);
        _isAttacking = false;
    }
}
