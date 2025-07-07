using System;
using System.Collections.Generic;
using System.Linq;
using Common2D.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace Common2D.Inventory
{
    [Serializable]
    public class InventoryItemBrief
    {
        public string itemID;
        public int amount;
        public int slotIndex;
    }
    public class InventoryManager : Singleton<InventoryManager>
    {
        public int inventorySize;
        public List<InventorySlot> inventorySlots = new List<InventorySlot>();

        public UnityEvent OnInventoryChanged;

        protected override void Awake()
        {
            base.Awake();
            inventorySize = 50;
            List<InventoryItemBrief> inventoryItemBriefs =
                SaveSystem.LoadArrayData<InventoryItemBrief>(StringDefault.InventoryData.ToString());
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryItemBrief itemBrief = inventoryItemBriefs.FirstOrDefault(x => x.slotIndex == i);
                if (itemBrief != null)
                {
                    Item item = ResourcesManager.GetItemDatabase().GetItemByID(itemBrief.itemID);
                    inventorySlots.Add(new InventorySlot(item, itemBrief.amount));
                }
                else
                    inventorySlots.Add(new InventorySlot
                    {
                        item = null,
                        amount = 0
                    });
            }
        }

        public void SaveInventoryInLocalStorage()
        {
            List<InventoryItemBrief> InventoryItemBriefs = new List<InventoryItemBrief>();
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if(inventorySlots[i].item == null)
                {
                    continue;
                }
                InventoryItemBriefs.Add(new InventoryItemBrief{ itemID = inventorySlots[i].item.itemID, amount = inventorySlots[i].amount, slotIndex = i });
            }

            if (InventoryItemBriefs.Count != 0)
            {
                SaveSystem.SaveArrayData<InventoryItemBrief>(StringDefault.InventoryData.ToString(), InventoryItemBriefs);
            }
        }

        public bool AddItem(Item item, int amount)
        {
            if (item.isStackable)
            {
                int existingItemIndex = inventorySlots.FindIndex(
                        slot => slot.item != null && slot.item.itemID == item.itemID && slot.amount < item.maxStackSize);
                if (existingItemIndex != -1)
                {
                    int amountToAdd = Mathf.Min(amount, item.maxStackSize - inventorySlots[existingItemIndex].amount);
                    inventorySlots[existingItemIndex].AddToStack(amountToAdd);

                    if (amountToAdd < amount)
                    {
                        return AddItem(item, amount - amountToAdd);
                    }

                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
            int emptySlotIndex = inventorySlots.FindIndex(slot => slot.item == null);
            if (emptySlotIndex != -1)
            {

                if (item.isStackable)
                {
                    int amountToAdd = Mathf.Min(amount, item.maxStackSize);
                    inventorySlots[emptySlotIndex] = new InventorySlot(item, amountToAdd);

                    if (amountToAdd < amount)
                    {
                        return AddItem(item, amount - amountToAdd);
                    }
                }
                else
                {
                    inventorySlots[emptySlotIndex] = new InventorySlot(item, 1);

                    if (amount > 1)
                    {
                        return AddItem(item, amount - 1);
                    }
                }

                OnInventoryChanged?.Invoke();
                return true;
            }

            // NotificationSystem.Instance.ShowNotification(
            //     "Inventory Full", NotificationType.Warning);
            return false;
        }

        public void RemoveItem(string itemID, int amount)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].item != null && inventorySlots[i].item.itemID == itemID)
                {
                    int amountToRemove = Mathf.Min(amount, inventorySlots[i].amount);
                    inventorySlots[i].RemoveFromStack(amountToRemove);
                    amount -= amountToRemove;

                    if (amount <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return;
                    }
                }
            }

            OnInventoryChanged?.Invoke();
        }

        public int GetItemCount(string itemID)
        {
            int count = 0;
            foreach (var slot in inventorySlots)
            {
                if (slot.item != null && slot.item.itemID == itemID)
                {
                    count += slot.amount;
                }
            }
            return count;
        }

        public void SwapItems(int slotA, int slotB)
        {
            if (slotA >= 0 && slotA < inventorySlots.Count && slotB >= 0 && slotB < inventorySlots.Count && slotA != slotB)
            {
                InventorySlot temp = new InventorySlot(inventorySlots[slotA].item, inventorySlots[slotA].amount);
                inventorySlots[slotA] = new InventorySlot(inventorySlots[slotB].item, inventorySlots[slotB].amount);
                inventorySlots[slotB] = temp;

                OnInventoryChanged?.Invoke();
            }
        }
    }
}
