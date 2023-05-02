using UnityEngine;
using MyUtils;
using System.Collections.Generic;

namespace Barista.Clients
{
    public class ClientManager : Singleton<ClientManager>
    {
        [SerializeField] private Client m_client;
        [SerializeField] private Transform[] m_GoalPositions;

        private LinkedList<Client> m_clients = new LinkedList<Client>();

        private int iterator = 0;


        private void Start()
        {
           



            Invoke(nameof(FirstSpawnClient), 2f);
            Invoke(nameof(FirstSpawnClient), 4f);
            Invoke(nameof(FirstSpawnClient), 6f);
            Invoke(nameof(FirstSpawnClient), 8f);
        }

        private void FirstSpawnClient()
        {
            var client = Instantiate(m_client,transform);
            client.InitClient(m_GoalPositions[iterator]);

            m_clients.AddLast(client);

            
           
        }

        public void DespawnClient()
        {
          

            

        }

       
    }
}
