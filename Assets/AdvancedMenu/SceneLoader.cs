using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdvancedMenu
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private string m_SceneToLoad;
        [SerializeField] private LoadingHandler m_LoadingHandler;

        private void Start()
        {
            LoadScene(); //loading first scene after Init
        }

        //Simple brute load scene
        public void LoadScene()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(m_SceneToLoad,LoadSceneMode.Single);
        }

        public void LoadSceneAsync()
        {
            StartCoroutine(LoadSceneAsyncProcesss());
        }

        public IEnumerator LoadSceneAsyncProcesss()
        {
            Time.timeScale = 1f;

            yield return m_LoadingHandler.StartFadeIn();

            AsyncOperation operation = SceneManager.LoadSceneAsync(m_SceneToLoad);
            m_LoadingHandler.LoadYourAsyncScene(m_SceneToLoad,operation);
        }


        #region Getters & Setters
        //Getters & Setters
        public string GetSceneToLoadName()
        {
            return m_SceneToLoad;
        }

        public void SetSceneToLoadName(string sceneToLoadName)
        {
            m_SceneToLoad = sceneToLoadName;
        }

        #endregion
    }
}
