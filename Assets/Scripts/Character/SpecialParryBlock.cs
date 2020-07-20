using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialParryBlock : MonoBehaviour
{
    public MeshRenderer sphereRenderer;
    [Range(0.1f, 5f)]
    public float shieldTimer = 1.0f;
    
    private float tempShieldTimer;

    [HideInInspector]
    public bool playerShielded = false;
    [HideInInspector]
    public bool specialAttackParried = false;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure the blocking sphere is turned off by default
        sphereRenderer.enabled = false;
        tempShieldTimer = shieldTimer;
    }

    // Update is called once per frame
    void Update()
    {
        EnableShield();

        // If specialAttack has been parried send data to enemy
        if (specialAttackParried)
        {
            //enemyState = staggered;
        } 
    }

    void EnableShield()
    {
        // When the player hits the shield key
        if (Input.GetKeyDown(KeyCode.Space))
            // playerShielded is set to true
            playerShielded = true;
        // after shieldTime has expired, playerShielded is set to false
        else if (shieldTimer <= 0)
        {
            playerShielded = false;
            shieldTimer = tempShieldTimer;
        }


        if (playerShielded && shieldTimer > 0)
        {
            sphereRenderer.enabled = true;
            shieldTimer -= Time.deltaTime;
        }
        else
            sphereRenderer.enabled = false;
    }
}
