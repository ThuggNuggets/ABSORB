using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAbsorbed : AIBehaviour
{
    [Header("Graphic References")]
    public Material bodyMaterial;
    public Renderer bodyRenderer;
    public Material weaponMaterial;
    public Renderer weaponRenderer;

    [Header("VFX Refereneces")]
    public GameObject absorbGameObject;
    public ParticleSystem absorbParticleEffect;
    private AbilityManager _playerAbilityManager;
    private SpecialParried _specialParried;
    private Animator _animator;

    [Header("Properties")]
    public float cutoffMax = 1.2f;
    public float cutOutSpeed = 1.0f;
    public float cutOffSpeed = 1.5f;
    public float destoryAbsorbEffectAfter = 4.0f;

    private bool _enabled = false;
    private float _cutOutTimer = 0.0f;
    private float _cutOffTimer = 0.0f;

    private void Awake()
    {
        // Getting the other behaviour
        _specialParried = this.GetComponent<SpecialParried>();

        // Getting the animator
        _animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        // Creating and assigning a new material
        bodyRenderer.material = Instantiate(bodyMaterial);

        // Creating and assigning a new material
        weaponRenderer.material = Instantiate(weaponMaterial);

        // Getting the ability manager from the player
        _playerAbilityManager = brain.playerTransform.GetComponent<AbilityManager>();
    }

    public override void OnEnter()
    {
        _animator.enabled = false;
        _cutOutTimer = cutoffMax;
        _enabled = true;
        //_playerAbilityManager.playerForceField.SetActive(true);


        absorbParticleEffect.Play();
        absorbGameObject.SetActive(true);
        absorbGameObject.transform.SetParent(null);
    }

    public override void OnExit() {}

    public override void OnFixedUpdate() {}

    public override void OnUpdate() 
    {
        if (_enabled)
        {
            if (_cutOutTimer > 0.7f)
            {
                _cutOutTimer -= Time.deltaTime * cutOutSpeed;
                _cutOffTimer -= Time.deltaTime * cutOffSpeed;
                bodyRenderer.material.SetFloat("_Cutout", _cutOutTimer);
                weaponRenderer.material.SetFloat("_Cutoff", _cutOffTimer);
            }
            else
            {
                absorbParticleEffect.Stop();
               // _playerAbilityManager.playerForceField.SetActive(false);
                _playerAbilityManager.SetAbility(AbilityManager.E_Ability.HAMMER);
                enemyHandler.Kill();
                Destroy(absorbGameObject, destoryAbsorbEffectAfter);
            }
        }
    }
}
