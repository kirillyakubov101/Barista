using MyUtils;
using UnityEngine;

namespace Barista
{
    public class GameManager : Singleton<GameManager>
    {
        private float m_currentTimeScale = 1f;

        public void PauseGame()
        {
            m_currentTimeScale = 0f;
            UpdateTimeScale();
        }

        public void ContinueGame()
        {
            m_currentTimeScale = 1f;
            UpdateTimeScale();
        }

        private void UpdateTimeScale()
        {
            Time.timeScale = m_currentTimeScale;
        }
    }
}
