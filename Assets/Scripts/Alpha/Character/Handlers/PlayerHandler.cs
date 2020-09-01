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

    // Attributes
    [Header("Attributes")]
    public int maxHealth = 10;
    public float timeUntilRespawn = 5.0f;
    private int currentHealth = 10;

    // References
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private LocomotionHandler _locomotionHandler;
    private CombatHandler _combatHandler;
    private InputManager _inputManager;
    private CheckPoint _checkpoints;
    private Transform _respawnPosition;

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

        // Set the player position as a respawn point
        SetRespawnPosition(_transform);
    }

    public void RespawnPlayer()
    {
        // if (!isAlive)
        // {
            isAlive = true;
            currentHealth = maxHealth;
            StartCoroutine(MoveOverSeconds(this.gameObject, GetRespawnPosition().position));
            _transform.rotation = GetRespawnPosition().rotation;
        //}
    }

    // Move the player back to checkpoint position over a certain number of seconds
    private IEnumerator MoveOverSeconds (GameObject objectToMove, Vector3 end/* , float seconds */)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        capsule.enabled = false;
        _locomotionHandler.enabled = false;
        _rigidbody.useGravity = false;

        while (elapsedTime < timeUntilRespawn)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / timeUntilRespawn));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
        _locomotionHandler.enabled = true;
        capsule.enabled = true;
        _rigidbody.useGravity = true;
    }

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

    public Transform GetRespawnPosition()
    {
        return _respawnPosition;
    }

    public Transform SetRespawnPosition(Transform checkpointPosition)
    {
        Debug.Log("Set new respawn position at: (" + checkpointPosition.transform.position.x + ", " +
        checkpointPosition.transform.position.y + ", " + checkpointPosition.transform.position.z + ")");
        return _respawnPosition = checkpointPosition;
    }

    #endregion
}