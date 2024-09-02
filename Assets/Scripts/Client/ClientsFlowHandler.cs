using UnityEngine;
using MyUtils;
using System.Collections;
using System.Collections.Generic;

namespace Barista.Clients
{
    public class ClientsFlowHandler : Singleton<ClientsFlowHandler>
    {
        [Header("Transforms")]
        [SerializeField] private Transform[] m_LinePositions;
        [SerializeField] private Transform m_clientSpawnPoint;
        [SerializeField] private Transform m_PlayerApproxTransform;
        [Header("Client Params")]
        [SerializeField] private ClientPawn m_clientPrefab;
        [SerializeField] private LayerMask m_ClientLayerMask;

        public int m_currentLinePositionIndex = 0;
        public int m_currentAmountOfClients = 0;

        private LinkedList<ClientPawn> m_ListOfClients = new LinkedList<ClientPawn>(); //Linked List of Clients
        private Transform m_CounterTransform;
        private float m_sleepTime = 4f;
        private ClientFlowTaskSystem m_cientFlowTaskSystem;
        private AnimGraph m_ClientsArrivalGraph;
        private float m_timer = 0f;

        private void Awake()
        {
            m_cientFlowTaskSystem = GetComponent<ClientFlowTaskSystem>();
            m_ClientsArrivalGraph = GetComponent<AnimGraph>();
        }

        private void Start()
        {
            if(m_LinePositions.Length > 0)
            {
                m_CounterTransform = m_LinePositions[0];
            }

            //make sure every client know about the player Pos
            ClientPawn.SetPlayerAndStartTransform(m_PlayerApproxTransform, m_clientSpawnPoint);
        }

        private void Update()
        {
            if(m_timer >= m_sleepTime)
            {
                m_timer = 0f;

                if(m_ClientsArrivalGraph.GetCurrentMaxClients() > m_currentAmountOfClients)
                {
                    SpawnClient();
                }
            }

            m_timer += Time.deltaTime;


        }

        public void ProcessClientLeft()
        {
            if(m_currentAmountOfClients == 0) { return; }

            m_ListOfClients.RemoveFirst();

            m_currentLinePositionIndex--;
            m_currentAmountOfClients--;

            if (m_ListOfClients.Count > 0)
            {
                m_cientFlowTaskSystem.AddNewTask(UpdateLineProcess());
            }
        }

        private IEnumerator UpdateLineProcess()
        {
            LinkedListNode<ClientPawn> CurrentNode = m_ListOfClients.First;
            Transform CurrentGoalTransform = m_CounterTransform;

            while (CurrentNode != null)
            {
                Transform PreviousTransform = CurrentNode.Value.m_TargetTransform;
                CurrentNode.Value.SetCurrentGoal(CurrentGoalTransform);
                CurrentNode.Value.AdvanceTheLine();
                CurrentGoalTransform = PreviousTransform;

                CurrentNode = CurrentNode.Next;
                yield return null;
            }
        }

     

        private void SpawnClient()
        {
            var newClient = Instantiate<ClientPawn>(m_clientPrefab, m_clientSpawnPoint.position, m_clientSpawnPoint.rotation);
            m_ListOfClients.AddLast(newClient);

            newClient.InitSpawnedClient(m_LinePositions[m_currentLinePositionIndex]);
            m_currentAmountOfClients++;
            m_currentLinePositionIndex++;
        }
    }
}
