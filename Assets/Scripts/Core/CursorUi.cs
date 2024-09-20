using UnityEngine;

namespace Barista.Core
{
    public class CursorUi : MonoBehaviour
    {
        private Raycaster m_Raycaster;

        private void Awake()
        {
            m_Raycaster = GetComponent<Raycaster>();
        }

        private void OnEnable()
        {
            m_Raycaster.OnRaycastHitChange += ChangeCursor;
        }

        private void OnDisable()
        {
            m_Raycaster.OnRaycastHitChange -= ChangeCursor;
        }

        private void ChangeCursor(bool state)
        {
            if (!ApplicationWrapper.Instance) { return; }
            ApplicationWrapper.Instance.ChangeCursor(state);
        }


    }
}
