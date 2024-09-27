using AdvancedMenu;
using UnityEngine;

namespace Barista.UI
{
    public class GameUI : MonoBehaviour
    {
        private SceneLoader m_sceneLoader;

        private void Awake()
        {
            m_sceneLoader = FindObjectOfType<SceneLoader>();
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
    }
}
