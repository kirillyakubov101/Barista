using Barista.MyCamera;
using UnityEngine;
using UnityEngine.UI;

namespace Barista.Clients
{
    public class EmoteHandler : MonoBehaviour
    {
        [SerializeField] private Image m_emoteImageContainer;
        [SerializeField] private Animator m_animator;

        private static Transform m_CenterCameraTransform = null;
        readonly int ShowTriggerId = Animator.StringToHash("Show");

        private void ShowEmote()
        {
            m_animator.SetTrigger(ShowTriggerId);
        }

        public void ChangeEmote(Mood clientMood)
        {
            ClientMood currentClientMood = MoodLoader.Instance.GetClientMood(clientMood);
            m_emoteImageContainer.sprite = currentClientMood.m_Sprite;
            ShowEmote();
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
