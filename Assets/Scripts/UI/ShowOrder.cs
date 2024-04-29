using UnityEngine;
using UnityEngine.UI;
using Barista.Menu;
using Barista.Food;

namespace Barista.UI
{
    public class ShowOrder : MonoBehaviour
    {
        [SerializeField] private GameObject m_container;
        [SerializeField] private GameObject[] m_orderSlots;
        [SerializeField] private Image[] m_imageSlots;


        private void OnEnable()
        {
            MenuFactory.OnRecipeGenerated += ShowClientOrder;
        }

        private void OnDestroy()
        {
            MenuFactory.OnRecipeGenerated -= ShowClientOrder;
        }

        private void ShowClientOrder()
        {
            m_container.SetActive(true);

            foreach(var slot in m_orderSlots) { slot.SetActive(false); }

            var CurrentRecipe = MenuFactory.Instance.CurrentRecipe;
            var AllImages = MenuFactory.Instance.FoodImages;

            int index = 0;

            foreach (var ele in CurrentRecipe)
            {
                int amount = ele.Value;
                while (amount > 0)
                {
                    m_orderSlots[index].SetActive(true);
                    m_imageSlots[index++].sprite = GetSpriteFromRecipe(ele.Key, AllImages);
                    amount--;
                }
            }
        }

        private Sprite GetSpriteFromRecipe(FoodType food, FoodImage[] foods)
        {
            foreach(var ele in foods)
            {
                if(ele.m_foodType == food) { return ele.m_sprite; }
            }

            return null;
        }
    }
}
