﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SimpleObjectPool : MonoBehaviour
{
    public static SimpleObjectPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }       
    }

    public Dictionary<string, List<GameObject>> allPools = new Dictionary<string, List<GameObject>>();

    public GameObject GetObjectFromPool(GameObject obj, Transform parent)
    {
        string key = obj.name;

        if (!allPools.ContainsKey(key))
        {
            var newObject = Instantiate(obj, parent);
            newObject.transform.position = parent.position;
            var newPool = new List<GameObject>();
            newPool.Add(newObject);
            allPools.Add(key, newPool);
            return newObject;
        }

        var pool = allPools[key];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                pool[i].transform.rotation = Quaternion.identity;
                pool[i].transform.parent = parent;
                pool[i].transform.position = parent.position;
                return pool[i];
            }
        }
        var addObject = Instantiate(obj, parent);
        addObject.transform.position = parent.position;
        pool.Add(addObject);
        return addObject;
    }
    public GameObject GetObjectFromPoolC(GameObject obj, Transform parent)
    {
        string key = obj.name;

        if (!allPools.ContainsKey(key))
        {
            var newObject = Instantiate(obj, parent);
            var newPool = new List<GameObject>();
            newPool.Add(newObject);
            allPools.Add(key, newPool);
            return newObject;
        }

        var pool = allPools[key];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                pool[i].transform.rotation = Quaternion.identity;
                pool[i].transform.SetParent(parent);
                return pool[i];
            }
        }
        var addObject = Instantiate(obj, parent);
        pool.Add(addObject);
        return addObject;
    }
    public GameObject GetObjectFromPool(GameObject obj, Vector3 pos)
    {
        string key = obj.name;

        if (!allPools.ContainsKey(key))
        {
            var newObject = Instantiate(obj, pos, Quaternion.identity);
            var newPool = new List<GameObject>();
            newPool.Add(newObject);
            allPools.Add(key, newPool);
            return newObject;
        }

        var pool = allPools[key];
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                pool[i].transform.rotation = Quaternion.identity;
                pool[i].transform.position = pos;
                return pool[i];
            }
        }
        var addObject = Instantiate(obj, pos, Quaternion.identity);
        pool.Add(addObject);
        return addObject;
    }



    public T GetObjectFromPool<T>(T prefab, Vector3 position) where T : Component
    {
        string key = prefab.name;

        if (!allPools.ContainsKey(key))
        {
            var newObject = Instantiate(prefab, position, Quaternion.identity);
            var newPool = new List<GameObject>();
            newPool.Add(newObject.gameObject);
            allPools.Add(key, newPool);
            return newObject;
        }
        var pool = allPools[key];
        foreach (var obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.transform.position = position;
                obj.gameObject.SetActive(true);
                obj.transform.rotation = Quaternion.identity;
                return obj.GetComponent<T>();
            }
        }
        var newObj = Instantiate(prefab, position, Quaternion.identity);
        pool.Add(newObj.gameObject);
        return newObj;
    }

    public T GetObjectFromPool<T>(T prefab, Transform parent) where T : Component
    {
        string key = prefab.name;

        if (!allPools.ContainsKey(key))
        {
            var newObject = Instantiate(prefab, parent);
            newObject.transform.position = parent.position;
            var newPool = new List<GameObject>();
            newPool.Add(newObject.gameObject);
            allPools.Add(key, newPool);
            return newObject;
        }
        var pool = allPools[key];
        foreach (var obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.transform.SetParent(parent);
                obj.transform.position = parent.position;
                obj.gameObject.SetActive(true);
                obj.transform.rotation = Quaternion.identity;
                return obj.GetComponent<T>();
            }
        }
        var newObj = Instantiate(prefab, parent);
        newObj.transform.position = parent.position;
        pool.Add(newObj.gameObject);
        return newObj;
    }
}
