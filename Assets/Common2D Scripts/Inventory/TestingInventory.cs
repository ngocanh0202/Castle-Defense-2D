using System.Collections.Generic;
using Common2D.Inventory;
using Common2D;
using UnityEngine;

public class TestingInventory : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Item itemSword = ResourcesManager.GetItemDatabase().GetItemByID("1");
            InventoryManager.Instance.AddItem(itemSword, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Item itemBow = ResourcesManager.GetItemDatabase().GetItemByID("2");
            InventoryManager.Instance.AddItem(itemBow, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Item itemArrow = ResourcesManager.GetItemDatabase().GetItemByID("3");
            InventoryManager.Instance.AddItem(itemArrow, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InventoryManager.Instance.SaveInventoryInLocalStorage();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Delete data");
            SaveSystem.DeleteData(StringDefault.InventoryData.ToString());
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            List<InventoryItemBrief> inventorySlot =
                            SaveSystem.LoadArrayData<InventoryItemBrief>(StringDefault.InventoryData.ToString());
            Debug.Log(inventorySlot.Count);
        }
    }
}
