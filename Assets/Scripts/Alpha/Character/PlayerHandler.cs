using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public int maxHealth = 10;
    private bool isAlive = false;
    private int currentHealth = 10;
    private bool canShield = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool GetCanShield()
    {
        return canShield;
    }
}
