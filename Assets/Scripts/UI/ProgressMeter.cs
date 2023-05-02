using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace Barista
{
    public class ProgressMeter : MonoBehaviour
    {
        [SerializeField] private GameObject m_progressContainer;
        [SerializeField] private Image m_progressImage;
        public async Task ProgressFillProcess(float time)
        {
            float timer = 0f;
            m_progressContainer.SetActive(true);

            while (timer <= time)
            {
                if(m_progressImage == null) { return; } //avoid game crusing with the task still running
                
                m_progressImage.fillAmount = timer / time;


                timer += Time.deltaTime;
                await Task.Yield();
            }
            
            m_progressContainer.SetActive(false);
            m_progressImage.fillAmount = 0f;
        }
    }
}
