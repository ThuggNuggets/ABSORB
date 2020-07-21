using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EliteProjectile : MonoBehaviour
{
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
    public void InitialiseProjectile(Transform parentTransform, Vector3 playerPosition, Transform projectileStartPoint, float speed, float lifeTime, float damage)
    {
        transform.position = projectileStartPoint.position;
        transform.rotation = projectileStartPoint.rotation;
        _parentTransform = parentTransform;
        _isActive = true;
        _direction = (playerPosition - transform.position).normalized;
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

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    public IEnumerator LifeTimer()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy(this.gameObject);
    }
}
