using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using AsyncOperation = UnityEngine.AsyncOperation;

namespace AdvancedMenu
{
    public class LoadingHandler : MonoBehaviour
    {
        [SerializeField] private Transform m_loadingScreen;
        [SerializeField] private Transform m_fader;

        private string m_CachedSceneToLoad;
        private Image m_faderImage;
        private AsyncOperation m_asyncOperation;

        private void Awake()
        {
            m_faderImage = m_fader.GetComponent<Image>();
        }

        public void LoadYourAsyncScene(string sceneName, AsyncOperation operation)
        {
            m_CachedSceneToLoad = sceneName;
            m_asyncOperation = operation;
            BeginLoadProcess();
        }

        private void BeginLoadProcess()
        {
            StartCoroutine(LoadYourAsyncSceneProcess(m_CachedSceneToLoad));
        }

        private IEnumerator LoadYourAsyncSceneProcess(string sceneName)
        {
            m_loadingScreen.gameObject.SetActive(true);
            while (!m_asyncOperation.isDone)
            {
                yield return null;
            }

            yield return new WaitForSeconds(4); //loading time artificial
            m_loadingScreen.gameObject.SetActive(false);

            yield return FadeOutCoroutine();
        }

        public IEnumerator StartFadeIn()
        {
            yield return FadeInCoroutine();
        }

        private IEnumerator FadeInCoroutine()
        {           
            m_fader.gameObject.SetActive(true);
            float progress = 0f;
            float maxTimer = 2f;
            float timer = 0f;
            Color cache = m_faderImage.color;

            while (timer < maxTimer)
            {
               
                cache.a = progress;
                m_faderImage.color = cache;
                timer += Time.deltaTime;
                progress += Time.deltaTime;
                progress = Math.Clamp(progress, 0, 1);
                yield return null;
            }
            yield return null;
        }

        private IEnumerator FadeOutCoroutine()
        {
            m_fader.gameObject.SetActive(true);
            float progress = 1f;
            float maxTimer = 2f;
            float timer = 0f;
            Color cache = m_faderImage.color;

            while (timer < maxTimer)
            {

                cache.a = progress;
                m_faderImage.color = cache;
                timer += Time.deltaTime;
                progress -= Time.deltaTime;
                progress = Math.Clamp(progress, 0, 1);
                yield return null;
            }
            m_fader.gameObject.SetActive(false);
            yield return null;
        }
    }
}
