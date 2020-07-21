using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Max health
    // Heal from absorb

    [Range(0, 100)]
    public float currentHealth = 100.0f;
    [Range(0, 100)]
    public float healthFromAbsorb = 30.0f;

    //public float maxHealth = 100.0f;
    private float damageTaken = 0.0f;
    private SpecialParryBlock player;
    private GameObject collidedObject;

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

    void Update()
    {
        Function();
    }

    private void OnCollisionEnter(Collision collision)
    {
        collidedObject = collision.collider.gameObject;

        if (player.shieldState != SpecialParryBlock.ShieldState.Shielding &&
            collidedObject.layer == LayerMask.NameToLayer("EnemyWeapon"))
        {
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
        }
        else if (player.shieldState != SpecialParryBlock.ShieldState.Shielding &&
            collidedObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
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
        }
    }

    private void Function()
    {
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

    private float TakeDamage(float damageAmount)
    {
        return currentHealth -= damageAmount;
    }

    private void MinionDamage()
    {
        //float damage = collidedObject.GetComponent<AIBrain>().GetDamage();        
        //TakeDamage(damage);
        enemy = EnemyType.None;
        //Debug.Log("Damage taken: " + damage);
    }

    private void SpecialDamage()
    {
        //float damage = collidedObject.GetComponent<AIBrain>().GetDamage();
        //TakeDamage(damage);
        enemy = EnemyType.None;
        //Debug.Log("Damage taken: " + damage);
    }

    private void EliteDamage()
    {
        //float damage = collidedObject.GetComponent<AIBrain>().GetDamage();
        //TakeDamage(damage);
        enemy = EnemyType.None;
        //Debug.Log("Damage taken: " + damage);
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