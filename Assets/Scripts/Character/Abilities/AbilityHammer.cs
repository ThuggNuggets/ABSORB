using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHammer : Ability
{
    [Header("References")]
    public GameObject hammerGameObject;
    public ParticleSystem groundSmashParticleSystem;
    public AudioSource groundSmashAudio;

    [Header("Properties")]
    public float damageToMinion = 50.0f;
    public float damageToSpecial = 20.0f;
    public float damageToElite = 100.0f;

    [Header("Sphere Cast")]
    public float radius = 10.0f;

    [Header("Debug")]
    public bool drawSphereGizmo = false;

    private bool _hasRan = true;
    private Animator animator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    public override void OnEnter() 
    {
        hammerGameObject.SetActive(true);
    }

    public override void OnExit() {}

    public override void OnFixedUpdate() 
    {
        if (!_hasRan)
        {
            //StartCoroutine(TurnOffSequence());

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
            _hasRan = true;
        }
    }

    public override void OnUpdate() {}

    public override void Activate()
    {
        animator.SetBool("HammerAttack", true);
        active = true;
    }

    public override void Deactivate()
    {
        animator.SetBool("HammerAttack", false);
        active = false;
        _hasRan = true;
        abilityManager.SetAbility(AbilityManager.E_Ability.NONE);
    }

    public void DeactivateHammerGameobject()
    {
        hammerGameObject.SetActive(false);
    }

    public void ActivateHammerCheck()
    {
        _hasRan = false;
        ActivateHammerGroundSmash();
    }

    public void ActivateHammerGroundSmash()
    {
        groundSmashParticleSystem.Play();
        groundSmashAudio.Play();
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
