using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupCombat : GroupState
{
    [Header("Properties")]
    public float beginHuddleDistance = 20.0f;
    public float huddleDistance = 10.0f;

    List<EnemyHandler> _slots = new List<EnemyHandler>();

    public override void OnStateEnter() { }

    public override void OnStateUpdate()
    {
        if (this.enemyGroupHandler.GetDistanceFromPlayer() <= beginHuddleDistance)
        {
            // Making each enemy find a position around the player, if not locked on
            for (int i = 0; i < this.enemyGroupHandler.GetEnemies().Count; ++i)
            {
                AIBrain aiBrain = enemyGroupHandler.GetEnemy(i).GetBrain();
                if (!aiBrain.GetAIBehaviour("Movement").IsLockedOntoPlayer())
                {
                    // Fix the tilt back
                    Vector3 posFix = aiBrain.PlayerTransform.position;
                    posFix.y = aiBrain.transform.position.y;

                    // Move enemy into position and make them face player
                    aiBrain.transform.forward = (posFix - aiBrain.transform.position).normalized; ;
                    aiBrain.GetAIBehaviour("Movement").OverrideDestination(GetPositionAroundPlayer(i), 1.0f);
                }
            }
        }
        else
        {
            // Moving towards the player via flocking calculations
            this.enemyGroupHandler.SetTargetDestination(this.enemyGroupHandler.playerTransform.position);
            this.enemyGroupHandler.UpdateAllFlockDestinations();
        }
    }

    public override void OnStateFixedUpdate() { }

    public override void OnStateExit() { }

    public Vector3 GetPositionAroundPlayer(int index)
    {
        float degreesPerIndex = 360f / this.enemyGroupHandler.GetEnemies().Count;
        var pos = this.enemyGroupHandler.playerTransform.position;
        var offset = new Vector3(0f, 0f, huddleDistance);
        return pos + (Quaternion.Euler(new Vector3(0f, degreesPerIndex * index, 0f)) * offset);
    }
}
