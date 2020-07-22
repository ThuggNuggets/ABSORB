using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [Header("References")]
    // The transform the enemy will target
    public Transform playerTransform;

    public GameObject objectToSpawn;
    [Header("Spawner Properties")]
    public float numberOfEnemies = 1.0f;
    public int maxNumberOfSpawns = 0;
    public float addNumberOfEnemies = 0.2f;
    public float timeBetweenEachEnemySpawn = 5f;
    [Header("Keep off if you want to spawn first wave instantaneously")]
    public bool waitForSpawn = false;
    [Space]
    public Transform[] spawnerPositions;

    private string spawnTag;
    private int numberOfTaggedObjects = 0;
    private float tempSpawnTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
        tempSpawnTimer = timeBetweenEachEnemySpawn;
        spawnTag = objectToSpawn.tag;
    }

    // Update is called once per frame
    void Update()
    {
        // Only run if a objectToSpawn has been assigned
        if (objectToSpawn != null)
        {
            // TODO:
            // - Potentially spawn more enemies if 1 dies (e.g. 1 enemy dies, 2 more spawn)

            // Only run if we haven't reached the max number or we haven't set a max number
            if (numberOfTaggedObjects != maxNumberOfSpawns || maxNumberOfSpawns == 0)
            {
                tempSpawnTimer -= Time.deltaTime;

                if (tempSpawnTimer <= 0.0f)
                {
                    waitForSpawn = false;
                }

                // Allows the first wave to be spawned instantly instead of waiting
                // for the time between each enemy to reach 0 seconds
                if (!waitForSpawn)
                {
                    tempSpawnTimer -= Time.deltaTime;
                    for (int i = 0; i < (int)numberOfEnemies; i++)
                    {
                        int spawnNumber = Random.Range(0, spawnerPositions.Count());

                        Instantiate(objectToSpawn, spawnerPositions[spawnNumber].transform.position, Quaternion.identity).GetComponent<AIBrain>().playerTransform = this.playerTransform;
                    }

                    tempSpawnTimer = timeBetweenEachEnemySpawn;
                    numberOfEnemies += addNumberOfEnemies;
                    waitForSpawn = true;
                }
            }

            numberOfTaggedObjects = GameObject.FindGameObjectsWithTag(spawnTag).Length;
        }
    }
}