using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Barista
{
    public class ProgressMeter : MonoBehaviour
    {
        [SerializeField] private GameObject m_progressContainer;
        [SerializeField] private Image m_progressImage;
      
        public IEnumerator ProgressFillProcess(float time)
        {
            float timer = 0f;
            m_progressContainer.SetActive(true);

            while (timer <= time)
            {
                if (m_progressImage == null) { yield return null; } //avoid game crusing with the task still running

                m_progressImage.fillAmount = timer / time;
                timer += Time.deltaTime;
                yield return null;
            }

            m_progressContainer.SetActive(false);
            m_progressImage.fillAmount = 0f;
        }
    }
}
