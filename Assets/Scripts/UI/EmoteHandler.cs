using Barista.MyCamera;
using UnityEngine;
using UnityEngine.UI;

namespace Barista
{
    public class EmoteHandler : MonoBehaviour
    {
        [SerializeField] private Image m_emoteImageContainer;
        [SerializeField] private Animator m_animator;

        private static Transform m_CenterCameraTransform = null;

        public void ShowEmote()
        {

        }

        public void HideEmote()
        {

        }

        public void ChangeEmote()
        {

        }

        private void Start()
        {
            if(m_CenterCameraTransform == null)
            {
                m_CenterCameraTransform = CameraController.Instance.GetCenterCameraTransform();
            }   
        }


        private void LateUpdate()
        {
            if(m_CenterCameraTransform == null) { return; }
            m_emoteImageContainer.transform.forward= m_CenterCameraTransform.forward;
        }
    }
}
