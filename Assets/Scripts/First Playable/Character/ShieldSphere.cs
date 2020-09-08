using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSphere : MonoBehaviour
{
    public CombatHandler player;
    
    private void OnTriggerEnter(Collider other)
    {
       if (player.shieldState == CombatHandler.ShieldState.Shielding && other.gameObject.CompareTag("EnemyWeapon"))
       {
           EnemyHandler enemy = other.GetComponentInParent<EnemyHandler>();
           switch(enemy.GetEnemyType())
           {
               case EnemyHandler.EnemyType.SPECIAL:
                    enemy.GetBrain().SetBehaviour("Parried");
               break;
           }
       }    
    }
}
