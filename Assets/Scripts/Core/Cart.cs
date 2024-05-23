using UnityEngine;
using Barista.Food;
using Barista.Factory;
using System.Collections.Generic;
using Barista.Menu;
using MyUtils;
using Barista.UI;

namespace Barista.Core
{
    public class Cart : Singleton<Cart>
    {
        [SerializeField] private GameObject[] m_pickedItems = new GameObject[3];
        [SerializeField] private Transform[] m_placements = new Transform[3];
        [SerializeField] private List<FoodType> m_pickedFoodTypes = new List<FoodType>();


        private int m_currentIndex = 0;
        private int m_count = 0;

        private bool CartFull()
        {
            return m_count >= 3;
        }

        //TODO: DELETE TEST
        private void Start()
        {
            MenuFactory.Instance.GenerateNewRecipe();
        }


        public async void PopulateCart(FoodType food)
        {
            if (CartFull()) { return; }

            m_pickedFoodTypes.Add(food);

            //load the picked object GRAPHICS and put it in the cart
            GameObject inst = await ProductFactory.Instance.LoadFood(food);
            m_pickedItems[m_currentIndex] = inst;
            inst.transform.parent = m_placements[m_currentIndex].transform;
            inst.transform.position = m_placements[m_currentIndex].transform.position;

            m_count++;
            m_currentIndex++;

            if(m_currentIndex == m_pickedItems.Length)
            {
                m_currentIndex = m_pickedItems.Length - 1;
            }
        }

        public void SubmitCartOrder()
        {
            if(m_count == 0) { return; }

            //check if the order is correctly done
            if (IsCorrectRecipe())
            {
                //Correct
                print("yes");
                MenuFactory.Instance.GenerateNewRecipe();
            }
            else
            {
                //Test
                ErrorSystem.Instance.DisplayError(ErrorType.WrongOrder);
                //Wrong
                print("no");
                MenuFactory.Instance.GenerateNewRecipe();
            }

            //clear the cart
            foreach (var ele in m_pickedItems)
            {
                if(ele == null) { continue; }
                ProductFactory.Instance.Release(ele);
            }

            //clear the cart from the food types
            m_pickedFoodTypes.Clear();

            m_count = 0;
            m_currentIndex = 0;
        }

        public bool IsCorrectRecipe()
        {
            var recipe = MenuFactory.Instance.CurrentRecipe;

            for (int i = 0; i < m_pickedFoodTypes.Count; i++)
            {
                FoodType key = m_pickedFoodTypes[i];
                if (recipe.ContainsKey(key))
                {
                    MenuFactory.Instance.RemoveItemsFromRecipe(key); 
                }
                else
                {
                    //no key like that (too many of these item on the cart)
                    return false;
                }
            }
           
            return recipe.Count == 0;
        }
    }
}
