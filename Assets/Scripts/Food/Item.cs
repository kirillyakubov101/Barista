using Barista.Core;
using UnityEngine;

namespace Barista.Food
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] protected Outline m_outline;
        [SerializeField] private FoodType m_foodType;
       
        protected ItemState m_state = ItemState.NORMAL;

        public FoodType GetFoodType()
        {
            return m_foodType;
        }
    }
}
