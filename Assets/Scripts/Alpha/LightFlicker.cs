using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float minLightRange = 5f;
    [Range(0.0f, 100.0f)]
    public float frequencyPercentage = 70.0f;
    public float flickerTime = 0.3f;
    public float maxTimeBetweenFlickers = 0.5f;

    private Light lightObject;
    private bool flicker = true;
    private float maxRange;
    private float randNumber;
    private float tempTimer;

    // Start is called before the first frame update
    void Start()
    {
        lightObject = this.GetComponent<Light>();
        maxRange = lightObject.range;
    }

    // Update is called once per frame
    void Update()
    {
        tempTimer += Time.deltaTime;

        if (tempTimer >= maxTimeBetweenFlickers)
        {
            // Get a random number
            randNumber = CheckForFlicker();
            // if that random number is less than the frequency then flicker
            if (randNumber <= frequencyPercentage)
            {
                lightObject.range = minLightRange;
                StartCoroutine(Flicker());
            }
        }
    }

    private IEnumerator Flicker()
    {
        yield return new WaitForSeconds(flickerTime);
        tempTimer = 0f;
        lightObject.range = maxRange;
    }

    private float CheckForFlicker()
    {
        return Random.Range(0.0f, 100.0f);
    }
}
