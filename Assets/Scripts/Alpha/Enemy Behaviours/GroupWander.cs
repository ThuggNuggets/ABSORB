using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupWander : GroupState
{
    [Header("References")]
    public Transform wanderAroundThisTransform;

    [Header("Properties")]
    public float updatePathTime = 1.0f;
    private bool _isWaiting = false;

    public override void OnStateEnter() {}
    public override void OnStateUpdate() 
    {     
        // Check if the enemy is waiting to update a path, update if not
        if(!_isWaiting)
            StartCoroutine(WaitBeforeUpdatingPath());
    }
    public override void OnStateFixedUpdate() {}
    public override void OnStateExit() {}

    // Move all enemies towards the destination after cooldown time
    private IEnumerator WaitBeforeUpdatingPath()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(updatePathTime);
        this.enemyGroupHandler.SetTargetDestination(wanderAroundThisTransform.position);
        this.enemyGroupHandler.UpdateAllFlockDestinations();   
        _isWaiting = false;
    }
}
