using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Change currentHealth to maxHealth in future builds
    [Range(0, 100)]
    public float currentHealth = 100.0f;
    [Range(0, 100)]
    public float healthFromAbsorb = 30.0f;

    //public float maxHealth = 100.0f;
    private SpecialParryBlock player;
    private GameObject collidedObject = null;

    enum EnemyType
    {
        None,
        Minion,
        Special,
        Elite
    }
    EnemyType enemy;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<SpecialParryBlock>();
        //currentHealth = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Change this in the future, uses too much processing power
        GameObject temp = collision.collider.gameObject;

        // Check the layer of the object we collided with
        if (player.shieldState != SpecialParryBlock.ShieldState.Shielding &&
            temp.layer == LayerMask.NameToLayer("EnemyWeapon"))
        {
            // Only assign collidedObject if we collided with the correct layer
            collidedObject = collision.collider.gameObject;
            // Check what tag was assigned to it
            switch (collidedObject.tag)
            {
                case "EnemyMinion":
                    enemy = EnemyType.Minion;
                    break;
                case "EnemySpecial":
                    enemy = EnemyType.Special;
                    break;
                default:
                    break;
            }
            // Run the function cause we expected to hit a enemy attack
            Function(); 
        }
        else if (player.shieldState != SpecialParryBlock.ShieldState.Shielding &&
            temp.layer == LayerMask.NameToLayer("EnemyProjectile") ||
            temp.layer == LayerMask.NameToLayer("EnemyProjectile") && temp.CompareTag("EnemyElite"))
        {
            // Only assign collidedObject if we collided with the correct layer
            collidedObject = collision.collider.gameObject;
            switch (collidedObject.tag)
            {
                case "EnemySpecial":
                    enemy = EnemyType.Special;
                    break;
                case "EnemyElite":
                    enemy = EnemyType.Elite;
                    break;
                default:
                    break;
            }
            // Run the function cause we expected to hit a enemy attack
            Function();
        }
    }

    private void Function()
    {
        if (!collidedObject)
            return;

        switch (enemy)
        {
            case EnemyType.None:
                break;
            case EnemyType.Minion:
                MinionDamage();
                break;
            case EnemyType.Special:
                SpecialDamage();
                break;
            case EnemyType.Elite:
                EliteDamage();
                break;
        }
        
    }

    // Return the amount of damage the player should take
    private float TakeDamage(float damageAmount)
    {
        collidedObject = null;
        return currentHealth -= damageAmount;
    }

    private void MinionDamage()
    {
        float damage = collidedObject.GetComponent<AIBrain>().GetDamage();
        TakeDamage(damage);
        enemy = EnemyType.None;
        Debug.Log("Damage taken: " + damage);
    }

    private void SpecialDamage()
    {
        float damage = collidedObject.GetComponent<AIBrain>().GetDamage();
        TakeDamage(damage);
        enemy = EnemyType.None;
        Debug.Log("Damage taken: " + damage);
    }

    private void EliteDamage()
    {
        float damage = collidedObject.GetComponent<EliteProjectile>().GetDamage();
        TakeDamage(damage);
        enemy = EnemyType.None;
        Debug.Log("Damage taken: " + damage);
    }
}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (player.shieldState != SpecialParryBlock.ShieldState.Shielding && other.gameObject.CompareTag("EnemyWeapon"))
    //    {
    //        TakeDamage(other.GetComponent<DamageTest>().enemyDamage);
    //        Debug.Log("Attack damage taken!");
    //    }
    //}