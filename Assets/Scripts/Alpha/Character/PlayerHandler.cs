using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public int maxHealth = 10;
    public MeshRenderer sphereRenderer;
    public Collider sphereCollider;

    private Animator _animator;
    private bool isAlive = false;
    private int currentHealth = 10;
    private bool canShield = false;

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


        //if (Input.GetKeyDown(KeyCode.Space))
        //    shieldState = ShieldState.Shielding;
    }

    private void Shielding()
    {
        sphereRenderer.enabled = true;
        sphereCollider.enabled = true;
        shieldTimer -= Time.deltaTime;

        // Slow down the player when shielding
        //_playerSlowdown.SetSlowdown();

        if (shieldTimer <= 0)
        {
            _animator.SetBool("Shield", false);
            sphereRenderer.enabled = false;
            sphereCollider.enabled = false;
            shieldTimer = _tempShieldTimer;
            shieldState = ShieldState.Cooldown;
        }
    }

    private void Cooldown()
    {
        shieldCooldown -= Time.deltaTime;

        // Speed up the player after shield has expired
        //_playerSlowdown.SetSpeedUp();

        if (shieldCooldown <= 0)
        {
            shieldCooldown = _tempShieldCDTimer;
            //playerSlowdown.slowState = PlayerSlowdown.SlowState.Default; failsafe
            shieldState = ShieldState.Default;
        }
    }

    public bool SetCanShield(bool shieldState)
    {
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

        // Make sure the blocking sphere is turned off by default
        sphereRenderer.enabled = false;
        shieldState = ShieldState.Default;
        _inputManager = FindObjectOfType<InputManager>();
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
