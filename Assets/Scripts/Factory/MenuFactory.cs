using MyUtils;
using System.Collections.Generic;
using Barista.Food;
using System;
using Barista.Core;
using Random = UnityEngine.Random;
using UnityEngine;

namespace Barista.Menu
{
    [Serializable]
    public struct FoodImage
    {
        public Sprite m_sprite;
        public FoodType m_foodType;
    }

    public class MenuFactory : Singleton<MenuFactory>
    {
        [field: SerializeField] public FoodImage[] FoodImages;

        public static event Action OnRecipeGenerated;

        private readonly FoodType[] allFoods = (FoodType[])Enum.GetValues(typeof(FoodType));

        private Dictionary<FoodType, int> m_CurrentRecipe = new Dictionary<FoodType, int>();

        public Dictionary<FoodType, int> CurrentRecipe { get => m_CurrentRecipe;  }


        public void GenerateNewRecipe()
        {
            m_CurrentRecipe.Clear();
            int amountOfItems = Random.Range(1, 4);
            

            for (int i = 0; i < amountOfItems; i++)
            {
                //Solid
                int randomIndex = Random.Range(0, allFoods.Length);
                if (!m_CurrentRecipe.ContainsKey(allFoods[randomIndex]))
                {
                    m_CurrentRecipe.Add(allFoods[randomIndex], 1);
                }
                else
                {
                    m_CurrentRecipe[allFoods[randomIndex]]++;
                }
                
            }

            OnRecipeGenerated?.Invoke();
        }

        private void PrintRecipe()
        {
            foreach(var ele in m_CurrentRecipe)
            {
                print(ele);
            }
        }
    }
}
