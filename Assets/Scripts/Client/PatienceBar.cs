using Barista.Order;
using System.Collections;
using UnityEngine;

namespace Barista.Clients
{
    public class PatienceBar : MonoBehaviour
    {
        [SerializeField] private Mood ClientMood = Mood.Good;

        private float m_currentWaitTime;
        private EmoteHandler m_emoteHandler;
        private Coroutine m_coroutine;
        private bool m_hasFailed = false;

        public bool HasFailed { get => m_hasFailed; }

        private void Awake()
        {
            m_emoteHandler = GetComponent<EmoteHandler>();
        }

        public void StartPatienceBar()
        {
            m_currentWaitTime = MoodLoader.Instance.GetClientMood(ClientMood).m_time;
            m_emoteHandler.ChangeEmote(ClientMood);


            m_coroutine = StartCoroutine(WaitForOrderProcess());
        }

        public void StopPatienceBar()
        {
            StopCoroutine(m_coroutine);
        }

        //for wrong order
        public void ShowAngryEmote()
        {
            m_emoteHandler.ChangeEmote(Mood.Leaving);
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

            if((int)ClientMood > 4)
            {
                OrderHandler.Instance.FailToSubmitOrderOnTime();
                m_hasFailed = true;
                yield break;
            }

            m_emoteHandler.ChangeEmote(ClientMood);
            m_currentWaitTime = MoodLoader.Instance.GetClientMood(ClientMood).m_time;
            
            yield return WaitForOrderProcess();
        }
    }
}
