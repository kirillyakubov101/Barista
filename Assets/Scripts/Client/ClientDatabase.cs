using MyUtils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Barista.Clients
{


    public class ClientDatabase : Singleton<ClientDatabase>
    {
        [SerializeField] private AssetReferenceGameObject[] ListOfClientPrefabs;


        private List<int> recentIndices = new List<int>(); // Tracks recently spawned indices
        


        public void SpawnNewClient(Action<GameObject> callback,Vector3 pos, Quaternion rot)
        {
            int randomIndex = GetWeightedRandomIndex();
            AsyncOperationHandle<GameObject> handle = ListOfClientPrefabs[randomIndex].InstantiateAsync(pos,rot);

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

            UpdateRecentIndices(randomIndex);

        }

        private int GetWeightedRandomIndex()
        {
            List<int> validIndices = new List<int>();
            for (int i = 0; i < ListOfClientPrefabs.Length; i++)
            {
                // Add index multiple times if not recently spawned, to increase probability
                if (!recentIndices.Contains(i))
                {

                    validIndices.Add(i);
                    validIndices.Add(i);
                    validIndices.Add(i); // Increase weight for non-recent indices
                }
                else
                {
                    validIndices.Add(i); // Still allow, but with less weight
                }
            }

            // Randomly select from the weighted list
            int randomWeightedIndex = UnityEngine.Random.Range(0, validIndices.Count);
            return validIndices[randomWeightedIndex];
        }

        private void UpdateRecentIndices(int index)
        {
            if (recentIndices.Contains(index)) return;

            // Add to the recent indices list
            recentIndices.Add(index);

            // Keep the list size within the history limit
            if (recentIndices.Count > 3)
            {
                recentIndices.RemoveAt(0);
            }
        }

        public void Release(GameObject obj)
        {
            Addressables.ReleaseInstance(obj);
        }
    }

  
}
