using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorb : MonoBehaviour
{
    /*
     * TODO:
     * Slowdown player. (Reference Will's script)
     * Make the player face the enemy they are absorbing.
     * Play the abosrb animations on both enemy and player.
     * Set players ability.
     * Kill enemy.
     */


    // The enemy which we will abosrb
    private AIBrain _targetEnemy = null;
    public AIBrain TargetEnemy
    {
        get { return _targetEnemy;  }
        set { _targetEnemy = value; }
    }

    [Header("Properties")]
    // The mouse button that controls the activation of the absorb.
    [Range(0, 2)]
    public int mouseButtonInput = 1;
    public float turnSpeed = 0.5F;
    public float animationTime = 2.0f;
    private bool _isAbosrbing = false;
    private PlayerSlowdown playerSlowdown;

    private void Awake()
    {
        playerSlowdown = this.GetComponent<PlayerSlowdown>();
    }

    // Called every frame
    private void Update()
    {
        if(_targetEnemy)
        {
            // Check if we should start abosrbing
            if (Input.GetMouseButtonDown(mouseButtonInput) && !_isAbosrbing)
            {
                Activate();
                StartCoroutine(WaitFor(animationTime));
            }
        }
    }

    // Returns true after n amount of seconds
    private IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Deactivate();
    }

    public void Activate()
    {
        Debug.Log("Activate");
        playerSlowdown.SetSlowdown();
        _isAbosrbing = true;
    }

    public void Deactivate()
    {
        playerSlowdown.SetSpeedUp();
        _isAbosrbing = false;
    }

    public bool IsActive()
    {
        return _isAbosrbing;
    }
}