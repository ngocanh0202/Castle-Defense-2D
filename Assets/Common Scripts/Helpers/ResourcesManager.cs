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

        public static Sprite GetSpriteWithPath(string path)
        {
            Sprite sprite = Resources.Load<Sprite>(path);
            if (sprite != null)
            {
                return sprite;
            }
            else
            {
                Debug.LogError("Sprite not found in Resources folder.");
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
            GunScriptableObject prefab = Resources.Load<GunScriptableObject>($"Scriptable/Bullets/{gunName}");
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
            string path = $"Prefabs/Bullets/{bulletName}";
            GameObject prefab = Resources.Load<GameObject>(path);
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
        public static GameObject GetItemDrop()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/ItemDrops/ItemDrop");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab ItemDrop not found in Resources folder.");
                return null;
            }
        }
        public static GameObject GetEnemyPrefab()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/Enemies/Enemy");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab Enemy not found in Resources folder.");
                return null;
            }
        }
        public static TowerDefenseBehaviour GetEnemyTowerPrefab()
        {
            TowerDefenseBehaviour prefab = Resources.Load<TowerDefenseBehaviour>("Prefabs/Towers/EnemyTower");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab Tower not found in Resources folder.");
                return null;
            }
        }
        public static TowerDefenseBehaviour GetPlayerTowerPrefab()
        {
            TowerDefenseBehaviour prefab = Resources.Load<TowerDefenseBehaviour>("Prefabs/Towers/PlayerTower");
            if (prefab != null)
            {
                return prefab;
            }
            else
            {
                Debug.LogError("Prefab Tower not found in Resources folder.");
                return null;
            }
        }

        public static EnemyStatScriptable GetEnemyStatScriptable()
        {
            EnemyStatScriptable scriptable = Resources.Load<EnemyStatScriptable>("Scriptable/Stats/EnemyStat");
            if (scriptable != null)
            {
                return scriptable;
            }
            else
            {
                Debug.LogError("EnemyStatScriptable not found in Resources folder.");
                return null;
            }
        }

        public static ObjectStatScriptable GetObjectStatScriptable()
        {
            ObjectStatScriptable scriptable = Resources.Load<ObjectStatScriptable>("Scriptable/Stats/ObjectStat");
            if (scriptable != null)
            {
                return scriptable;
            }
            else
            {
                Debug.LogError("ObjectStatScriptable not found in Resources folder.");
                return null;
            }
        }

        public static PlayerStatScriptable GetPlayerStatScriptable()
        {
            PlayerStatScriptable scriptable = Resources.Load<PlayerStatScriptable>("Scriptable/Stats/PlayerStat");
            if (scriptable != null)
            {
                return scriptable;
            }
            else
            {
                Debug.LogError("PlayerStatScriptable not found in Resources folder.");
                return null;
            }
        }

        public static TowerStatScriptable GetTowerStatScriptable(string towerName)
        {
            TowerStatScriptable scriptable = Resources.Load<TowerStatScriptable>($"Scriptable/Stats/{towerName}");
            if (scriptable != null)
            {
                return scriptable;
            }
            else
            {
                Debug.LogError("TowerStatScriptable not found in Resources folder.");
                return null;
            }
        }

    }
}

