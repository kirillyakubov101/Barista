using Barista.Menu;
using System.Collections;
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

        private void OnEnable()
        {
            MenuFactory.OnRecipeGenerated += DisplayOrder;
        }

        private void OnDestroy()
        {
            MenuFactory.OnRecipeGenerated -= DisplayOrder;
        }

        public void DisplayOrder()
        {
            StartCoroutine(nameof(DisplayOrderProcess));
        }

        private IEnumerator DisplayOrderProcess()
        {
            foreach (Transform child in m_Transform)
            {
                // Destroy the child GameObject
                Destroy(child.gameObject);
            }

            m_startingOffset = 0f;

            var Recipe = MenuFactory.Instance.CurrentRecipe;
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