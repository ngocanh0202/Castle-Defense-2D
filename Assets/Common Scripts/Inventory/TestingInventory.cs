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
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Item itemSword = ResourcesManager.GetItemDatabase().GetItemByID("4");
            InventoryManager.Instance.AddItem(itemSword, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Item itemBow = ResourcesManager.GetItemDatabase().GetItemByID("5");
            InventoryManager.Instance.AddItem(itemBow, 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Item itemArrow = ResourcesManager.GetItemDatabase().GetItemByID("6");
            InventoryManager.Instance.AddItem(itemArrow, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InventoryManager.Instance.SaveInventoryInLocalStorage();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SaveSystem.DeleteData(StringDefault.InventoryData.ToString());
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            List<InventoryItemBrief> inventorySlot =
                            SaveSystem.LoadArrayData<InventoryItemBrief>(StringDefault.InventoryData.ToString());
        }
    }
}
