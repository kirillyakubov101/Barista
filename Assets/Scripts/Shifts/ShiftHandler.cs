using System.Collections;
using System.Collections.Generic;
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

        private void OnEanble
        {
            
        }

        void StartShift() {}
        void EndShift() { }
        void FailShift() { }
    }
}
