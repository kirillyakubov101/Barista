using UnityEngine;
using MyUtils;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Barista.Clients
{
    public class ClientsFlowHandler : Singleton<ClientsFlowHandler>
    {
        [Header("Transforms")]
        [SerializeField] private Transform[] m_LinePositions;
        [SerializeField] private Transform[] m_clientSpawnPoints;
        [SerializeField] private Transform m_PlayerApproxTransform;
        [Header("Client Params")]
        [SerializeField] private ClientPawn m_clientPrefab;
        [SerializeField] private LayerMask m_ClientLayerMask;

        public List<ClientPawn> TestListOfClients = new List<ClientPawn>();

        private LinkedList<ClientPawn> m_ListOfClients = new LinkedList<ClientPawn>(); //Linked List of Clients
        public int m_currentLinePositionIndex = 0;
        private Transform m_CounterTransform;
        private float m_sleepTime = 2f;
        private ClientFlowTaskSystem m_cientFlowTaskSystem;
        private AnimGraph m_ClientsArrivalGraph;
        public int m_currentAmountOfClients = 0;
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

            TestListOfClients.Remove(m_ListOfClients.First.Value); //test
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
                CurrentNode.Value.AdvanceTheLine(CurrentGoalTransform);
                CurrentNode.Value.m_TargetTransform = CurrentGoalTransform;
                CurrentGoalTransform = PreviousTransform;


                CurrentNode = CurrentNode.Next;
                yield return new WaitForSeconds(1f);
            }
        }

     

        private void SpawnClient()
        {

            //bool canSpawn = !IsSpawnPlaceOccupied(out Transform SpawnPoint);

            //if(canSpawn)
            //{
            //    var newClient = Instantiate<ClientPawn>(m_clientPrefab, SpawnPoint.position, SpawnPoint.rotation);
            //    m_ListOfClients.AddLast(newClient);

            //    newClient.InitSpawnedClient(m_LinePositions[m_currentLinePositionIndex], m_PlayerApproxTransform, SpawnPoint);
            //    m_currentAmountOfClients++;
            //    m_currentLinePositionIndex++;
            //    TestListOfClients.Add(newClient); //test
            //}


            var newClient = Instantiate<ClientPawn>(m_clientPrefab, m_clientSpawnPoints[0].position, m_clientSpawnPoints[0].rotation);
            m_ListOfClients.AddLast(newClient);

            newClient.InitSpawnedClient(m_LinePositions[m_currentLinePositionIndex], m_PlayerApproxTransform, m_clientSpawnPoints[0]);
            m_currentAmountOfClients++;
            m_currentLinePositionIndex++;
            TestListOfClients.Add(newClient); //test

        }

        private bool IsSpawnPlaceOccupied(out Transform spawnPos)
        {
            spawnPos = null;
            int randomIndex = UnityEngine.Random.Range(0, m_clientSpawnPoints.Length);

            spawnPos = m_clientSpawnPoints[randomIndex];

            var hits = Physics.OverlapSphere(spawnPos.position, 2, m_ClientLayerMask,queryTriggerInteraction:QueryTriggerInteraction.Collide);

            return hits.Length > 0;
        }

      


    }
}
