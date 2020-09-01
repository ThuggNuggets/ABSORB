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
    private int currentHealth = 10;

    // References
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private LocomotionHandler _locomotionHandler;
    private CombatHandler _combatHandler;
    private InputManager _inputManager;

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

    #endregion
}