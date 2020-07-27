using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialParryBlock : MonoBehaviour
{
    public MeshRenderer sphereRenderer;
    public Collider sphereCollider;

    [Header("Timers", order = 0)]
    [Range(0.1f, 5f)]
    public float shieldTimer = 1.0f;
    [Range(0.1f, 5f)]
    public float shieldCooldown = 1.0f;

    private PlayerSlowdown playerSlowdown;
    private float tempShieldTimer;
    private float tempShieldCDTimer;

    [HideInInspector]
    public bool specialAttackParried = false;

    public enum ShieldState
    {
        Default,    // Shield can be used & Checking for input
        Shielding,  // Player is currently shielded
        Cooldown    // Shield is currently on cooldown
    }
    [HideInInspector]
    public ShieldState shieldState;

    // Start is called before the first frame update
    void Start()
    {
        playerSlowdown = GetComponent<PlayerSlowdown>();
        // Make sure the blocking sphere is turned off by default
        sphereRenderer.enabled = false;
        shieldState = ShieldState.Default;

        // Set temp timers
        tempShieldTimer = shieldTimer;
        tempShieldCDTimer = shieldCooldown;
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

    private void EnableShield()
    {
        // When the player hits the shield key
        if (Input.GetKeyDown(KeyCode.Space))
            shieldState = ShieldState.Shielding;
    }

    private void Shielding()
    {
        sphereRenderer.enabled = true;
        sphereCollider.enabled = true;
        shieldTimer -= Time.deltaTime;

        // Slow down the player when shielding
        playerSlowdown.SetSlowdown();
        playerSlowdown.slowState = PlayerSlowdown.SlowState.Slowdown;

        if (shieldTimer <= 0)
        {
            sphereRenderer.enabled = false;
            sphereCollider.enabled = false;
            shieldTimer = tempShieldTimer;
            shieldState = ShieldState.Cooldown;
        }
    }

    private void Cooldown()
    {
        shieldCooldown -= Time.deltaTime;

        // Speed up the player after shield has expired
        playerSlowdown.SetSpeedUp();
        playerSlowdown.slowState = PlayerSlowdown.SlowState.SpeedUp;

        if (shieldCooldown <= 0)
        {
            shieldCooldown = tempShieldCDTimer;
            playerSlowdown.slowState = PlayerSlowdown.SlowState.Default;
            shieldState = ShieldState.Default;
        }
    }
}