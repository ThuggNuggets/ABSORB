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

    public Dictionary<string, List<GameObject>> gameObjectsByTag;
    private string spawnTag;
    public int numberOfTaggedObjects = 0;
    private float tempSpawnTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
        gameObjectsByTag = new Dictionary<string, List<GameObject>>();
        tempSpawnTimer = timeBetweenEachEnemySpawn;
        spawnTag = objectToSpawn.tag;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
            RemoveEnemy(objectToSpawn);

        // Only run if a objectToSpawn has been assigned
        if (objectToSpawn != null)
        {
            // TODO:
            // - Potentially spawn more enemies if 1 dies (e.g. 1 enemy dies, 2 more spawn)

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
                    AddEnemy(objectToSpawn);
                }

                tempSpawnTimer = timeBetweenEachEnemySpawn;
                numberOfEnemies += addNumberOfEnemies;
                waitForSpawn = true;
            }

            numberOfTaggedObjects = GetEnemyFromTag(spawnTag).Count;
            //numberOfTaggedObjects = GameObject.FindGameObjectsWithTag(spawnTag).Length;
        }
    }

    // Come back to try and get working
    public void AddEnemy(GameObject enemy)
    {
        if (!gameObjectsByTag.ContainsKey(enemy.tag))
            gameObjectsByTag.Add(enemy.tag, new List<GameObject>());

        // Only run if we haven't reached the max number
        if (gameObjectsByTag[enemy.tag].Count < maxNumberOfSpawns || maxNumberOfSpawns == 0)
        {
            gameObjectsByTag[enemy.tag].Add(enemy);
            int spawnNumber = Random.Range(0, spawnerPositions.Count());
            AIBrain aIBrain = Instantiate(objectToSpawn, spawnerPositions[spawnNumber].transform.position, Quaternion.identity).GetComponent<AIBrain>();
            aIBrain.playerTransform = playerTransform;
            aIBrain.GetComponent<EnemyHandler>().SetupSpawner(this);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (gameObjectsByTag.ContainsKey(enemy.tag) && gameObjectsByTag[enemy.tag].Contains(enemy))
            gameObjectsByTag[enemy.tag].Remove(enemy);
    }

    public List<GameObject> GetEnemyFromTag(string tag)
    {
        if (!gameObjectsByTag.ContainsKey(tag))
            return null;
        else
            return gameObjectsByTag[tag];
    }

    //public void MoveEnemyToLayer(GameObject enemy, int newLayer)
    //{
    //    RemoveEnemy(enemy);
    //    enemy.layer = newLayer;
    //    AddEnemy(enemy);
    //}

}