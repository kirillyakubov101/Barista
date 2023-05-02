using UnityEngine;

namespace MyUtils
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var foundInstances = FindObjectsOfType<T>();

                    if (foundInstances.Length == 0)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                    }
                    else
                    {
                        instance = foundInstances[0];

                        for (int i = 1; i < foundInstances.Length; i++)
                        {
                            Destroy(foundInstances[i]);
                        }
                    }
                }
                return instance;
            }
        }

        private void OnApplicationQuit()
        {
            instance = null;
            Destroy(gameObject);
        }

    }
}
