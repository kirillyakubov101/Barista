using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barista.Clients
{
    [RequireComponent(typeof(ClientPawn))]
    public class ClientTaskSystem : MonoBehaviour
    {
        private Queue<IEnumerator> m_ListOfTasks = new Queue<IEnumerator>();
        private ClientPawn m_Client;
        private bool m_isProcessing = false;
        private Coroutine m_mainTaskThread;

        private void Awake()
        {
            m_Client = GetComponent<ClientPawn>();
        }

        public void AddNewTask(IEnumerator Task)
        {
            if(Task == null) { return; }
            m_ListOfTasks.Enqueue(Task);
            
            //if the task execution did not start, start it. If it is already running it will find the newly added task
            if (!m_isProcessing)
            {
                m_mainTaskThread = StartCoroutine(ProcessingTasks());
            }
        }


        private IEnumerator ProcessingTasks()
        {
            while(m_ListOfTasks.Count > 0)
            {
                if (m_ListOfTasks.TryDequeue(out IEnumerator task))
                {
                    m_isProcessing = true; //task was found in the queue, begin process
                    yield return task;

                }
            }

            m_isProcessing = false;

        }

       
    }
}
