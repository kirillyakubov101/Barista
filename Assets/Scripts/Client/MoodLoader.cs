using MyUtils;
using System.Collections.Generic;
using UnityEngine;

namespace Barista.Clients
{
    public class MoodLoader : Singleton<MoodLoader>
    {
        private ClientMood[] m_moods = null;
        private Dictionary<Mood, ClientMood> m_moodLevels = new Dictionary<Mood, ClientMood>();

        private void Awake()
        {
            m_moods = Resources.LoadAll<ClientMood>("Mood");
        }

        private void Start()
        {
            foreach(var mood in m_moods)
            {
                m_moodLevels.Add(mood.m_Mood, mood);
            }
        }

        public ClientMood GetClientMood(Mood mood)
        {
            if(m_moodLevels.Count <= 0) { return null; }

            m_moodLevels.TryGetValue(mood, out var client);

            return client;
        }


    }
}
