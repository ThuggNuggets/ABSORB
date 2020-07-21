using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteAttack : AIBehaviour
{
    [Header("Properties")]
    public float turnSpeed = 0.20f;
    public float justFiredProjectileTimer = 1.0f;
    public float projectileSpeed = 150.0f;
    public float projectileLifeTime = 4.0f;
    public float dashCancelRange = 10.0f;
    public GameObject projectilePrefab;
    public Transform projectileStartPoint;

    public override void OnEnter() 
    {
        EliteProjectile eliteProjectile = Instantiate(projectilePrefab, null).GetComponent<EliteProjectile>();
        eliteProjectile.InitialiseProjectile(transform, brain.playerTransform.position, projectileStartPoint, projectileSpeed, projectileLifeTime);
        StartCoroutine(JustFiredTimer());
    }

    public override void OnExit() {}

    public override void OnFixedUpdate() {}

    public override void OnUpdate() 
    {
        // If player gets too close when preparing to fire, the enemy will cancel the attack.
        if (brain.GetDistanceToPlayer() < dashCancelRange)
        {
            brain.SetBehaviour("Movement");
            return;
        }

        // Rotate to face direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(brain.GetDirectionToPlayer()), turnSpeed);
    }

    public IEnumerator JustFiredTimer()
    {
        yield return new WaitForSecondsRealtime(justFiredProjectileTimer);
        brain.SetBehaviour("Movement");
    }
}
