using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public int maxHealth = 10;
    public MeshRenderer sphereRenderer;
    public Collider sphereCollider;

    private Animator _animator;
    private PlayerMovement _playerMovement;
    private bool isAlive = false;
    private int currentHealth = 10;
    private bool canShield = true;

    #region Shield

    [Header("Timers", order = 0)]
    [Range(0.1f, 5f)]
    public float shieldTimer = 1.0f;
    [Range(0.1f, 5f)]
    public float shieldCooldown = 1.0f;

    private InputManager _inputManager;
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

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
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

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
