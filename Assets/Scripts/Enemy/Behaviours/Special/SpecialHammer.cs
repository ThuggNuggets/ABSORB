using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialHammer : AIBehaviour
{
    [Header("References")]
    public Animator hammerAnimator;

    [Header("Properties")]
    public float lungeForce = 100.0f;
    public float turnSpeed = 0.20f;

    [Header("Times")]
    public float animationTriggerTime = 0.3f;
    public float returnToChaseTime = 0.4f;
    public float retreatTime = 0.4f;

    public override void OnEnter()
    {
        rigidbody.AddForce(brain.GetDirectionToPlayer() * lungeForce, ForceMode.Impulse);
        StartCoroutine(HammerAnimationSequence());
    }

    public IEnumerator HammerAnimationSequence()
    {
        yield return new WaitForSecondsRealtime(animationTriggerTime);
        hammerAnimator.SetTrigger("Swing");
        yield return new WaitForSecondsRealtime(retreatTime);
        rigidbody.AddForce(-brain.GetDirectionToPlayer() * lungeForce, ForceMode.Impulse);
        yield return new WaitForSecondsRealtime(returnToChaseTime);
        brain.SetBehaviour("Movement");
    }

    public override void OnExit() {}

    public override void OnFixedUpdate() {}

    public override void OnUpdate() 
    {
        // Rotate to face direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(brain.GetDirectionToPlayer()), turnSpeed);
    }

}
