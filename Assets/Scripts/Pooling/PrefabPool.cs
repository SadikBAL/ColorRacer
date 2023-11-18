using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//PoolingDataClass for Scriptable object.
[Serializable]
public class PrefabPoolData
{
    public GameObject Prefab;
    public int InitSize;
}
//Pooling Class
public class PrefabPool
{
    public GameObject Prefab;
    public int InitSize;
    private readonly Stack<GameObject> Instances;
    public PrefabPool(GameObject obj, int size)
    {
        Prefab = obj;
        InitSize = size;
        Instances = new Stack<GameObject>();
        for (var i = 0; i < InitSize; i++)
        {
            var Object = CreateInstance();
            Object.SetActive(false);
            Instances.Push(Object);
        }
    }
    private GameObject CreateInstance()
    {
        var Object = GameObject.Instantiate(Prefab);
        var PooledPrefab = Object.AddComponent<PooledPrefab>();
        PooledPrefab.Pool = this;
        return Object;
    }
    public void ReturnObject(GameObject ObjectRef)
    {
        var Object = ObjectRef.GetComponent<PooledPrefab>();
        Assert.IsNotNull(Object);
        Assert.IsTrue(Object.Pool == this);
        ObjectRef.SetActive(false);
        if (!Instances.Contains(ObjectRef))
        {
            Instances.Push(ObjectRef);
        }
    }
    public GameObject GetObject()
    {
        var Object = Instances.Count > 0 ? Instances.Pop() : CreateInstance();
        Object.SetActive(true);
        //Object.transform.localScale = Vector3.one;
        return Object;
    }
}
//This class save referance for return object to pool.
public class PooledPrefab : MonoBehaviour
{
    public PrefabPool Pool;
}
