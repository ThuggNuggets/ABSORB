using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerV2 : MonoBehaviour
{
    [Header("References")]
    // The transform the enemy will target
    public Transform playerTransform;
    public string objectToSpawnTag;
    [Header("Spawner Properties")]
    public float timeBetweenEachEnemySpawn = 5f;
    [Header("Only spawn the max number of enemies")]
    public bool setSpawnAmount = false;
    [Header("Keep off if you want to spawn first wave instantaneously")]
    public bool waitForSpawn = false;
    [Space]
    public Transform[] spawnerPositions;

    //public int numberOfTaggedObjects = 0;
    private float _tempSpawnTimer = 0.0f;
    public int objectPoolCount;
    private int checkAmountOfSpawns;
    public int numberOfTaggedObjects;

    ObjectPooler objectPooler;

    // Use this for initialization
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        _tempSpawnTimer = timeBetweenEachEnemySpawn;
        objectPoolCount = objectPooler.objectPool.Count();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO:
        // - Potentially spawn more enemies if 1 dies (e.g. 1 enemy dies, 2 more spawn)
        _tempSpawnTimer -= Time.deltaTime;


        if (_tempSpawnTimer <= 0.0f)
        {
            waitForSpawn = false;
        }

        // Allows the first wave to be spawned instantly instead of waiting
        // for the time between each enemy to reach 0 seconds
        if (!waitForSpawn)
        {
            if (numberOfTaggedObjects < objectPoolCount && objectPooler.objectPool.Count() > 0)
            {
                //_tempSpawnTimer -= Time.deltaTime;
                //for (int i = 0; i < ObjectPooler.Instance.poolDictionary.Count(); i++)
                //{
                int spawnNumber = Random.Range(0, spawnerPositions.Count());
                objectPooler.SpawnFromPool(objectToSpawnTag, spawnerPositions[spawnNumber].transform.position, Quaternion.identity);
                //}

                _tempSpawnTimer = timeBetweenEachEnemySpawn;
                waitForSpawn = true;
            }
        }

        numberOfTaggedObjects = GameObject.FindGameObjectsWithTag(objectToSpawnTag).Length;
    }
}
