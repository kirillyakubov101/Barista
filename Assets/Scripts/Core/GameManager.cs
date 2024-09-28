using MyUtils;
using UnityEngine;

namespace Barista
{
    public class GameManager : Singleton<GameManager>
    {
        private float m_currentTimeScale = 1f;
        private int m_clientsServed = 0;
        private int m_mistakes = 0;
        private int m_refilFoods = 0;
        private int m_coffeeMade = 0;
        private int m_foodWasted = 0;
        private int m_shiftsDone = 0;

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
