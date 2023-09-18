using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private int _resizeSize;
    [SerializeField] private int _maxSize;

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)
        {
            Queue<GameObject> objectsPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectsPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectsPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        GameObject obj = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(obj);

        int count = 0;
        while (obj.activeInHierarchy)
        {
            if (count == _resizeSize)
            {
                if (UpsizePool(tag, _resizeSize) == pools.Find(x => x.tag == tag).size)
                    break;
            }
            else
            {
                count++;

                obj = poolDictionary[tag].Dequeue();
                poolDictionary[tag].Enqueue(obj);
            }
        }

        return obj;
    }

    public int UpsizePool(string tag, int size)
    {
        var poolQueue = poolDictionary[tag];

        int poolIndex = pools.FindIndex(x => x.tag == tag);
        Pool targetPool = pools[poolIndex];

        if (targetPool.size >= _maxSize)
            return targetPool.size;
        else
        {
            targetPool.size += size;
            pools[poolIndex] = targetPool;

            for (int i = 0; i < size; ++i)
            {
                GameObject obj = Instantiate(targetPool.prefab);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }

            return targetPool.size;
        }
    }

    public int DownsizePool(string tag, int size)
    {
        var poolQueue = poolDictionary[tag];

        int poolIndex = pools.FindIndex(x => x.tag == tag);
        Pool targetPool = pools[poolIndex];
        targetPool.size -= size;
        pools[poolIndex] = targetPool;

        for (int i = 0; i < size; ++i)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(false);
            Destroy(obj);
        }

        return targetPool.size;
    }
}
