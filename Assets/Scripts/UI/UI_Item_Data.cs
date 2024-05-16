using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Barista
{
    public class UI_Item_Data : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_amountText;
        [SerializeField] private Image m_image;
        [SerializeField] private AnimatorController m_animatorController;

        private Animator m_animator;
        private RectTransform m_rectTransform;

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_rectTransform = GetComponent<RectTransform>();

            if(m_animator== null)
            {
                m_animator = gameObject.AddComponent<Animator>();
                m_animator.runtimeAnimatorController = m_animatorController;
            }
        }

        public void InitUI_Item_Data(int amount,Sprite sprite)
        {
            m_amountText.text = amount.ToString();
            m_image.sprite = sprite;
        }

        public void DisplayItem(float positionY)
        {
            m_rectTransform.anchoredPosition = new Vector2(0f, positionY);
            m_animator.SetTrigger("Show");
        }
        
      
       
    }
}
