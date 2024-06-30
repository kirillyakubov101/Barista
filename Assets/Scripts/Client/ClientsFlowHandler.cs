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

        private LinkedList<ClientPawn> m_ListOfClients = new LinkedList<ClientPawn>(); //Linked List of Clients

        private const int c_MAX_Clients = 1;
        private int m_currentLinePositionIndex = 0; //TODO: adjust this when new clients spawn after intial group
        private Transform m_CounterTransform;
        private WaitForSeconds m_sleepTime = new WaitForSeconds(2f);

        private void Start()
        {
            if(m_LinePositions.Length > 0)
            {
                m_CounterTransform = m_LinePositions[0];
            }
           
            StartCoroutine(BeginClientWorkFlow());
        }

        public void ProcessClientLeft()
        {
            if(m_ListOfClients.Count == 0) { return; }

            m_currentLinePositionIndex--;

            m_ListOfClients.RemoveFirst();

            if (m_ListOfClients.Count > 0)
            {
                StartCoroutine(UpdateLineProcess());
            }
        }

        private IEnumerator UpdateLineProcess()
        {
            LinkedListNode<ClientPawn> CurrentNode = m_ListOfClients.First;
            Transform CurrentGoalTransform = m_CounterTransform;

            while (CurrentNode != null)
            {
               
                Transform temp = CurrentNode.Value.GetGoalTransform();
                CurrentNode.Value.SetGoalTransform(CurrentGoalTransform);
                CurrentGoalTransform = temp;
                CurrentNode.Value.AdvanceTheLine();
                CurrentNode = CurrentNode.Next;

                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }


        private IEnumerator BeginClientWorkFlow()
        {
            while (m_ListOfClients.Count < c_MAX_Clients)
            {
                if (IsRestaurantFull()) { break; }
                SpawnClient();

                yield return m_sleepTime;
            }
        }

        private bool IsRestaurantFull()
        {
            return m_ListOfClients.Count >= c_MAX_Clients;
        }

        private void SpawnClient()
        {
            
            bool canSpawn = !IsSpawnPlaceOccupied(out Transform SpawnPoint);

            if(canSpawn)
            {
                var newClient = Instantiate<ClientPawn>(m_clientPrefab, SpawnPoint.position, SpawnPoint.rotation);
                newClient.name = m_currentLinePositionIndex.ToString();
                m_ListOfClients.AddLast(newClient);

                newClient.InitSpawnedClient(m_LinePositions[m_currentLinePositionIndex++], m_PlayerApproxTransform, SpawnPoint);
            }

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
