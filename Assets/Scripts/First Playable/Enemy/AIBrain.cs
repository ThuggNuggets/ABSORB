using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIBrain : MonoBehaviour
{
    [Header("References")]
    // The transom the enemy will target
    public Transform playerTransform;

    // Array list of information to fill the dictionary with
    [System.Serializable]
    public struct AIBehaviourInfo
    {
        public string name;
        public AIBehaviour behaviour;
    }
    public AIBehaviourInfo[] behaviourInformation;

    // The public list of behaviours setup by user
    public Dictionary<string, AIBehaviour> _aiBehaviours = new Dictionary<string, AIBehaviour>();

    // Options to assist with debugging
    [Header("Debug Options")]
    public bool printCurrentState = false;
    public string currentStateReadOnly = "NULL";

    // Rigidbody attached to this object
    private Rigidbody _rigidbody;

    // Transform attached to this object
    private Transform _transform;

    // This enemy's handler.
    private EnemyHandler _handler;

    // Current state ID of the fsm
    private string _currentBehaviourID = "";

    // Called on initialise
    private void Awake()
    {
        // Get rigidbody attached to the gameobject
        _rigidbody = this.GetComponent<Rigidbody>();

        // Get the transform of the gameobject
        _transform = this.GetComponent<Transform>();

        // Get the enemy handler
        _handler = this.GetComponent<EnemyHandler>();

        // Fill out dictionary
        foreach (AIBehaviourInfo bi in behaviourInformation)
            _aiBehaviours.Add(bi.name, bi.behaviour);

        // Initialise references within states
        foreach (AIBehaviour aib in _aiBehaviours.Values)
            aib.InitialiseState(this);

        // Set current behaviour state
        _currentBehaviourID = behaviourInformation[0].name;
    }

    // Called every frame
    private void Update()
    {
        // Debug option to print current state
        if (printCurrentState)
            DebugStateMachine();

        _aiBehaviours[_currentBehaviourID].OnUpdate();
    }

    // Called every fixed update
    private void FixedUpdate()
    {
        _aiBehaviours[_currentBehaviourID].OnFixedUpdate();
    }

    // Sets the current state to the next state, calling exit/enter functions
    internal void SetBehaviour(string behaviour)
    {
        // Absorb quick fix: if in end state of machine, do NOT change state.
        if (_currentBehaviourID == "Absorbed")
            return;

        // Call OnExit() before the state switch, then call OnEnter() after.
        _aiBehaviours[_currentBehaviourID].OnExit();
        _currentBehaviourID = behaviour;
        _aiBehaviours[_currentBehaviourID].OnEnter();

        // Setting the new current state ID for debugging purposes.
        currentStateReadOnly = _currentBehaviourID;
    }

    // Returns the distance from this enemy and the player
    internal float GetDistanceToPlayer()
    {
        return Vector3.Distance(_transform.position, playerTransform.position);
    }

    // Returns the direction from this enemy to the player
    internal Vector3 GetDirectionToPlayer()
    {
        return (playerTransform.position - _transform.position).normalized;
    }

    // Returns rigidbody attached to this enemy
    internal Rigidbody GetRigidbody()
    {
        return _rigidbody;
    }

    // Returns transform attached to this enemy
    internal Transform GetTransform()
    {
        return _transform;
    }    
    
    // Returns this enemy's handler
    internal EnemyHandler GetHandler()
    {
        return _handler;
    }

    // Prints a debug message to unity's console
    public void DebugStateMachine()
    {
        Debug.Log(_currentBehaviourID);
    }
}

