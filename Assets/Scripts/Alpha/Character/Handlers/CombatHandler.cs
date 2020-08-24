using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    /*
        This class will be used to control the "Combat" state machine within the 
        player controller.  
    */

    // References
    [Header("References")]
    public MeshRenderer shieldMeshRenderer;
    public Collider shieldSphereCollider;
    public GameObject playerWeapon;
    public AudioSource weaponSwingAudio;
    private PlayerHandler _playerHandler;
    private InputManager _inputManager;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;

    // Attributes
    private bool _canShield = false;

    // Start is called before first frame
    private void Start()
    {
        // Getting the required references
        _playerHandler = this.GetComponent<PlayerHandler>();
        _rigidbody = _playerHandler.GetRigidbody();
        _transform = _playerHandler.GetTransform();
        _animator = _playerHandler.GetAnimator();
        _inputManager = _playerHandler.GetInputManager();

        // Make sure the shield sphere is turned off by default
        shieldMeshRenderer.enabled = false;
        shieldState = ShieldState.Default;

        // Set temp timers
        _tempShieldTimer = shieldTimer;
        _tempShieldCDTimer = shieldCooldown;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateShieldFSM();
        UpdateAttack();
    }

    // Updates the shields FSM
    private void UpdateShieldFSM()
    {
        switch (shieldState)
        {
            case ShieldState.Default:
                EnableShield();
                break;
            case ShieldState.Shielding:
                Shielding();
                break;
            case ShieldState.Cooldown:
                Cooldown();
                break;
        }
    }

    #region Attacking 

    [Header("Attack Attributes")]
    [Range(0f, 3f)]
    public float animationSpeedMultiplier = 1.0f;
    public float minTimeBetweenAttack = 0.1f;
    public float maxTimeBetweenAttack = 1.0f;
    private float _attackTimer = 0.0f;
    private bool _runAttackTimer = false;
    private bool _comboStart = true;

    public enum AttackState
    {
        None,
        Attack1,
        Attack2,
        Attack3
    }
    public AttackState attackState;

    private void UpdateAttack()
    {
        _animator.speed = animationSpeedMultiplier;

        if (_comboStart)
        {
            if (_inputManager.GetAttackButtonPress() && shieldState != ShieldState.Shielding && _comboStart)
            {
                //_animator.SetBool("Attack", true);
                _comboStart = false;
                _runAttackTimer = true;
                attackState += 1;
            }
        }
        else if (_inputManager.GetAttackButtonPress() && shieldState != ShieldState.Shielding && _attackTimer >= minTimeBetweenAttack)
        {
            _runAttackTimer = true;
            attackState += 1;
        }

        if (_runAttackTimer)
        {
            _attackTimer += Time.deltaTime;
        }

        if (_attackTimer >= maxTimeBetweenAttack)
            ResetAttackTimer();

        // If an attack is within the min and max times, continue the attack state +1
        // If it goes over the max time, reset the state and timer
    }


    public float GetAttackTimer()
    {
        return _attackTimer;
    }

    public float ResetAttackTimer()
    {
        return _attackTimer = 0.0f;
    }

    public void AttackComboFinish()
    {
        if (attackState == AttackState.Attack3)
            _animator.SetBool("Attack3", false);

        _comboStart = true;
        attackState = AttackState.None;
        ResetAttackTimer();
    }

    public void Key_EnablePlayerWeaponObject()
    {
        playerWeapon.SetActive(true);
    }

    public void Key_DisableWeaponObject()
    {
        playerWeapon.SetActive(false);
    }

    public void Key_PlayWeaponSound()
    {
        weaponSwingAudio.Play();
    }

    public void Key_SetAttack1Bool()
    {
        _animator.SetBool("Attack1", false);
    }

    public void Key_SetAttack2Bool()
    {
        _animator.SetBool("Attack2", false);
    }

    public void Key_SetAttack3Bool()
    {
        _animator.SetBool("Attack3", false);
    }

    #endregion

    #region Shield
    // Attributes
    [Header("Timers", order = 0)]
    [Range(0.1f, 5f)]
    public float shieldTimer = 1.0f;
    [Range(0.1f, 5f)]
    public float shieldCooldown = 1.0f;

    // Properties
    private float _tempShieldTimer;
    private float _tempShieldCDTimer;

    public enum ShieldState
    {
        Default,    // Shield can be used & Checking for input
        Shielding,  // Player is currently shielded
        Cooldown    // Shield is currently on cooldown
    }
    [HideInInspector]
    public ShieldState shieldState;

    private void EnableShield()
    {
        // When the player hits the shield key
        if (_inputManager.GetShieldButtonPress())
        {
            shieldState = ShieldState.Shielding;
            _animator.SetBool("Shield", true);
        }
    }

    private void Shielding()
    {
        // // Slowdown the player while shielding
        // _playerHandler.GetLocomotionHandler().ActivateSlowdown();

        if (!_canShield)
            shieldState = ShieldState.Cooldown;
    }

    private void Cooldown()
    {
        shieldCooldown -= Time.deltaTime;

        // // Speed up the player after shield has expired
        // _playerHandler.GetLocomotionHandler().DeactivateSlowdown();

        if (shieldCooldown <= 0)
        {
            _animator.SetBool("Shield", false);
            shieldCooldown = _tempShieldCDTimer;
            shieldState = ShieldState.Default;
            _canShield = true;
        }
    }

    public bool SetCanShield(bool shieldState)
    {
        //Debug.Log("canShield set to " + shieldState);
        return _canShield = shieldState;
    }

    public bool GetCanShield()
    {
        return _canShield;
    }

    public float GetMaxShieldTimer()
    {
        return _tempShieldTimer;
    }

    #endregion
}
