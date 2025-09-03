using System.Collections.Generic;
using Common2D;
using Common2D.Inventory;
using UnityEngine;


public class ItemDropSystem : Singleton<ItemDropSystem>
{
    ItemDatabase itemDatabase;
    List<Item> itemsToDrop = new List<Item>();
    float rateToDrop = 1f;
    void Start()
    {
        ItemDrop itemDrop = ResourcesManager.GetItemDrop().GetComponent<ItemDrop>();
        itemDatabase = ResourcesManager.GetItemDatabase();
        itemsToDrop = itemDatabase.GetItemsByBullet();
        ObjectPooler.Instance.InitObjectPooler<ItemDrop>(KeyOfObjPooler.ItemDrop.ToString(), 4, itemDrop);
    }

    public void OnEnemyDie(Transform enemyTransform)
    {
        if (Random.value < rateToDrop)
        {
            ItemDrop itemDrop = ObjectPooler.Instance.GetObject<ItemDrop>(KeyOfObjPooler.ItemDrop.ToString(), false);
            if (itemDrop != null)
            {
                Item randomItem = itemsToDrop[Random.Range(0, itemsToDrop.Count)];
                int quantityRandom = Random.Range(1, 5);
                itemDrop.item = randomItem;
                itemDrop.quantity = quantityRandom;
                itemDrop.transform.position = enemyTransform.transform.position;
                itemDrop.gameObject.SetActive(true);
            }
        }
    }
}
