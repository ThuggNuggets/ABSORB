using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSphere : MonoBehaviour
{
    public SpecialParryBlock player;

    private void OnTriggerEnter(Collider other)
    {
        if (player.shieldState == SpecialParryBlock.ShieldState.Shielding && other.gameObject.CompareTag("EnemyWeapon"))
        {
            player.specialAttackParried = true;
            Debug.Log("Attack Parried!");
        }    
    }
}
