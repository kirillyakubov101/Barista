using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Barista.Food;
using System.Threading.Tasks;
using MyUtils;
using System.Collections;
using System;

namespace Barista.Factory
{
    public class ProductFactory : Singleton<ProductFactory>
    {
        [SerializeField] private AssetReferenceGameObject m_sandwich;
        [SerializeField] private AssetReferenceGameObject m_coffee;
        [SerializeField] private AssetReferenceGameObject m_doughnutChocolate;
        [SerializeField] private AssetReferenceGameObject m_doughnutVanil;
        [SerializeField] private AssetReferenceGameObject m_doughnutCherry;
        [SerializeField] private AssetReferenceGameObject m_orangeJuice;
        [SerializeField] private AssetReferenceGameObject m_Lemonade;
        [SerializeField] private AssetReferenceGameObject m_Croissant;
        [SerializeField] private AssetReferenceGameObject m_Toast;
        [SerializeField] private AssetReferenceGameObject m_Capcake;
        [SerializeField] private AssetReferenceGameObject m_Cookie;


        private Dictionary<FoodType, AssetReferenceGameObject> m_foods = new Dictionary<FoodType, AssetReferenceGameObject>();
        private void Start()
        {
            m_foods.Add(FoodType.Sandwich, m_sandwich);
            m_foods.Add(FoodType.Coffee, m_coffee);
            m_foods.Add(FoodType.Doughnut_Chocolate, m_doughnutChocolate);
            m_foods.Add(FoodType.Doughnut_Vanile, m_doughnutVanil);
            m_foods.Add(FoodType.Doughnut_Cherry, m_doughnutCherry);
            m_foods.Add(FoodType.OrangeJuice, m_orangeJuice);
            m_foods.Add(FoodType.Lemonade, m_Lemonade);
            m_foods.Add(FoodType.Croissant, m_Croissant);
            m_foods.Add(FoodType.Toast, m_Toast);
            m_foods.Add(FoodType.Capcake, m_Capcake);
            m_foods.Add(FoodType.Cookie, m_Cookie);
        }

        public async Task<GameObject> LoadFood(FoodType food)
        {
            GameObject returnInst = null;
            AsyncOperationHandle<GameObject> operation;


            if (!m_foods.ContainsKey(food)) { return null; }

            operation = Addressables.InstantiateAsync(m_foods[food]);
            
            await operation.Task;

            if(operation.Task.Status == TaskStatus.RanToCompletion)
            {
                returnInst = operation.Result;
            }
            return returnInst;
        }

        public IEnumerator LoadFoodNew(FoodType food, Action<GameObject> callback)
        {
            GameObject returnInst = null;
            AsyncOperationHandle<GameObject> operation;

            if (!m_foods.ContainsKey(food))
            {
                callback?.Invoke(null);
                yield break;
            }

            operation = Addressables.InstantiateAsync(m_foods[food]);
            yield return operation;

            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                returnInst = operation.Result;
            }

            callback?.Invoke(returnInst);
        }

        public void Release(GameObject obj)
        {
            Addressables.ReleaseInstance(obj);
        }
    }
}
