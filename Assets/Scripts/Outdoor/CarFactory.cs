using MyUtils;
using System;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Barista
{
    public class CarFactory : Singleton<CarFactory>
    {
        [SerializeField] private AddressableAssetGroup m_assetGroup;
        [SerializeField] private List<AddressableAssetEntry> m_listOfCarsObject = new List<AddressableAssetEntry>();

        private AsyncOperationHandle<GameObject> m_asyncOperation = new AsyncOperationHandle<GameObject>();

        public event Action OnCarsLoaded;

        private void Start()
        {
            LoadAllCarAssets();
        }
        private void LoadAllCarAssets()
        {
            m_assetGroup.GatherAllAssets(m_listOfCarsObject, true, false, false);
            OnCarsLoaded?.Invoke();
        }

        public void SpawnNewCar(Action<GameObject> callback, Transform Parent)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_listOfCarsObject.Count-1);
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(m_listOfCarsObject[randomIndex].address, Parent);
                
            handle.Completed += (operation) =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(handle.Result);
                }
                else if (operation.Status == AsyncOperationStatus.Failed)
                {
                    Debug.Log("ERROR operation.Status == AsyncOperationStatus.Failed");
                }
            };
        }

        public void CleanGameObject(GameObject obj)
        {
            Addressables.ReleaseInstance(obj);
        }
    }
}
