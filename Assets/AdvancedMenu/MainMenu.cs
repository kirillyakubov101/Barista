using UnityEngine;
using UnityEngine.UI;

namespace AdvancedMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private string m_sceneToLoadName;
        [SerializeField] private Button m_startButton;

        private SceneLoader m_sceneLoader;

        private void Awake()
        {
            m_sceneLoader = FindObjectOfType<SceneLoader>();
        }

        private void Start()
        {
            m_startButton.onClick.AddListener(StartGame);
        }

        private void OnDisable()
        {
            m_startButton.onClick.RemoveAllListeners();
        }

        public void StartGame()
        {
            m_startButton.interactable = false;
            m_sceneLoader.SetSceneToLoadName(m_sceneToLoadName);
            m_sceneLoader.LoadSceneAsync();
            
        }
    }
}
