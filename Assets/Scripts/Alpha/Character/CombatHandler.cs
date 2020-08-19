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
    private PlayerHandler _playerHandler;
    private InputManager _inputManager;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;

    // Attributes
    private bool _canShield = true;

    // Start is called before first frame
    private void Start()
    {
        // Getting the required references
        _playerHandler  = this.GetComponent<PlayerHandler>();
        _rigidbody = _playerHandler.GetRigidbody();
        _transform = _playerHandler.GetTransform();
        _animator = _playerHandler.GetAnimator();
        _inputManager   = _playerHandler.GetInputManager();

        // Make sure the blocking sphere is turned off by default
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
        // Slowdown the player while shielding
        //_playerMovement.Key_ActivateSlowdown();

        if (!_canShield)
            shieldState = ShieldState.Cooldown;
    }

    private void Cooldown()
    {
        shieldCooldown -= Time.deltaTime;

        // Speed up the player after shield has expired
        //_playerMovement.Key_DeactivateSlowdown();

        if (shieldCooldown <= 0)
        {
            shieldCooldown = _tempShieldCDTimer;
            shieldState = ShieldState.Default;
            _canShield = true;
        }
    }

    public bool SetCanShield(bool shieldState)
    {
        Debug.Log("canShield set to " + shieldState);
        return _canShield = shieldState;
    }

    public bool GetCanShield()
    {
        return _canShield;
    }

    #endregion
}
