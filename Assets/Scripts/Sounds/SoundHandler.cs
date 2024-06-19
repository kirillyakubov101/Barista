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
        [Header("Shakers")]
        [SerializeField] private AudioSource[] m_ShakersAudioSource;
        [Header("Microwave")] [Tooltip("0 - door open 1- door close")]
        [SerializeField] private AudioSource[] m_microwaveAudioSources;
        [Header("Error")]
        [SerializeField] private AudioSource m_ErrorAudioSource;
        [Header("Correct")]
        [SerializeField] private AudioSource m_CorrectAudioSource;



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
                foreach (var ele in m_ShakersAudioSource)
                {
                    if (!ele.isPlaying)
                    {
                        ele.Play();
                        break;
                    }
                }
            }

            else
            {
                foreach (var ele in m_ShakersAudioSource)
                {
                    if (ele.isPlaying) { ele.Stop(); break; }
                }
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

        public void PlayMicrowaveDoorOpen(bool state)
        {
            if (state)
            {
                m_microwaveAudioSources[0].Play();
            }
            else
            {
                m_microwaveAudioSources[0].Stop();
            }
        }

        public void PlayMicrowaveDoorClose(bool state)
        {
            if (state)
            {
                m_microwaveAudioSources[1].Play();
            }
            else
            {
                m_microwaveAudioSources[1].Stop();
            }
        }


        public void PlayCorrectSound(bool state)
        {
            if (state)
            {
                m_CorrectAudioSource.Play();
            }
            else
            {
                m_CorrectAudioSource.Stop();
            }
        }

    }
}
