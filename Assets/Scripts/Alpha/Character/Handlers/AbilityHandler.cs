using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AbilitySickle))]
[RequireComponent(typeof(AbilityHammer))]
[RequireComponent(typeof(AbilityPot))]
public class AbilityHandler : MonoBehaviour
{
    public enum AbilityType
    {
        NONE,
        SICKLE,
        HAMMER,
        POT,
    }

    [Header("Parried Check Properties")]
    public LayerMask parriedCastLayer;
    public float parriedCastRadius = 5.0f;

    private Ability[] _abilities;
    private AbilityType _currentAbility = AbilityType.NONE;
    private List<RaycastHit> _sortedHitList = new List<RaycastHit>();
    private PlayerHandler _playerHandler;
    private InputManager _inputManager;
    private LocomotionHandler _locomotionHanlder;
    private Animator _animator;

    // Called on initialise
    private void Start()
    {
        // Getting the player handler
        _playerHandler = this.GetComponent<PlayerHandler>();

        // Assign all the abilites
        _abilities = new Ability[(int) AbilityType.POT + 1];
        _abilities[(int) AbilityType.SICKLE] = this.GetComponent<AbilitySickle>();
        _abilities[(int) AbilityType.HAMMER] = this.GetComponent<AbilityHammer>();
        _abilities[(int) AbilityType.POT]    = this.GetComponent<AbilityPot>();

        // Getting the references out of the player handler
        _inputManager = _playerHandler.GetInputManager();
        _locomotionHanlder = _playerHandler.GetLocomotionHandler();
        _animator = _playerHandler.GetAnimator();
    }

    // Called every frame
    private void Update()
    {
        // Check for abosrb
        if (_currentAbility == AbilityType.NONE)
        {
            // Checking if the player requests to absorb
            CheckForAbosrb();

            // Returning out of this update function
            return;
        }
        else
        {
            // Checking if the player requests to acitvate their current ability
            if (_inputManager.GetSpecialAttackButtonPress())
                _abilities[(int) _currentAbility].Activate();
        }
    }

    // Gets called every frame we don't have an ability
    private void CheckForAbosrb()
    {
        // Checking if player has inputed the absorb
        if (_inputManager.GetSpecialAttackButtonPress())
        {
            // Get the closest enemy, then set them to the absorbed state
            EnemyHandler enemy = GetClosestParriedEnemy();
            if (enemy != null)
            {
                _animator.SetBool("Absorb", true);
                _playerHandler.GetLocomotionHandler().Key_ActivateSlowdown();
                enemy.GetBrain().SetBehaviour("Absorbed");
                this.SetAbility(enemy.GetAbilityType());
            }
        }
    }

    // Key Event: Deactivates abosrb once activated; only to be called through animation
    public void Key_DeactivateAbsorb()
    {
        _animator.SetBool("Absorb", false);
        _playerHandler.GetLocomotionHandler().Key_DeactivateSlowdown();
    }

    // Sets the current ability
    public void SetAbility(AbilityType nextAbility)
    {
        if (_currentAbility != AbilityType.NONE)
            _abilities[(int)_currentAbility].OnExit();

        _currentAbility = nextAbility;
        _abilities[(int)_currentAbility].OnEnter();
    }

    // Returns the closest parried enemy to the player
    public EnemyHandler GetClosestParriedEnemy()
    {
        // Clearing the map
        _sortedHitList.Clear();

        // Scan for enemies within radius of player
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, parriedCastRadius, Vector3.up, 0.0f, parriedCastLayer);

        // If we don't hit anything, print a warning then exit this function
        if (hits.Length <= 0)
        {
            Debug.LogWarning("Ability Handler: Sphere cast didn't hit any enemies.");
            return null;
        }

        // Populating a list with collided transforms and distances from origin
        foreach (RaycastHit hit in hits)
            _sortedHitList.Add(hit);

        // Sorting list based on distance
        _sortedHitList.Sort((h1, h2) => h1.distance.CompareTo(h2.distance));

        // Returning the closest enemy that is parried
        for (int i = 0; i < _sortedHitList.Count; ++i)
        {
            EnemyHandler enemyHandler = _sortedHitList[i].transform.GetComponent<EnemyHandler>();
            if (enemyHandler.IsParried())
                return enemyHandler;
        }

        // Returning null and logging an error, since we shouldn't get here
        Debug.LogError("Ability Handler: GetClosestParriedEnemy() reached end of function without finding an enemy.");
        return null;
    }
}
