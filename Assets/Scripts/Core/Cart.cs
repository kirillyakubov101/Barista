using UnityEngine;
using Barista.Food;
using Barista.Factory;
using System.Collections.Generic;
using Barista.Menu;
using MyUtils;
using Barista.UI;
using Barista.Order;
using Barista.Sounds;
using System.Collections;

namespace Barista.Core
{
    public class Cart : Singleton<Cart>
    {
        [SerializeField] private GameObject[] m_pickedItems = new GameObject[3];
        [SerializeField] private Transform[] m_placements = new Transform[3];
        [SerializeField] private List<FoodType> m_pickedFoodTypes = new List<FoodType>();

        private int m_currentPlacementIndex = 0;
        private int m_foodItemsInCart = 0;

        public void PopulateCart(FoodType food)
        {
            if (CartFull())
            {
                ErrorSystem.Instance.DisplayError(ErrorType.CartFull);
                return;
            }

            SoundHandler.Instance.PlayTakeitemSound(true);
            m_pickedFoodTypes.Add(food);

            // Start the coroutine to load the picked object GRAPHICS and put it in the cart
            StartCoroutine(LoadAndPlaceFood(food));
        }

        private IEnumerator LoadAndPlaceFood(FoodType food)
        {
            GameObject inst = null;
            // Load the food using coroutine

            yield return StartCoroutine(ProductFactory.Instance.LoadFoodNew(food, result => inst = result));
          
            if (inst != null)
            {
                m_pickedItems[m_currentPlacementIndex] = inst;
                inst.transform.parent = m_placements[m_currentPlacementIndex].transform;
                inst.transform.position = m_placements[m_currentPlacementIndex].transform.position;

                m_foodItemsInCart++;
                m_currentPlacementIndex++;

                if (m_currentPlacementIndex == m_pickedItems.Length)
                {
                    m_currentPlacementIndex = m_pickedItems.Length - 1;
                }
            }
        }

        public async void PopulateCartOld(FoodType food)
        {
            if (CartFull()) { ErrorSystem.Instance.DisplayError(ErrorType.CartFull); return; }

            SoundHandler.Instance.PlayTakeitemSound(true);
            m_pickedFoodTypes.Add(food);

            //load the picked object GRAPHICS and put it in the cart
            GameObject inst = await ProductFactory.Instance.LoadFood(food);
            m_pickedItems[m_currentPlacementIndex] = inst;
            inst.transform.parent = m_placements[m_currentPlacementIndex].transform;
            inst.transform.position = m_placements[m_currentPlacementIndex].transform.position;

            m_foodItemsInCart++;
            m_currentPlacementIndex++;

            if(m_currentPlacementIndex == m_pickedItems.Length)
            {
                m_currentPlacementIndex = m_pickedItems.Length - 1;
            }
        }

        public void SubmitCartOrder()
        {
            if(m_foodItemsInCart == 0 || MenuFactory.Instance.CurrentRecipe.Count == 0 || OrderHandler.Instance.m_currentClient == null) { return; }

            OrderHandler.Instance.SubmitOrderToClient(m_pickedFoodTypes);

            ClearTheCart();
        }

        private bool CartFull()
        {
            return m_foodItemsInCart >= 3;
        }

        private void ClearTheCart()
        {
            //clear the cart
            foreach (var ele in m_pickedItems)
            {
                if (ele == null) { continue; }
                ProductFactory.Instance.Release(ele);
            }

            //clear the cart from the food types
            m_pickedFoodTypes.Clear();

            m_foodItemsInCart = 0;
            m_currentPlacementIndex = 0;
        }
    }
}
