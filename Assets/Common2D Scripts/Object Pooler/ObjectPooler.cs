using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolerEventArgs
{
    public string key;
}

public static class ObjectPooler
{
    private static Dictionary<string, List<Component>> objectEnable = new Dictionary<string, List<Component>>();
    private static Dictionary<string, Queue<Component>> objectDisable = new Dictionary<string, Queue<Component>>();

    public static bool IsObjectPoolerExist(string key)
    {
        return objectEnable.ContainsKey(key) && objectDisable.ContainsKey(key);
    }

    public static void InitObjectPooler<T>(string key, int size, T prefab, Action<T> onCreateObj) where T : Component, IPooler
    {
        if (!objectDisable.ContainsKey(key))
        {
            objectDisable.Add(key, new Queue<Component>());
            objectEnable.Add(key, new List<Component>());
        }
        // Debug.Log($"Init Object Pooler: {key}, Size: {size}");
        // Debug.Log("Disable count: " + objectDisable[key].Count);


        for (int i = 0; i < size; i++)
        {
            T newPrefab = MonoBehaviour.Instantiate(prefab);
            newPrefab.gameObject.SetActive(false);
            newPrefab.OnSetInactive += OnObjectReturn;
            objectDisable[key].Enqueue(newPrefab);
            onCreateObj?.Invoke(newPrefab);
        }
    }

    public static T GetObject<T>(string key, bool isActiveNow = true) where T : Component, IPooler
    {
        if (!objectEnable.ContainsKey(key) || !objectDisable.ContainsKey(key))
        {
            return default(T);
        }

        T newPrefab;
        if (objectDisable[key].Count == 0)
        {
            T objEnableFirst = (T)objectEnable[key].First();
            newPrefab = MonoBehaviour.Instantiate(objEnableFirst);
            newPrefab.OnSetInactive += OnObjectReturn;
            Transform transformParent = objEnableFirst.transform.parent;
            if (transformParent != null)
            {
                newPrefab.transform.SetParent(transformParent);
            }
        }
        else
        {
            newPrefab = (T)objectDisable[key].Dequeue();
        }
        
        if (isActiveNow)
        {
            newPrefab.gameObject.SetActive(true);
        }
        objectEnable[key].Add(newPrefab);

        return newPrefab;
    }

    private static void OnObjectReturn(object obj, PoolerEventArgs poolerSender)
    {
        string key = poolerSender.key;
        if (!objectEnable.ContainsKey(key) || !objectDisable.ContainsKey(key))
        {
            return;
        }

        // ReturnObj.gameObject.SetActive(false);
        objectDisable[key].Enqueue(obj as Component);
        objectEnable[key].Remove(obj as Component);
    }
}
