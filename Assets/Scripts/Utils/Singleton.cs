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

                    //if (foundInstances.Length == 0)
                    //{
                    //    GameObject singletonObject = new GameObject(typeof(T).Name);
                    //    instance = singletonObject.AddComponent<T>();
                    //}
                    if (foundInstances.Length == 0) { print("singleton was not found!"); return null; }

                    instance = foundInstances[0];

                    for (int i = 1; i < foundInstances.Length; i++)
                    {
                        Destroy(foundInstances[i]);
                    }
                }
                return instance;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            instance = null;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

    }
}
