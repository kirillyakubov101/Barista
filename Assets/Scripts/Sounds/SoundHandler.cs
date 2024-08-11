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
        [Header("Microwave")] [Tooltip("0 - door open 1- door close 2 - beep")]
        [SerializeField] private AudioSource[] m_microwaveAudioSources;
        [Header("Error")]
        [SerializeField] private AudioSource m_ErrorAudioSource;
        [Header("Correct")]
        [SerializeField] private AudioSource m_CorrectAudioSource;
        [Header("EmptyClick")]
        [SerializeField] private AudioSource m_EmptyClickAudioSource;

        public void PlayEmptyClick(bool state)
        {
            ProccessState(state, m_EmptyClickAudioSource);
        }

        public void PlayStoveSound(bool state)
        {
            ProccessState(state, m_StoveAudioSource);
        }

        public void PlayCoffeeMachine(bool state)
        {
            ProccessState(state, m_CoffeeMachineAudioSource);
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
            ProccessState(state, m_ErrorAudioSource);
        }

        public void PlayMicrowaveDoorOpen(bool state)
        {
            ProccessState(state, m_microwaveAudioSources[0]);
        }

        public void PlayMicrowaveDoorClose(bool state)
        {
            ProccessState(state, m_microwaveAudioSources[1]);
        }


        public void PlayCorrectSound(bool state)
        {
            ProccessState(state, m_CorrectAudioSource);
        }

        public void PlayeMicroDoneBeep(bool state)
        {
            ProccessState(state, m_microwaveAudioSources[2]);
        }

        private void ProccessState(bool state, AudioSource source)
        {
            if (state)
            {
                source.Play();
            }
            else
            {
                source.Stop();
            }
        }

    }
}
