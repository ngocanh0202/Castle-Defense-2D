using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common2D
{   
    public class SaveSystem
    {
        public static void SaveData<T>(string key, T data)
        {
            string jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

        public static T LoadData<T>(string key) where T : new()
        {
            if (PlayerPrefs.HasKey(key))
            {
                string jsonData = PlayerPrefs.GetString(key);
                return JsonUtility.FromJson<T>(jsonData);
            }
            return new T();
        }

        public static void SaveArrayData<T>(string key, List<T> data)
        {
            T[] arrayWrapper = data.ToArray();
            string playerToJson = JsonHelper.ToJson<T>(arrayWrapper, true);
            PlayerPrefs.SetString(key, playerToJson);
            PlayerPrefs.Save();
        }

        public static List<T> LoadArrayData<T>(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                string jsonData = PlayerPrefs.GetString(key);
                if(string.IsNullOrEmpty(jsonData))
                {
                    return new List<T>();
                }
                List<T> wrapper = JsonHelper.FromJson<T>(jsonData).ToList();
                return wrapper;
            }

            return new List<T>();
        }

        public static void DeleteData(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
            }
        }

        public static void DeleteAllData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
