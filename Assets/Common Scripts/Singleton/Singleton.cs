using UnityEngine;

namespace Common2D
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                        GameObject gameParent = GameObject.Find("Singletons");
                        if (gameParent != null)
                        {
                            singletonObject.transform.SetParent(gameParent.transform);
                        }
                        else
                        {
                            gameParent = new GameObject("Singletons");
                            gameParent.transform.position = Vector3.zero;
                            singletonObject.transform.SetParent(gameParent.transform);
                        }
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
