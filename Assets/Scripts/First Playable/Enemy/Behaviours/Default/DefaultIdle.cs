﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultIdle : AIBehaviour
{

    // This state will turn into a view cone detection system, where the enemy will wander
    // around and look for the player.
    [Header("Properties")]
    public float viewConeAngle = 90.0f;
    public float totalDistance = 20.0f;
    public float minY = 10.0f;
    public float maxY = 10.0f;

    public override void OnStateEnter() { }
    public override void OnStateExit() { }
    public override void OnStateFixedUpdate() { }

    public override void OnStateUpdate()
    {
        if (brain.GetDistanceToPlayer() < totalDistance && // Is the player within distance?
        CheckValue(brain.transform.position.y, minY, maxY) && // Is the player around the same Y level?
        Vector3.Angle(brain.GetDirectionToPlayer(), brain.transform.forward) < viewConeAngle) // Is the player within the view cone?
        {
            // If the enemy's group handler isn't null, set the state to chase
            brain.GetHandler().GetEnemyGroupHandler()?.SetState(EnemyGroupHandler.E_GroupState.CHASE);

            // Lock the enemy onto the player
            brain.GetAIBehaviour("Movement").LockDestinationToPlayer(1.0f);

            // Setting the enemy into their movement state
            brain.SetBehaviour("Movement");
        }
    }

    private bool CheckValue(float value, float min, float max)
    {
        return value >= (brain.PlayerTransform.position.y - min) && value <= (brain.PlayerTransform.position.y + max);
    }
}
