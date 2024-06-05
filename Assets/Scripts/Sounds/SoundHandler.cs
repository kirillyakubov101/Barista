using MyUtils;
using UnityEngine;

namespace Barista.Sounds
{
    public class SoundHandler : Singleton<SoundHandler>
    {
        [Header("Stove")]
        [SerializeField] private AudioSource m_StoveAudioSource;

        public void PlayStoveSound(bool state)
        {
            if(state)
            {
                m_StoveAudioSource.Play();
            }
            else
            {
                m_StoveAudioSource.Stop();
            }
            
        }
    }
}
