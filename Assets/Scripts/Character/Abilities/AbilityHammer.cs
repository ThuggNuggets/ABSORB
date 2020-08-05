using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHammer : Ability
{
    [Header("References")]
    public GameObject hammerGameObject;
    public Transform groundSmashTransform;
    public Transform groundSmashParent;
    public ParticleSystem groundSmashParticleSystem;
    public AudioSource groundSmashAudio;

    [Header("Properties")]
    public float groundSmashReparentTimer = 2.0f;
    private Vector3 _groundSmashImpactLocation = Vector3.zero;

    [Header("Damage Properties")]
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
        _groundSmashImpactLocation = groundSmashTransform.position;
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
                    hit.transform.gameObject.GetComponent<EnemyHandler>().TakeDamage(damage, AbilityManager.E_Ability.HAMMER);
                }
            }
            _hasRan = true;
        }
    }

    public override void OnUpdate() {}

    public override void Activate()
    {
        active = true;
        animator.SetBool("HammerAttack", true);
    }

    public override void Deactivate()
    {
        _hasRan = true;
        hammerGameObject.SetActive(false);
        animator.SetBool("HammerAttack", false);
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
        groundSmashTransform.SetParent(null);
        Vector3 fixY = groundSmashTransform.position;
        fixY.y = 0.997f;
        groundSmashTransform.position = fixY;
        groundSmashParticleSystem.Play();
        groundSmashAudio.Play();
        active = false;
        abilityManager.SetAbility(AbilityManager.E_Ability.NONE);
        StartCoroutine(ReparentGroundSmash());
    }

    private void OnDrawGizmos()
    {
        if(!_hasRan || drawSphereGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    private IEnumerator ReparentGroundSmash()
    {
        yield return new WaitForSecondsRealtime(groundSmashReparentTimer);
        groundSmashTransform.SetParent(groundSmashParent);
        groundSmashTransform.transform.localPosition = _groundSmashImpactLocation;
        groundSmashTransform.transform.localRotation = Quaternion.identity;
    }
}
