using Barista.Menu;
using Barista.Order;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barista
{
    public class AddOrderToScreen : MonoBehaviour
    {
        [SerializeField] private UI_Item_Data m_UI_Element_Prefab;
        [SerializeField] private RectTransform m_Transform;
        [SerializeField] private float m_showUI_Item_Delay = 0.75f;
        

        [SerializeField] private float m_offsetY = 100f;

        private float m_startingOffset = 0f;
        private Coroutine m_current;

        public void DisplayOrder()
        {
            if(m_current != null) { StopCoroutine(m_current); }
            m_current = StartCoroutine(nameof(DisplayOrderProcess));
        }

        public void ClearOrderScreen(bool status = true)
        {
            foreach (Transform child in m_Transform)
            {
                // Destroy the child GameObject
                Destroy(child.gameObject);
            }

            m_startingOffset = 0f;
        }


        private IEnumerator DisplayOrderProcess()
        {
            ClearOrderScreen();

            m_startingOffset = 0f;

            var Recipe = new Dictionary<Food.FoodType,int>(MenuFactory.Instance.CurrentRecipe);

            foreach (var item in Recipe)
            {
                UI_Item_Data newInst = Instantiate(m_UI_Element_Prefab, m_Transform);

                int amount = item.Value;
                Sprite sprite = MenuFactory.Instance.GetSpriteFromRecipe(item.Key);

                newInst.InitUI_Item_Data(amount, sprite);
                newInst.DisplayItem(m_startingOffset);

                m_startingOffset -= m_offsetY;

                yield return new WaitForSeconds(m_showUI_Item_Delay);
            }
        }



       

    }
}
