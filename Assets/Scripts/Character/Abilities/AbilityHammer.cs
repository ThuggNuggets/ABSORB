using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHammer : Ability
{
    [Header("Properties")]
    public float damageToMinion = 50.0f;
    public float damageToSpecial = 20.0f;
    public float damageToElite = 100.0f;

    [Header("Timers")]
    public float turnOffTime = 2.0f;

    [Header("Sphere Cast")]
    public float radius = 10.0f;

    [Header("Debug")]
    public bool drawSphereGizmo = false;

    private bool _check = false;

    public override void OnEnter() {}
    public override void OnExit() {}
    public override void OnFixedUpdate() 
    {
        if (!_check)
        {
            StartCoroutine(TurnOffSequence());

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, Vector3.up, 0.0f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    float damage = 0.0f;

                    switch (hit.transform.gameObject.tag)
                    {
                        case "EnemyMinion":
                            damage = damageToMinion;
                            break;

                        case "EnemySpecial":
                            damage = damageToSpecial;
                            break;

                        case "EnemyElite":
                            damage = damageToElite;
                            break;
                    }
                    hit.transform.gameObject.GetComponent<AIBrain>().TakeDamage(damage, AbilityManager.E_Ability.HAMMER);
                }
            }
            _check = true;
        }
    }

    public override void OnUpdate() {}

    public override void Activate()
    {
        //colliderToToggle.enabled = true;
        //meshRendererToToggle.enabled = true;
        active = true;
    }

    public override void Deactivate()
    {
        //colliderToToggle.enabled = false;
        //meshRendererToToggle.enabled = false;
        _check = false;
        active = false;
        abilityManager.SetAbility(AbilityManager.E_Ability.NONE);
    }

    private IEnumerator TurnOffSequence()
    {
        yield return new WaitForSecondsRealtime(turnOffTime);
        Deactivate();
    }

    private void OnDrawGizmos()
    {
        if(active || drawSphereGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
