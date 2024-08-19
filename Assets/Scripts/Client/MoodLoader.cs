using MyUtils;
using System.Collections.Generic;
using UnityEngine;

namespace Barista.Clients
{
    public class MoodLoader : Singleton<MoodLoader>
    {
        private ClientMood[] m_moods = null;
        private Dictionary<Mood, float> m_moodLevels = new Dictionary<Mood, float>();

        private void Awake()
        {
            m_moods = Resources.LoadAll<ClientMood>("Mood");
        }

        private void Start()
        {
            foreach(var mood in m_moods)
            {
                m_moodLevels.Add(mood.m_Mood,mood.m_time);
            }
        }

        public float GetPatienceTime(Mood mood)
        {
            bool success = m_moodLevels.TryGetValue(mood, out float value);

            if (success)
            {
                return value;
            }
            else
            {
                print("GetPatienceTime wrong mood");
                return -1f;
            }

        }
    }
}
