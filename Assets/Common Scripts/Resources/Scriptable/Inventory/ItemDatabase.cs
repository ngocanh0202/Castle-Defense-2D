using System.Collections.Generic;
using Common2D.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] public List<Item> items = new List<Item>();
    public Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();

    private void OnEnable()
    {
        // Build the dictionary
        itemDictionary.Clear();
        foreach (var item in items)
        {
            itemDictionary[item.itemID] = item;
        }
    }

    public Item GetItemByID(string id)
    {
        if (itemDictionary.TryGetValue(id, out Item item))
        {
            return item;
        }

        Debug.LogWarning($"Item with ID {id} not found in database!");
        return null;
    }

    public List<Item> GetItemsByBullet()
    {
        List<Item> bulletItems = new List<Item>();
        foreach (var item in items)
        {
            if (item.itemType == Item.ItemType.Bullet)
            {
                bulletItems.Add(item);
            }
        }
        return bulletItems;
    }

    [ContextMenu("Load data from CSV")]
    void ExecuteAction()
    {
        items.Clear();
        items = CSVManager.LoadDatasFromCSV<Item>("Item Database",(datas) =>
        {
            Item newItem = new Item();

            for (int i = 0; i < datas.Length; i++)
            {
                newItem.itemID = datas[0];
                newItem.itemName = datas[1];
                newItem.description = datas[2];
                newItem.icon = Resources.Load<Sprite>(datas[3]);
                newItem.isStackable = bool.Parse(datas[4]);
                newItem.maxStackSize = newItem.isStackable == false ?  0 : int.Parse(datas[5]);
                newItem.itemType = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), datas[6]);
            }
            return newItem;
        });
    }
}
