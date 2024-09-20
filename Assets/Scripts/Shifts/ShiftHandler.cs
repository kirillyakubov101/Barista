using UnityEngine;
using UnityEngine.Events;
using MyUtils;
using System.Collections;
using Barista.Order;

namespace Barista.Shift
{
    public class ShiftHandler : Singleton<ShiftHandler>
    {
        public UnityEvent OnStartShift;
        public UnityEvent OnEndShift;
        public UnityEvent OnFailShift;
        public UnityEvent OnNightTimeStart;

        private const int c_maxShiftFailures = 3;
        private int m_currentAmountOfDayTimeClients = -1; //to serve
        private int m_currentAmountOfClientsVisited = 0; //served
        private int m_amountOfShiftFailures = 0;
        private int m_currentAmountOfNightTimeClients = -1; //to serve
        private bool m_isNightTimeEnabled = false;
        private WaitForSeconds m_delayTime = new WaitForSeconds(2f);

        private void Start()
        {
            StartCoroutine(MainGameLoop());
        }

        private IEnumerator MainGameLoop()
        {
            InitRequiredClients();
            OnStartShift.Invoke();

            //outer game loop of the entire shift
            while(m_currentAmountOfClientsVisited < m_currentAmountOfDayTimeClients + m_currentAmountOfNightTimeClients)
            {
                if(m_currentAmountOfClientsVisited == m_currentAmountOfDayTimeClients)
                {
                    NightTimeStart();
                }

                yield return m_delayTime;
            }

            OnEndShift?.Invoke();
        }

        public void IncrementServedClient(bool state)
        {
            m_currentAmountOfClientsVisited++;

            if(!state)
            {
                m_amountOfShiftFailures++;
                if(m_amountOfShiftFailures == c_maxShiftFailures)
                {
                    OnFailShift?.Invoke();
                }
            }
        }


        private void OnEnable()
        {
            OnStartShift.AddListener(StartShift);
            OnEndShift.AddListener(EndShift);
            OnFailShift.AddListener(FailShift);

            OrderHandler.Instance.OnOrderComplete += IncrementServedClient;
        }

        private void OnDisable()
        {
            if(OrderHandler.Instance)
            {
                OrderHandler.Instance.OnOrderComplete += IncrementServedClient;
            }
            
        }

        private void StartShift()
        {
            
        }

        private void InitRequiredClients()
        {
            m_currentAmountOfDayTimeClients = GetRandomNumberOfClients(3, 7);
            m_currentAmountOfNightTimeClients = GetRandomNumberOfClients(5, 8);
        }


        private void EndShift() 
        {
            m_currentAmountOfClientsVisited = 0;
            m_amountOfShiftFailures = 0;

            print("shift done");
        }
        private void FailShift() 
        {
            print("fail");
        }

        private int GetRandomNumberOfClients(int a, int b)
        {
            return Random.Range(a, b);
        }

        private void NightTimeStart()
        {
            print("NIGHT");
            OnNightTimeStart.Invoke();
        }
    }
}
