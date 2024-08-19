using System.Collections;
using UnityEngine;

namespace Barista.Clients
{
    public class PatienceBar : MonoBehaviour
    {
        [SerializeField] private Mood ClientMood = Mood.Good;

        private float m_currentWaitTime;

        public void StartPatienceBar()
        {
            m_currentWaitTime = MoodLoader.Instance.GetPatienceTime(ClientMood);

            StartCoroutine(WaitForOrderProcess());
        }

        private IEnumerator WaitForOrderProcess()
        {
            float timer = 0f;

            while (timer < m_currentWaitTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            ClientMood++;
            m_currentWaitTime = MoodLoader.Instance.GetPatienceTime(ClientMood);
        }
    }
}
