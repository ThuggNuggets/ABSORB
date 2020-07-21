using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialParryBlock : MonoBehaviour
{
    public MeshRenderer sphereRenderer;

    [Header("Timers", order = 0)]
    [Range(0.1f, 5f)]
    public float shieldTimer = 1.0f;
    [Range(0.1f, 5f)]
    public float shieldCooldown = 1.0f;

    [Header("Acceleration Properties", order = 1)]
    public float maxSlowAcceleration = 10.0f;
    public float timeToAcceleration = 0.2f; // Approx time for the player acceleration to reach the slow/default amount

    float speedSmoothVelocity;
    float speedSmoothVelocityN;

    private Movement playerMovement;
    private float tempPlayerAcceleration;
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
        playerMovement = GetComponent<Movement>();
        speedSmoothVelocity = playerMovement.maxVelocity;
        speedSmoothVelocityN = -playerMovement.maxVelocity;
        // Make sure the blocking sphere is turned off by default
        sphereRenderer.enabled = false;
        shieldState = ShieldState.Default;

        tempPlayerAcceleration = playerMovement.acceleration;
        // Set temp timers
        tempShieldTimer = shieldTimer;
        tempShieldCDTimer = shieldCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO:
        // when the shield is finished, shieldOnCooldown = true, while its true shieldCooldown will deplete
        // - Slowdown player when shielding (send data to Player Movement script)
        // Change acceleration in movement script
        // playerAcceleration -= shieldAcceleration * time.deltatime

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


        // If specialAttack has been parried send data to enemy
        if (specialAttackParried)
        {
            //enemyState = staggered;
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
        shieldTimer -= Time.deltaTime;

        // Slow down the player when shielding
        //if (!(playerMovement.acceleration <= slowAcceleration))
        //playerMovement.acceleration -= slowAcceleration * Time.deltaTime;
        playerMovement.acceleration = Mathf.SmoothDamp(playerMovement.acceleration, maxSlowAcceleration, ref speedSmoothVelocity, timeToAcceleration);

        if (shieldTimer <= 0)
        {
            sphereRenderer.enabled = false;
            shieldTimer = tempShieldTimer;
            shieldState = ShieldState.Cooldown;
        }
    }

    private void Cooldown()
    {
        shieldCooldown -= Time.deltaTime;
        playerMovement.acceleration = Mathf.SmoothDamp(playerMovement.acceleration, tempPlayerAcceleration, ref speedSmoothVelocity, timeToAcceleration);

        if (shieldCooldown <= 0)
        {
            shieldCooldown = tempShieldCDTimer;
            playerMovement.acceleration = tempPlayerAcceleration;
            shieldState = ShieldState.Default;
        }
    }
}