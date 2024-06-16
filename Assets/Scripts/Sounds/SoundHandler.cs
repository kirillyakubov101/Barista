using MyUtils;
using UnityEngine;

namespace Barista.Sounds
{
    public class SoundHandler : Singleton<SoundHandler>
    {
        [Header("Stove")]
        [SerializeField] private AudioSource m_StoveAudioSource;
        [Header("Coffee Machine")]
        [SerializeField] private AudioSource m_CoffeeMachineAudioSource;
        [Header("Shaker")]
        [SerializeField] private AudioSource m_ShakerAudioSource;
        [Header("Error")]
        [SerializeField] private AudioSource m_ErrorAudioSource;

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

        public void PlayCoffeeMachine(bool state)
        {
            if (state)
            {
                m_CoffeeMachineAudioSource.Play();
            }
            else
            {
                m_CoffeeMachineAudioSource.Stop();
            }
        }

        public void PlayShakerPourSound(bool state)
        {
            if (state)
            {
                m_ShakerAudioSource.Play();
            }
            else
            {
                m_ShakerAudioSource.Stop();
            }
        }

        public void PlayErrorSound(bool state)
        {
            if (state)
            {
                m_ErrorAudioSource.Play();
            }
            else
            {
                m_ErrorAudioSource.Stop();
            }
        }
    }
}
