﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AIBrain : MonoBehaviour
{
    [Header("Debug Options")]
    public bool printCurrentState = false;

    [Header("References")]
    // The player the AI will detect
    public Transform playerTransform;

    [Header("Damage")]
    public float baseDamage = 10.0f;

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

    // Rigidbody attached to this object
    private Rigidbody _rigidbody;

    // Transform attached to this object
    private Transform _transform;

    // Current state ID of the fsm
    private string _currentBehaviourID = "";

    // Called on initialise
    private void Awake()
    {
        // Get rigidbody attached to the gameobject
        _rigidbody = this.GetComponent<Rigidbody>();

        // Get the transform of the gameobject
        _transform = this.GetComponent<Transform>();

        // Fill out dictionary
        foreach (AIBehaviourInfo bi in behaviourInformation)
            _aiBehaviours.Add(bi.name, bi.behaviour);

        // Initialise references within states
        foreach (AIBehaviour aib in _aiBehaviours.Values)
            aib.InitialiseState(this);

        // Set current behaviour state
        _currentBehaviourID = behaviourInformation[0].name;
    }

    // Called before the current state update
    private void AnyStateUpdate() 
    {
        // Debug option to print current state
        if (printCurrentState)
            DebugStateMachine();
    }

    // Called every frame
    private void Update()
    {
        AnyStateUpdate();
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
        _aiBehaviours[_currentBehaviourID].OnExit();
        _currentBehaviourID = behaviour;
        _aiBehaviours[_currentBehaviourID].OnEnter();
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

    // Prints a debug message to unity's console
    public void DebugStateMachine()
    {
        Debug.Log(_currentBehaviourID);
    }

    // Currently just destroying the enemy if the player attacks them
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerWeapon"))
            Destroy(this.gameObject);
    }

    // Returns the base damage of the enemy.
    public float GetDamage()
    {
        return baseDamage;
    }
}

