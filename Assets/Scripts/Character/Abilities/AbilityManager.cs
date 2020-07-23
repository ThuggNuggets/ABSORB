using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public enum E_Ability
    {
        NONE,
        HAMMER,
        // NEW_ABILITY_HERE,
        COUNT,
    }

    [Header("Properties")]
    // The current ability in use. Do NOT use this to change the current state. Use SetAbility() instead.
    public E_Ability startingAbility = E_Ability.NONE;
    private E_Ability _currentAbility;

    // The mouse button that controls the use of the ability
    [Range(0, 2)]
    public int mouseButtonInput = 1;

    // Array to fill out ability dictionary with
    [System.Serializable]
    public struct AbilityInformation
    {
        public E_Ability e_Ability;
        public Ability ability;
    }
    [Header("References")]
    public AbilityInformation[] abilityInformation;

    // Dictionary to access abilties via enum
    private Dictionary<E_Ability, Ability> _abilityDictionary = new Dictionary<E_Ability, Ability>();

    // Options to assist with debugging
    [Header("Debug")]
    public bool printCurrentState = false;

    // Called on initialise
    private void Awake()
    {
        // Fill out dictionary
        foreach (AbilityInformation ai in abilityInformation)
            _abilityDictionary.Add(ai.e_Ability, ai.ability);

        // Initialise references within states
        foreach (Ability a in _abilityDictionary.Values)
            a.InitialiseAbility(this);

        // Set the current ability to the starting ability
        if(startingAbility != E_Ability.NONE)
            SetAbility(startingAbility);
    }

    // Called every frame
    private void Update()
    {
        // Exit function if player doesn't have an active ability
        if (_currentAbility == E_Ability.NONE)
            return;

        // Checks if the player uses the ability
        if (!_abilityDictionary[_currentAbility].Active)
        {
            if (Input.GetMouseButtonDown(mouseButtonInput))
                _abilityDictionary[_currentAbility].Activate();
        }
        else
            _abilityDictionary[_currentAbility].OnUpdate();

        // Debug printing if option enabled
        if (printCurrentState)
            Debug.Log(_currentAbility);
    }

    // Called every physics update
    private void FixedUpdate()
    {
        // Exit function if player doesn't have an active ability
        if (_currentAbility == E_Ability.NONE || !_abilityDictionary[_currentAbility].Active)
            return;

        // Updating current ability
        _abilityDictionary[_currentAbility].OnFixedUpdate();
    }

    // Sets the current ability
    public void SetAbility(E_Ability ability)
    {
        if(_currentAbility != E_Ability.NONE)
            _abilityDictionary[_currentAbility].OnExit();

        _currentAbility = ability;

        if (_currentAbility != E_Ability.NONE)
            _abilityDictionary[_currentAbility].OnEnter();


    }
}
