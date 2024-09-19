using UnityEngine;
using UnityEngine.Events;
using MyUtils;

namespace Barista.Shift
{
    public class ShiftHandler : Singleton<ShiftHandler>
    {
        public UnityEvent OnStartShift;
        public UnityEvent OnEndShift;
        public UnityEvent OnFailShift;

        private void Start()
        {
            OnStartShift.Invoke();
        }

        private void OnEnable()
        {
            OnStartShift.AddListener(StartShift);
            OnEndShift.AddListener(EndShift);
            OnFailShift.AddListener(FailShift);
        }

        void StartShift()
        {

        }


        void EndShift() { }
        void FailShift() { }
    }
}
