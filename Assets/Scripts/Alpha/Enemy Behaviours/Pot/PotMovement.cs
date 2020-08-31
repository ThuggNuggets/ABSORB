using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotMovement : AIBehaviour
{
    [Header("Properties")]
    public float destinationPadding = 1.0f;

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

        // Currently setting the on enter destination to the player; in the future we'll have to set the destination from a "EnemyAI Controller"
        this.LockDestinationToPlayer(destinationPadding);
    }

    public override void OnStateEnter() {}

    public override void OnStateUpdate()
    {
        // Checking if we should be locked onto the player or not...
        if (this.destinationLockedToPlayer)
            this.currentDestination = brain.PlayerTransform.position;

        // Updating the target destination every frame
        brain.UpdateTargetDestination(this.currentDestination, destinationPadding);

        // If player is within attack range;
        if(brain.GetNavMeshAgent().remainingDistance <= _attackRange)
        {
            // Enemy will enter attack phase if locked onto player:
            if(this.destinationLockedToPlayer)
            {
                brain.SetBehaviour("Attack");
                return;
            }
            else
            {
                // Here is what they'll do when they aren't locked on
                // so general movement, stuff will go here when
                // the group system has been worked out
            }
            
        }
    }

    public override void OnStateFixedUpdate() {}

    public override void OnStateExit(){}
}
