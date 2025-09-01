using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContainPools
{
    public Component prefab;
    public Queue<Component> objectDisable;
}
public static class ObjectPooler
{
    private static Dictionary<string, List<Component>> objectEnable = new Dictionary<string, List<Component>>();
    private static Dictionary<string, ContainPools> objectDisable = new Dictionary<string, ContainPools>();

    public static bool IsObjectPoolerExist(string key)
    {
        return objectEnable.ContainsKey(key) && objectDisable.ContainsKey(key);
    }

    public static void InitObjectPooler<T>(string key, int size, T prefab, Action<T> onCreateObj) where T : Component, IPoolObject
    {
        if (!objectDisable.ContainsKey(key))
        {
            objectDisable.Add(key, new ContainPools { prefab = prefab, objectDisable = new Queue<Component>() });
            objectEnable.Add(key, new List<Component>());
        }
        else
        {
            Debug.LogWarning($"Object Pooler with key '{key}' already exists.");
            return;
        }

        for (int i = 0; i < size; i++)
        {
            T newPrefab = MonoBehaviour.Instantiate(prefab);
            newPrefab.gameObject.SetActive(false);
            if (newPrefab.OnSetInactive == null)
                newPrefab.OnSetInactive = OnObjectReturn;
            objectDisable[key].objectDisable.Enqueue(newPrefab);
            onCreateObj?.Invoke(newPrefab);
        }
    }

    public static T GetObject<T>(string key, bool isActiveNow = true) where T : Component, IPoolObject
    {
        if (!objectEnable.ContainsKey(key) || !objectDisable.ContainsKey(key)) return default(T);

        T newPrefab;
        if (objectDisable[key].objectDisable.Count == 0)
        {
            T objEnableFirst = objectDisable[key].prefab as T;
            newPrefab = MonoBehaviour.Instantiate(objEnableFirst);
            if (newPrefab.OnSetInactive == null)
                newPrefab.OnSetInactive = OnObjectReturn;       
            Transform transformParent = objectEnable[key].FirstOrDefault()?.transform.parent;
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

    private static void OnObjectReturn(object obj, string key)
    {
        string _key = key;
        if (!objectEnable.ContainsKey(_key) || !objectDisable.ContainsKey(_key)) return;

        objectDisable[key].objectDisable.Enqueue(obj as Component);
        objectEnable[key].Remove(obj as Component);
    }
}
