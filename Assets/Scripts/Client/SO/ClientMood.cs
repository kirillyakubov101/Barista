using UnityEngine;

namespace Barista.Clients
{
    public enum Mood
    {
        Good,
        Fair,
        Impatient,
        Angry,
        Leaving
    }

    [CreateAssetMenu(fileName = "NewMood", menuName = "ScriptableObjects/ClientMood", order = 1)]
    public class ClientMood : ScriptableObject
    {
        public Mood m_Mood;
        public Sprite m_Sprite;
        public float m_time;
    }
}
