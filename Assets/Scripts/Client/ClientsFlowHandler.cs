using UnityEngine;
using MyUtils;
using System.Collections;
using System.Collections.Generic;

namespace Barista.Clients
{
    public class ClientsFlowHandler : Singleton<ClientsFlowHandler>
    {
        [SerializeField] private Transform[] m_LinePositions;
        [SerializeField] private Transform[] m_clientSpawnPoints;
        [SerializeField] private Transform m_PlayerApproxTransform;

        [SerializeField] private ClientPawn m_clientPrefab;
        [SerializeField] private LayerMask m_ClientLayerMask;


        private LinkedList<ClientPawn> m_ListOfClients = new LinkedList<ClientPawn>();
        private const int c_MAX_Clients = 4;
        private int m_currentLinePositionIndex = 0;

        public bool test = false;

        private WaitForSeconds m_sleepTime = new WaitForSeconds(2f);

        private void Start()
        {
            StartCoroutine(BeginClientWorkFlow());
        }

        private IEnumerator BeginClientWorkFlow()
        {
            while (true)
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

        //private void Update()
        //{
        //    if(test)
        //    {
        //        test = false;
        //        m_clientsAmount = 0;
        //        StopCoroutine(BeginClientWorkFlow());
        //        StartCoroutine(BeginClientWorkFlow());
        //    }
        //}

        private void SpawnClient()
        {
            
            bool canSpawn = !IsSpawnPlaceOccupied(out Transform SpawnPoint);

            if(canSpawn)
            {
                var newClient = Instantiate<ClientPawn>(m_clientPrefab, SpawnPoint.position, SpawnPoint.rotation);
                m_ListOfClients.AddLast(newClient);

                newClient.InitSpawnedClient(m_LinePositions[m_currentLinePositionIndex++], m_PlayerApproxTransform);
            }

        }

        private bool IsSpawnPlaceOccupied(out Transform spawnPos)
        {
            spawnPos = null;
            int randomIndex = Random.Range(0, m_clientSpawnPoints.Length);

            spawnPos = m_clientSpawnPoints[randomIndex];

            var hits = Physics.OverlapSphere(spawnPos.position, 2, m_ClientLayerMask,queryTriggerInteraction:QueryTriggerInteraction.Collide);

            return hits.Length > 0;
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.DrawWireSphere(m_clientSpawnPoints[0].position, 2);
        //}

    }
}
