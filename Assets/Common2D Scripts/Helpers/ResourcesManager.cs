using UnityEngine;

namespace Common2D
{
    public class ResourcesManager
    {
        public static ItemDatabase GetItemDatabase()
        {
            ItemDatabase itemDatabase = Resources.Load<ItemDatabase>("Scriptable/Inventory/ItemDatabase");
            if (itemDatabase != null)
            {
                return itemDatabase;
            }
            else
            {
                Debug.LogError("ItemDatabase not found in Resources folder.");
                return null;
            }
        }
        public static GameObject GetPrefabInventorySlot()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Inventory/InventorySlot");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab InventorySlot not found in Resources folder.");
                return null;
            }
        }
        public static Sprite GetSpriteDefault()
        {
            Sprite spriteDefault = Resources.Load<Sprite>("Images/Default");
            if (spriteDefault != null)
            {
                return spriteDefault;
            }
            else
            {
                Debug.LogError("SpriteDefault not found in Resources folder.");
                return null;
            }
        }
        public static GameObject GetDefaultNotification()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Notification/Notification Default");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab Notification not found in Resources folder.");
                return null;
            }
        }
        public static GameObject GetDefaultConfirmModal()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Confirm Modal/Confirm Modal");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab InventorySlot not found in Resources folder.");
                return null;
            }
        }
        public static TextAsset GetItemDatabaseCSV()
        {
            TextAsset textAssetItemData = Resources.Load<TextAsset>("CSV/Item Database");
            if (textAssetItemData != null)
            {
                return textAssetItemData;
            }
            else
            {
                Debug.LogError("textAssetItemData not found in Resources folder.");
                return null;
            }
        }
        public static GameObject GetPopupTextPrefab()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Create Objects/Popup Text");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab Popup Text not found in Resources folder.");
                return null;
            }
        }

        public static GunScriptableObject GetGunScriptableObject(string gunName)
        {
            GunScriptableObject prefab = Resources.Load<GunScriptableObject>($"Gun Scriptable/{gunName}");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError($"Prefab {gunName} not found in Resources folder.");
                return null;
            }
        }

        public static GameObject GetBulletPrefab(string bulletName)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Bullets/{bulletName}");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError($"Prefab {bulletName} not found in Resources folder.");
                return null;
            }
        }
    }
}

