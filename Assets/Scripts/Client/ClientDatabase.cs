using MyUtils;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Barista.Clients
{


    public class ClientDatabase : Singleton<ClientDatabase>
    {
        [SerializeField] private AssetReferenceGameObject Client_Kate;

        public void SpawnNewClient(Action<GameObject> callback,Vector3 pos, Quaternion rot)
        {

            AsyncOperationHandle<GameObject> handle = Client_Kate.InstantiateAsync(pos,rot);

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
      


        public void Release(GameObject obj)
        {
            Addressables.ReleaseInstance(obj);
        }
    }

  
}
