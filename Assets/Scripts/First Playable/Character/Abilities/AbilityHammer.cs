using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHammer : Ability
{
    [Header("References")]
    public GameObject hammerGameObject;
    public Transform groundSmashTransform;

    [Header("Properties")]
    public float groundSmashReparentTimer = 2.0f;
    public float damageToMinion = 50.0f;
    public float damageToSpecial = 20.0f;
    public float damageToElite = 100.0f;
    public float areaOfEffect = 10.0f;

    private Vector3 _groundSmashImpactLocation = Vector3.zero;
    private Animator _animator;
    private PlayerHandler _playerHandler;
    private Transform _groundSmashParent;
    private AudioSource _groundSmashAudio;
    private ParticleSystem _groundSmashParticleSystem;

    private void Awake()
    {
        _playerHandler = this.GetComponent<PlayerHandler>();
        _animator = _playerHandler.GetAnimator();
        _groundSmashParent = hammerGameObject.transform.parent;
        _groundSmashImpactLocation = groundSmashTransform.position;
        _groundSmashAudio = groundSmashTransform.GetComponent<AudioSource>();
        _groundSmashParticleSystem = groundSmashTransform.GetComponent<ParticleSystem>();
    }

    public override void OnEnter() 
    {
        hammerGameObject.SetActive(true);
    }

    public override void OnExit() {}

    public override void Activate()
    {
        active = true;
        _animator.SetBool("HammerAttack", true);
    }

    // Key Event: Deactivates the hammer ability; only to be called through animation key event
    public void Key_Deactivate()
    {
        hammerGameObject.SetActive(false);
        _animator.SetBool("HammerAttack", false);
        abilityHandler.SetAbility(AbilityHandler.AbilityType.NONE);
    }

    // Key Event: Activates the ground smash VFX; only to be called through animation key event
    public void Key_ActivateHammerGroundSmash()
    {
        // Unparent and play VFX
        groundSmashTransform.SetParent(null);
        _groundSmashParticleSystem.Play();
        _groundSmashAudio.Play();
        StartCoroutine(ReparentGroundSmash());
        
        // Check for any hits
        CheckForEnemyHit();
        active = false;
    }

    private IEnumerator ReparentGroundSmash()
    {
        yield return new WaitForSecondsRealtime(groundSmashReparentTimer);
        groundSmashTransform.SetParent(_groundSmashParent);
        groundSmashTransform.transform.localPosition = _groundSmashImpactLocation;
        groundSmashTransform.transform.localRotation = Quaternion.identity;
    }

    private void CheckForEnemyHit()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, areaOfEffect, Vector3.up, 0.0f);
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
                hit.transform.gameObject.GetComponent<EnemyHandler>().TakeDamage(damage, AbilityHandler.AbilityType.HAMMER);
            }
        }
    }
}
