using UnityEngine;

namespace Common2D.Inventory
{
    [System.Serializable]
    public class Item
    {
        public string itemID;
        public string itemName;
        public string description;
        public Sprite icon;
        public bool isStackable;
        public int maxStackSize = 99;
        public ItemType itemType;

        public enum ItemType
        {
            Weapon,
            Armor,
            Consumable,
            Material,
            Quest
        }

        public override string ToString()
        {
            return $"Item ID: {itemID}, Name: {itemName}, Description: {description}, Is Stackable: {isStackable}, Max Stack Size: {maxStackSize}";
        }
    }

}
