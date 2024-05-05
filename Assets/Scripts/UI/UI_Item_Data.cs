using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Barista
{
    public class UI_Item_Data : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_amountText;
        [SerializeField] private Image m_image;

        public void InitUI_Item_Data(int amount,Sprite sprite)
        {
            m_amountText.text = amount.ToString();
            m_image.sprite = sprite;
        }
        
      
       
    }
}
