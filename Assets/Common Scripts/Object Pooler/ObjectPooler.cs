using System;
using System.Collections.Generic;
using System.Linq;
using Common2D;
using UnityEngine;

public class ContainPools
{
    public Component prefab;
    public Transform holder;
    public Queue<Component> objectDisable;
}
public class ObjectPooler : Singleton<ObjectPooler>
{
    private Dictionary<string, List<Component>> objectEnable = new Dictionary<string, List<Component>>();
    private Dictionary<string, ContainPools> objectDisable = new Dictionary<string, ContainPools>();

    public bool IsObjectPoolerExist(string key)
    {
        return objectEnable.ContainsKey(key) && objectDisable.ContainsKey(key);
    }

    public void InitObjectPooler<T>(string key, int size, T prefab, Action<T> onCreateObj = null) where T : Component, IPoolObject
    {
        Transform holder;
        if (!objectDisable.ContainsKey(key))
        {
            holder = new GameObject($"ObjectPool_{key}").transform;
            holder.SetParent(this.transform);

            objectDisable.Add(key, new ContainPools { prefab = prefab, holder = holder, objectDisable = new Queue<Component>() });
            objectEnable.Add(key, new List<Component>());
        }
        else
        {
            Debug.LogWarning($"Object Pooler with key '{key}' already exists.");
            return;
        }

        for (int i = 0; i < size; i++)
        {
            T newPrefab = Instantiate(prefab);
            newPrefab.gameObject.SetActive(false);
            if (newPrefab.OnSetInactive == null)
                newPrefab.OnSetInactive = OnObjectReturn;
            objectDisable[key].objectDisable.Enqueue(newPrefab);
            newPrefab.transform.SetParent(holder);
            onCreateObj?.Invoke(newPrefab);
        }
    }

    public T GetObject<T>(string key, bool isActiveNow = true) where T : Component, IPoolObject
    {
        if (!objectEnable.ContainsKey(key) || !objectDisable.ContainsKey(key)) return default(T);

        T newPrefab;
        if (objectDisable[key].objectDisable.Count == 0)
        {
            T objEnableFirst = objectDisable[key].prefab as T;
            newPrefab = Instantiate(objEnableFirst);
            if (newPrefab.OnSetInactive == null)
                newPrefab.OnSetInactive = OnObjectReturn;
            Transform transformParent = objectDisable[key].holder;
            if (transformParent != null)
                newPrefab.transform.SetParent(transformParent);
        }
        else
            newPrefab = (T)objectDisable[key].objectDisable.Dequeue();


        if (isActiveNow)
            newPrefab.gameObject.SetActive(true);

        objectEnable[key].Add(newPrefab);

        return newPrefab;
    }

    private void OnObjectReturn(object obj, string key)
    {
        string _key = key;
        if (!objectEnable.ContainsKey(_key) || !objectDisable.ContainsKey(_key)) return;

        objectDisable[key].objectDisable.Enqueue(obj as Component);
        objectEnable[key].Remove(obj as Component);
    }
}
