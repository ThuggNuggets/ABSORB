﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupChase : GroupState
{
    [Header("Combat Transition Properties")]
    public float CoMDistanceFromPlayer = 10.0f;

    public override void OnStateEnter() {}

    public override void OnStateUpdate() 
    {
        // Move all enemies towards the player
        this.enemyGroupHandler.SetTargetDestination(enemyGroupHandler.playerTransform.position);
        this.enemyGroupHandler.UpdateAllFlockDestinations();

        // Check the distance between the group and the player; entering the combat state if close enough
        if(Vector3.Distance(this.enemyGroupHandler.GetCenterOfMass(), this.enemyGroupHandler.playerTransform.position) < CoMDistanceFromPlayer)
        {
            this.enemyGroupHandler.SetState(EnemyGroupHandler.E_GroupState.COMBAT);
            Debug.Log("Group entered attack state!");
        }
    }

    public override void OnStateFixedUpdate() {}

    public override void OnStateExit() {}
}
