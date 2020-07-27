using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultDeath : AIBehaviour
{
    // Currently just destorying the game object.
    public override void OnEnter()
    {
        if(enemyHandler.GetSpawner() != null)
        {
            enemyHandler.GetSpawner().RemoveEnemy(this.gameObject);
        }

        Destroy(this.gameObject);
    }

    public override void OnExit() {}

    public override void OnFixedUpdate() {}

    public override void OnUpdate() {}
}
