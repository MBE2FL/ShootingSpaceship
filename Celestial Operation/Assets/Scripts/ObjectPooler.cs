using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    // Class used to store information about a memory pool
    [System.Serializable]
    public class PoolInfo
    {
        public string tag;
        public GameObject prefab;
        public int size;
        /// <summary>
        /// Should this memory pool resize itself, if it runs out of objects.
        /// </summary>
        public bool resize = false;
        public int currentSize;
    }


    // Singleton reference
    public static ObjectPooler Instance;

    void Awake()
    {
        Instance = this;
    }


    // A list which contains the information for each memory pool
    public List<PoolInfo> poolInfos;
    // A dictionary which contains a all the memeroy pools for each specified object in poolInfos
    public Dictionary<string, List<GameObject>> objectPools;


    // Use this for initialization
    void Start()
    {
        // For each element in poolInfos, create and store a pool of specified objects in objectPools
        objectPools = new Dictionary<string, List<GameObject>>();

        foreach (PoolInfo poolInfo in poolInfos)
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < poolInfo.size; i++)
            {
                GameObject obj = Instantiate(poolInfo.prefab);
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            objectPools.Add(poolInfo.tag, objectPool);

            poolInfo.currentSize = poolInfo.size;
        }
    }


    public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        // Check if a memory pool exists for the given tag
        if (!objectPools.ContainsKey(tag))
        {
            Debug.LogWarning("Object pool with tag " + tag + " doesn't exist.");
            return null; ;
        }

        GameObject obj = null;
        // Find the tag's corresponding memory pool
        List<GameObject> pool = objectPools[tag];

        // Search the pool for an inactive object
        for (int i = 0; i < pool.Count; i++)
        {
            obj = pool[i];

            // Current object is not active in the scene
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj;
            }
        }

        // Resize the pool if no inactive object could be found in the pool, and the pool is resizable
        foreach (PoolInfo poolInfo in poolInfos)
        {
            if (poolInfo.tag == tag)
            {
                if (poolInfo.resize)
                {
                    obj = Instantiate(poolInfo.prefab, position, rotation);
                    obj.SetActive(true);
                    pool.Add(obj);
                    poolInfo.currentSize++;
                    return obj;
                }
                else
                {
                    return obj;
                }
            }
        }

        return obj;
    }
}
