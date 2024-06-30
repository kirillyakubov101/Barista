using MyUtils;
using System.Collections.Generic;
using Barista.Food;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
using Barista.Order;

namespace Barista.Menu
{
    [Serializable]
    public struct FoodVisual
    {
        public Sprite m_sprite;
        public FoodType m_foodType;
    }


    public class MenuFactory : Singleton<MenuFactory>
    {

        [SerializeField] private List<FoodVisual> m_FoodVisuals;

        private Dictionary<FoodType, int> m_CurrentRecipe = new Dictionary<FoodType, int>();
        private Dictionary<FoodType, Sprite> m_FoodToVisualDictionary = new Dictionary<FoodType, Sprite>();
        public IReadOnlyDictionary<FoodType, int> CurrentRecipe { get => m_CurrentRecipe;  }



        public void GenerateNewRecipe()
        {
           
            m_CurrentRecipe.Clear();
            int amountOfItems = Random.Range(1, 4);
            
           

            for (int i = 0; i < amountOfItems; i++)
            {
                //Solid
                int randomIndex = Random.Range(0, m_FoodVisuals.Count);
                if (!m_CurrentRecipe.ContainsKey(m_FoodVisuals[randomIndex].m_foodType))
                {
                    m_CurrentRecipe.Add(m_FoodVisuals[randomIndex].m_foodType, 1);
                }
                else
                {
                    m_CurrentRecipe[m_FoodVisuals[randomIndex].m_foodType]++;
                }
                
            }
            PrintRecipe();
        }

        public void RemoveItemsFromRecipe(FoodType key)
        {
            m_CurrentRecipe[key]--;
            if (m_CurrentRecipe[key] <= 0)
            {
                m_CurrentRecipe.Remove(key);
            }
        }

        public void PrintRecipe()
        {
            foreach(var ele in m_CurrentRecipe)
            {
                print(ele);
            }

        }

        public Sprite GetSpriteFromRecipe(FoodType food)
        {
            if(m_FoodToVisualDictionary.Count == 0) { InitFoodToSpriteEntry(); }
            if(m_FoodToVisualDictionary.ContainsKey(food))
            {
                return m_FoodToVisualDictionary[food];
            }
            else
            {
                print("Wrong m_FoodToVisualDictionary request");
                return null;
            }
        }

        private void InitFoodToSpriteEntry()
        {
            foreach (FoodVisual foodVisual in m_FoodVisuals)
            {
                m_FoodToVisualDictionary.Add(foodVisual.m_foodType, foodVisual.m_sprite);
            }
        }
    }
}
