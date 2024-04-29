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

        string[] names = { "Tom", "Sean", "Maria", "Lina" };

        private int iterator = 0;


        private void Start()
        {

            Invoke(nameof(FirstSpawnClient), 1f);
            Invoke(nameof(FirstSpawnClient), 2f);
            Invoke(nameof(FirstSpawnClient), 3f);
            Invoke(nameof(FirstSpawnClient), 4f);

        }

        public bool click = false;
        private void Update()
        {
            if(click)
            {
                click = false;
                ClientLeave();
            }
        }

        private void FirstSpawnClient()
        {
            var client = Instantiate(m_client,transform);
            client.InitClient(m_GoalPositions[iterator], names[iterator]);

            m_clients.AddLast(client);

            iterator++;
           
        }

        private void PrintClients()
        {
            foreach (var client in m_clients)
            {
                print(client.clientName + " " + client.m_goal);
            }
        }

        public void ClientLeave()
        {
            LinkedListNode<Client> currentNode = m_clients.First.Next;               //cache the next node after head
            Transform cachePos = currentNode.Value.m_goal;                           //cache pos saving '2'
            currentNode.Value.m_goal = m_clients.First.Value.m_goal;                 //update its position 'update to '1'

            Client first = m_clients.First.Value;
            m_clients.RemoveFirst();                                                 //remove First client
            Destroy(first.gameObject);

            TakeOverNextPosition(currentNode, cachePos);

            PrintClients();
        }

        private void TakeOverNextPosition(LinkedListNode<Client> node, Transform pos)
        {
            if (node.Next != null)
            {
                node.Value.WalkTowardsEmptySpace();
                node = node.Next;
                Transform cachePos = node.Value.m_goal;
                node.Value.m_goal = pos;

                TakeOverNextPosition(node, cachePos);
            }
            else
            {
                node.Value.WalkTowardsEmptySpace();
            }
        }





    }
}
