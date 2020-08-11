using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // Enemy type
    public enum EnemyType
    {
        MINION,
        SPECIAL,
        ELITE,
    }

    [Header("References")]
    public ParticleSystem damageEffect;
    public AudioSource damageEffectAudio;

    [Header("Parry Effect")]
    public bool hasParryEffect = false;
    public ParticleSystem parryEffect;
    public AudioSource parryAudio;
    public float parryHitEffectTime = 2.0f;
    private Transform _parryParticleParent;

    [Header("Death FX")]
    public AudioSource deathSound;
    public float deathSoundLength = 2.0f;
    public ParticleSystem deathParticleEffect;
    public float deathParticleLength = 2.0f;

    [Header("Properties")]
    public EnemyType typeOfEnemy;
    public float maxHealth = 100.0f;
    public float baseDamage = 10.0f;

    [Header("Debug Options")]
    public bool printHealthStats = false;

    private float _currentHealth = 0.0f;
    private bool _isAlive = true;

    // The collider of this enemies weapon
    public Collider weaponCollider;

    // The brain of this enemy
    private AIBrain _aiBrain;

    // Reference to the spawner which created this enemy
    private SpawnerV2 _spawner;

    //


    private void Awake()
    {
        // Setting the current health to be max
        _currentHealth = maxHealth;

        // Getting the components
        _aiBrain = this.GetComponent<AIBrain>();

        // Get the particle parent
        _parryParticleParent = parryEffect.transform.parent;
    }

    private void Update()
    {
        // Checking if the enemy is still alive
        if (!_isAlive)
            _aiBrain.SetBehaviour("Death");
    }

    // Currently just destroying the enemy if the player attacks them
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerMelee") && other.gameObject.layer == LayerMask.NameToLayer("PlayerWeapon"))
            TakeDamage(25.0f);
    }

    // Makes the enemy take damage
    public void TakeDamage(float damage, AbilityManager.E_Ability e_Ability = AbilityManager.E_Ability.NONE)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
            _isAlive = false;

        if (typeOfEnemy != EnemyType.ELITE)
            _aiBrain.SetBehaviour("Stagger");

        else if (e_Ability == AbilityManager.E_Ability.HAMMER)
            _aiBrain.SetBehaviour("Stagger");

        damageEffect.Play();
        damageEffectAudio.Play();

        if (printHealthStats)
            Debug.Log(gameObject.tag + " took damage. Current health: " + _currentHealth);
    }

    // Plays the hit parry effect
    public void PlayHitParryEffect()
    {
        if(hasParryEffect)
        {
            parryEffect.transform.SetParent(null);
            parryEffect.Play();
            parryAudio.Play();
            StartCoroutine(ReparentHitEffect());
        }
    }

    private IEnumerator ReparentHitEffect()
    {
        yield return new WaitForSecondsRealtime(parryHitEffectTime);
        parryEffect.transform.SetParent(_parryParticleParent);
    }

    // Returns true if the entity is still alive
    public bool IsAlive()
    {
        return _isAlive;
    }

    // Returns the base damage of the enemy.
    public float GetDamage()
    {
        return baseDamage;
    }

    // Activates the weapons collider
    public void ActivateWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    // Deactivates the weapons collider
    public void DeactiveWeaponCollider()
    {
        weaponCollider.enabled = false;
    }

    // Sets up the spawner
    public void SetupSpawner(SpawnerV2 spawner)
    {
        _spawner = spawner;
    }

    // Returns the brain of this enemy
    internal AIBrain GetBrain()
    {
        return _aiBrain;
    }

    // Returns the spawner
    public SpawnerV2 GetSpawner()
    {
        return _spawner;
    }

    // Kills the enemy
    public void Kill()
    {
        // Remove enemy spawner
        //if (_spawner != null)
        //    _spawner.RemoveEnemy(this.gameObject);

        // Playing VFX
        deathParticleEffect.transform.SetParent(null);
        deathSound.transform.SetParent(null);
        deathParticleEffect.Play();
        deathSound.Play();

        // Destroying everything
        //Destroy(deathParticleEffect.gameObject, deathParticleLength);
        //Destroy(deathSound.gameObject, deathSoundLength);
        //Destroy(this.gameObject);
        gameObject.SetActive(false);
        _currentHealth = 75;
        _aiBrain.SetBehaviour("Idle");
        ObjectPooler.Instance.poolDictionary[gameObject.tag].Enqueue(gameObject);
    }
}
