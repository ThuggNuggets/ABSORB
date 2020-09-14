﻿using System.Collections;
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

    // Attributes
    [Header("Attributes")]
    public int maxHealth = 10;
    public float respawnFlyingTime = 5.0f;
    public float respawnFlyingHeight = 20.0f;
    private int _currentHealth = 10;

    // References
    [Header("References")]
    private float offset = 10.0f;
    public SkinnedMeshRenderer abidaroMesh;
    public GameObject respawnParticle;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private LocomotionHandler _locomotionHandler;
    private CombatHandler _combatHandler;
    private InputManager _inputManager;
    private CheckPoint _checkpoints;
    private Vector3 _respawnPosition;
    private CapsuleCollider _capsule;

    Vector3 point = Vector3.zero;
    Vector3 point2 = Vector3.zero;

    // Flags
    private bool isAlive = true;

    void Awake()
    {
        // Temporarily setting cursor locked state within this script.
        Cursor.lockState = CursorLockMode.Locked;

        // Getting the required components
        _animator = this.GetComponent<Animator>();
        _rigidbody = this.GetComponent<Rigidbody>();
        _transform = this.GetComponent<Transform>();
        _locomotionHandler = this.GetComponent<LocomotionHandler>();
        _combatHandler = this.GetComponent<CombatHandler>();
        _inputManager = FindObjectOfType<InputManager>();
        _checkpoints = FindObjectOfType<CheckPoint>();
        _capsule = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        // Set the player position as a respawn point
        SetRespawnPosition(this._transform.position); // Might have to rewrite to be Vector3 instead of transform
        abidaroMesh.enabled = true;
        respawnParticle.SetActive(false);
    }

    #region Player Respawn

    public void RespawnPlayer()
    {
        // if (!isAlive)
        // {
        isAlive = true;
        _currentHealth = maxHealth;
        StartCoroutine(MoveOverSeconds(this.gameObject, GetRespawnPosition(), respawnFlyingHeight, respawnFlyingTime));
        //_transform.rotation = GetRespawnPosition().rotation;
        //}
    }

    // Move the player back to checkpoint position over a certain number of seconds
    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float height, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;

        // Stuff to disable while flying
        DisableReferences();

        while (elapsedTime < seconds)
        {
            //objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            objectToMove.transform.position = MathParabola.Parabola(startingPos, end, 20.0f, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Turn that stuff back on after flight is complete
        EnableReferences();

        // Set to end position as a failsafe
        objectToMove.transform.position = end;
    }

    // Get a mid point between two positions
    private Vector3 GetPoint(Vector3 object1, Vector3 object2)
    {
        //get the positions of our transforms
        Vector3 pos1 = object1;
        Vector3 pos2 = object2;

        //get the direction between the two transforms -->
        Vector3 dir = (pos2 - pos1).normalized;

        //get a direction that crosses our [dir] direction
        //NOTE! : this can be any of a buhgillion directions that cross our [dir] in 3D space
        //To alter which direction we're crossing in, assign another directional value to the 2nd parameter
        Vector3 perpDir = Vector3.Cross(dir, Vector3.right);

        //get our midway point
        Vector3 midPoint = (pos1 + pos2) / 2f;

        //get the offset point
        //This is the point you're looking for.
        Vector3 offsetPoint = midPoint + (perpDir * offset);

        return offsetPoint;
    }

    private void DisableReferences()
    {
        abidaroMesh.enabled = false;
        respawnParticle.SetActive(true);
        _capsule.enabled = false;
        _locomotionHandler.enabled = false;
        _rigidbody.useGravity = false;
    }

    private void EnableReferences()
    {
        abidaroMesh.enabled = true;
        respawnParticle.SetActive(false);
        _locomotionHandler.enabled = true;
        _capsule.enabled = true;
        _rigidbody.useGravity = true;
        _inputManager.EnableInput();
    }

    #endregion

    #region Getters and setters

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public bool SetIsAlive(bool aliveState)
    {
        return isAlive = aliveState;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    // Return the amount of damage the player should take
    public float TakeDamage(int damageAmount)
    {
        //hitParticleSystem.Play();
        // hitSoundEffect.Play();
        //collidedObject = null;
        // if (debug)
        //     Debug.Log("Player damage taken: " + damageAmount);
        return _currentHealth -= damageAmount;
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

    public Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    public Transform GetTransform()
    {
        return _transform;
    }

    public LocomotionHandler GetLocomotionHandler()
    {
        return _locomotionHandler;
    }

    public CombatHandler GetCombatHandler()
    {
        return _combatHandler;
    }

    public CheckPoint GetCheckPoint()
    {
        return _checkpoints;
    }

    public Vector3 GetRespawnPosition()
    {
        return _respawnPosition;
    }

    public Vector3 SetRespawnPosition(Vector3 checkpointPosition)
    {
        // Debug.Log("Set new respawn position at: (" + checkpointPosition.x + ", " +
        // checkpointPosition.y + ", " + checkpointPosition.z + ")");
        return _respawnPosition = checkpointPosition;
    }

    #endregion
}