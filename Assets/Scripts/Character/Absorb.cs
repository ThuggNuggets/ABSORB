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

    [Header("References")]
    public PlayerSlowdown playerSlowdown;

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
    public float animationTime = 2.0f;
    private bool _isAbosrbing = false;

    // Called every frame
    private void Update()
    {
        // Check if we should start abosrbing
        if(_targetEnemy && Input.GetMouseButtonDown(mouseButtonInput) && !_isAbosrbing)
        {
            playerSlowdown.SetSlowdown();
            _isAbosrbing = true;
            StartCoroutine(WaitFor(animationTime));
        }
    }

    // Returns true after n amount of seconds
    private IEnumerator WaitFor(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }
}
