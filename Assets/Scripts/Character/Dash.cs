using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Properties")]
    public float force = 50.0f;
    public float cooldownTime = 5.0f;
    public KeyCode inputKey;
    private bool _canDash = true;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(inputKey) && _canDash)
        {
            _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
            _canDash = false;
            StartCoroutine(CoolDownSequence());
        }
    }

    private IEnumerator CoolDownSequence()
    {
        yield return new WaitForSecondsRealtime(cooldownTime);
        _canDash = true;
        Debug.Log("Dash ready...");
    }
}
