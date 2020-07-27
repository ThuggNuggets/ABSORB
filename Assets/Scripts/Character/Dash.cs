using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Properties")]
    public float force = 50.0f;
    public float cooldownTime = 5.0f;
    public KeyCode inputKey;
    public float distance = 20.0f;
    private bool _canDash = true;
    private Rigidbody _rigidbody;
    private Vector3 _initialVelocity = Vector3.zero;
    private Vector3 _initialPosition = Vector3.zero;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(inputKey) && _canDash)
        {
            _initialVelocity = _rigidbody.velocity;
            _initialPosition = transform.position;
            _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
            _canDash = false;
            StartCoroutine(CoolDownSequence());
        }
        
        if(!_canDash)
        {
            float d = Vector3.Distance(transform.position, _initialPosition);
            if(d > distance)
            {
                _rigidbody.velocity = _initialVelocity;
                _initialVelocity = Vector3.zero;
                _initialPosition = Vector3.zero;
            }
        }
    }

    private IEnumerator CoolDownSequence()
    {
        yield return new WaitForSecondsRealtime(cooldownTime);
        _canDash = true;
        Debug.Log("Dash ready...");
    }
}
