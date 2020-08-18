using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public enum PlayerAnimatorState
    {
        IDLE,
        ATTACK,
        SHIELD,
        DASH,
        ABSORB,
        DEATH,
    }
    private PlayerAnimatorState _currentState;


    [Header("References")]
    public MeshRenderer sphereRenderer;
    public Collider sphereCollider;

    [Header("Properties")]
    public int maxHealth = 10;

    // Attributes
    private int currentHealth = 10;

    // References
    private Animator _animator;
    private InputManager _inputManager;
    private PlayerMovement _playerMovement;

    // Flags
    private bool isAlive = false;
    private bool canShield = true;

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        _inputManager = FindObjectOfType<InputManager>();

        // Make sure the blocking sphere is turned off by default
        sphereRenderer.enabled = false;
        shieldState = ShieldState.Default;
        // Set temp timers
        _tempShieldTimer = shieldTimer;
        _tempShieldCDTimer = shieldCooldown;

    }

    // Update is called once per frame
    void Update()
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
        _playerMovement.Key_ActivateSlowdown();

        if (!canShield)
            shieldState = ShieldState.Cooldown;
    }

    private void Cooldown()
    {
        shieldCooldown -= Time.deltaTime;

        // Speed up the player after shield has expired
        _playerMovement.Key_DeactivateSlowdown();

        if (shieldCooldown <= 0)
        {
            shieldCooldown = _tempShieldCDTimer;
            shieldState = ShieldState.Default;
            canShield = true;
        }
    }

    public bool SetCanShield(bool shieldState)
    {
        Debug.Log("canShield set to " + shieldState);
        return canShield = shieldState;
    }

    public bool GetCanShield()
    {
        return canShield;
    }

    #endregion

    #region Getters and setters
    public bool GetIsAlive()
    {
        return isAlive;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetState(PlayerAnimatorState state)
    {
        _currentState = state;
    }

    public PlayerAnimatorState GetState()
    {
        return _currentState;
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public InputManager GetInputManager()
    {
        return _inputManager;
    }

    #endregion
}