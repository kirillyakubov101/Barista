using UnityEngine;

namespace Barista
{
    public class ApplicationWrapper : MonoBehaviour
    {
        [SerializeField] private Texture2D m_CursorTexture;
        private void Start()
        {
            Cursor.SetCursor(m_CursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
