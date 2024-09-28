using AdvancedMenu;
using Barista.Shift;
using TMPro;
using UnityEngine;

namespace Barista.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_errosText;
        [SerializeField] private TMP_Text m_servedClients;
        

        private SceneLoader m_sceneLoader;

        private void Awake()
        {
            m_sceneLoader = FindObjectOfType<SceneLoader>();
        }

        private void OnEnable()
        {
            ShiftHandler.Instance.OnEndShift.AddListener(PopulateShiftSummary);
        }

        private void OnDisable()
        {
            if (ShiftHandler.Instance)
            {
                ShiftHandler.Instance.OnEndShift.RemoveListener(PopulateShiftSummary);
            }
        }

        private void PopulateShiftSummary()
        {
            m_servedClients.text = ShiftHandler.Instance.CurrentAmountOfClientsVisited.ToString();
            m_errosText.text = ShiftHandler.Instance.AmountOfShiftFailures.ToString();
        }

        public void ReloadScene()
        {
           if(m_sceneLoader == null) { return; }

            m_sceneLoader.SetSceneToLoadName("MainScene");
            m_sceneLoader.LoadSceneAsync(); 
        }

        public void BackToMenu()
        {
            if (m_sceneLoader == null) { return; }
            m_sceneLoader.SetSceneToLoadName("AdvancedMenu");
            m_sceneLoader.LoadSceneAsync();
        }

        public void CompleteShift()
        {
            ReloadScene();
        }
    }
}
