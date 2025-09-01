using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    [SerializeField] private Transform prefab;
    [SerializeField] private Transform holder;
    [SerializeField] private List<Transform> pooledObjects; 
    [SerializeField] private List<Transform> prefabObjects; 
    void Start()
    {
        pooledObjects = new List<Transform>();
        prefabObjects = new List<Transform>();
        prefab = transform.Find("Prefab");
        holder = transform.Find("Holder");
        if (prefab == null)
        {
            Debug.LogError("Prefab not found");
        }
        else{
            int countPrefab = prefab.childCount;
            for (int i = 0; i < countPrefab; i++)
            {
                prefabObjects.Add(prefab.GetChild(i));
            }
        }

        if (holder == null)
        {
            Debug.LogError("Holder not found");
        }
        else
        {
            int countHolder = holder.childCount;
            for (int i = 0; i < countHolder; i++)
            {
                pooledObjects.Add(holder.GetChild(i));
            }
        }
    }

    public Transform SpawmerObject(string name)
    {
        Transform spawerObject = null;
        if(name == "")
        {
            spawerObject = GetRandomPrefabsObject();
        }
        else
        {
            spawerObject = GetPrefabsObjectByName(name);
            if (spawerObject == null)
            {
                spawerObject = GetRandomPrefabsObject();
            }
        }

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].name.Contains(spawerObject.name) && !pooledObjects[i].gameObject.activeInHierarchy)
            {
                pooledObjects[i].gameObject.SetActive(true);
                return pooledObjects[i];
            }
        }

        Transform newObject = Instantiate(spawerObject, holder);
        newObject.gameObject.SetActive(true);
        pooledObjects.Add(newObject);
        return newObject;
    }

    private Transform GetRandomPrefabsObject(){
        return prefabObjects[Random.Range(0, prefabObjects.Count)];
    }

    private Transform GetPrefabsObjectByName(string name)
    {
        return prefabObjects.FirstOrDefault(x => x.name == name);
    }
}