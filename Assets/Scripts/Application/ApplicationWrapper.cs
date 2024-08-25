using MyUtils;
using UnityEngine;

namespace Barista
{
    public class ApplicationWrapper : Singleton<ApplicationWrapper>
    {
        [SerializeField] private Texture2D m_CursorTexture;
        [SerializeField] private Texture2D m_HoverCursorTexture;

        private void Start()
        {
            Cursor.SetCursor(m_CursorTexture, Vector2.zero,CursorMode.Auto);
            
        }

        public void ChangeCursor(bool isHover)
        {
            if (isHover)
            {
                Cursor.SetCursor(m_HoverCursorTexture, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(m_CursorTexture, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}
