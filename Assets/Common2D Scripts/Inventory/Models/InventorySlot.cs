using System;
using UnityEngine;

namespace Common2D.Inventory
{
    public class InventorySlot
    {
        public Item item;
        public int amount;

        public InventorySlot() { }

        public InventorySlot(Item item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public void AddToStack(int value)
        {
            amount += value;
        }

        public void RemoveFromStack(int value)
        {
            amount -= value;
            if (amount <= 0)
            {
                ClearSlot();
            }
        }

        public void ClearSlot()
        {
            item = null;
            amount = 0;
        }
        
        public override string ToString()
        {
            return $"Item ID: {item?.itemID}, Amount: {amount}";
        }
    }

}
