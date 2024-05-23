using MyUtils;
using TMPro;
using UnityEngine;

namespace Barista.UI
{
    public enum ErrorType
    {
        WrongOrder
    }

    public class ErrorSystem : Singleton<ErrorSystem>
    {
        [SerializeField] private TMP_Text m_errorTextContainr = null;
        [SerializeField] private Animator m_errorAnimator = null;

        readonly int hashIndex = Animator.StringToHash("Display");


        public void DisplayError(ErrorType errorType)
        {
            m_errorTextContainr.text = errorType.ToString();
            m_errorAnimator.Play(hashIndex);
        }

    }
}
