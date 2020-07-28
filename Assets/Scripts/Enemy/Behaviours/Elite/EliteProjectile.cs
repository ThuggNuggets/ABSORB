﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EliteProjectile : MonoBehaviour
{
    [Header("VFX References")]
    public GameObject waterHitEffectGO;
    public ParticleSystem waterHitEffect;
    public AudioSource waterHitAudio;

    [Header("Properties")]
    public Vector3 directionOffset = Vector3.zero;
    public float effectTime = 2.0f;

    private Rigidbody _rb;
    private Vector3 _direction = Vector3.zero;
    private float _speed = 0.0f;
    private bool _isActive = false;
    private float _lifeTime = 0.0f;
    private Transform _parentTransform;
    private float _damage;

    // Gets called on awake
    private void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    // Sets up the direction for the projectile
    public void InitialiseProjectile(Transform parentTransform, Transform playerTransform, Transform projectileStartPoint, float speed, float lifeTime, float damage)
    {

        transform.position = projectileStartPoint.position;
        transform.rotation = projectileStartPoint.rotation;
        _parentTransform = parentTransform;
        _isActive = true;

        float distance = Vector3.Distance(playerTransform.position, transform.position);

        Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
        Vector3 playerPos = playerTransform.position + directionOffset;
        _direction = ((playerPos + ((rb.velocity.normalized * (speed + distance)) * Time.fixedDeltaTime)) - transform.position).normalized;

        _speed = speed;
        _lifeTime = lifeTime;
        _damage = damage;
        StartCoroutine(LifeTimer());
    }

    // Gets called every frame
    private void Update()
    {
        if(_isActive)
            _rb.MovePosition(transform.position + _direction * _speed * Time.deltaTime);
    }

    // Returns the damage of this projectile
    public float GetDamage()
    {
        return _damage;
    }

    // Returns true if the projectile is still active within the scene.
    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }

    private void OnTriggerEnter(Collider collision)
    {
        waterHitEffect.Play();
        waterHitAudio.Play();
        waterHitEffectGO.transform.SetParent(null);
        Destroy(waterHitEffectGO, effectTime);
        Destroy(this.gameObject);
    }

    public IEnumerator LifeTimer()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy(this.gameObject);
    }
}
