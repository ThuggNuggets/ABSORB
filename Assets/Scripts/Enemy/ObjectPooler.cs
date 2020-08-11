using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private SpawnerV2 spawner;
    public Queue<GameObject> objectPool;

    public static ObjectPooler Instance;

    private void Awake()
    {
        // Singleton
        Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        spawner = FindObjectOfType<SpawnerV2>();

        foreach (Pool pool in pools)
        {
            objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                AIBrain aIBrain = obj.GetComponent<AIBrain>();
                aIBrain.playerTransform = spawner.playerTransform;
                aIBrain.GetComponent<EnemyHandler>().SetupSpawner(spawner);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        //if (!objectToSpawn.activeInHierarchy)
        //{
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        //poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
        //}
        //else
        //    return null;
    }
}
