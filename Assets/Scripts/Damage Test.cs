using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    public float enemyDamage = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        //if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.gameObject.GetComponent<Health>().DealDamage(enemyDamage);
            Debug.Log("Collided with player");
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //        other.gameObject.GetComponent<Health>().DealDamage(enemyDamage);
    //        Debug.Log("Collided with player");
    //    }
    //}
}
