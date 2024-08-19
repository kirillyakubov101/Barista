using Barista.Order;
using System.Collections;
using UnityEngine;

namespace Barista.Clients
{
    public class ClientPawn : MonoBehaviour
    {
        public Transform m_TargetTransform = null;
        public Transform m_CurrentTransform = null;


        private Transform m_PlayerTransform = null;
        private Transform m_startTransform = null;

        private WaitForSeconds m_delay = new WaitForSeconds(.5f);
        private Animator m_Animator;
        private ClientTaskSystem m_clientTaskSystem;
        private PatienceBar m_patienceBar;

        readonly int idleHashIndex = Animator.StringToHash("Idle");
        readonly int walkHashIndex = Animator.StringToHash("Walk");

        static int count = 0;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_clientTaskSystem = GetComponent<ClientTaskSystem>();
            m_patienceBar = GetComponent<PatienceBar>();
        }

        private void Start()
        {
            m_Animator.CrossFadeInFixedTime(walkHashIndex, 0.25f);
        }

        public void InitSpawnedClient(Transform newGoal,Transform playerTranform,Transform startTransform)
        {
            name = count.ToString();
            count++;
            m_TargetTransform = newGoal;
            m_PlayerTransform = playerTranform;
            m_startTransform = startTransform;

            m_clientTaskSystem.AddNewTask(GetInTheLineProcess(m_TargetTransform));
            m_clientTaskSystem.AddNewTask(LookTowardsThePlayer());
        }

        public void AdvanceTheLine(Transform newGoal)
        {
            m_clientTaskSystem.AddNewTask(GetInTheLineProcess(newGoal));
        }

        private IEnumerator GetInTheLineProcess(Transform GoalTransfrom)
        {
            m_Animator.CrossFadeInFixedTime(walkHashIndex, 0.25f);

            while (Vector3.Distance(transform.position, GoalTransfrom.position) > .2f)
            {
                Vector3 direction = GoalTransfrom.position - transform.position;
                direction.Normalize();
                transform.position += direction * Time.deltaTime * 2f; // Use position for global movement
                yield return null;
            }
            //Reached his goal position
            m_Animator.CrossFadeInFixedTime(idleHashIndex, 0.25f);


            //if the client reached the counter (he is the first in line) he will make the order
            if (m_TargetTransform.CompareTag("Goal") && m_TargetTransform != m_CurrentTransform)
            {
                OrderHandler.Instance.TakeOrderFromClient(this);
            }

            m_CurrentTransform = m_TargetTransform;
            m_patienceBar.StartPatienceBar(); //the client reached either the line or the counter, now he waits
        }

        private IEnumerator LookTowardsThePlayer()
        {
            Vector3 direction = m_TargetTransform.position - m_startTransform.forward;
            direction.y = 0;
            Quaternion goalRotation = Quaternion.LookRotation(direction);
            float angle = Quaternion.Angle(goalRotation, transform.rotation);

            while (angle >= 2f)
            {
                angle = Quaternion.Angle(goalRotation, transform.rotation);

                transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, 10f * Time.deltaTime);

                yield return null;
            }
        }

        //Get the order from the barista
        public void RecieveOrder(bool isCorrectOrder)
        {
            if (isCorrectOrder)
            {
                ClientsFlowHandler.Instance.ProcessClientLeft();
                m_clientTaskSystem.AddNewTask(LookTowardsStart());
                m_clientTaskSystem.AddNewTask(LeaveProcess());
            }
            else
            {
                ClientsFlowHandler.Instance.ProcessClientLeft();
                m_clientTaskSystem.AddNewTask(LookTowardsStart());
                m_clientTaskSystem.AddNewTask(LeaveProcess());
            }
        }

        //Leave the store process
        private IEnumerator LeaveProcess()
        {
            m_Animator.CrossFadeInFixedTime(walkHashIndex, 0.25f);

            while (Vector3.Distance(transform.position, m_startTransform.position) > .2f)
            {
                Vector3 direction = m_startTransform.position - transform.position;
                direction.Normalize();
                transform.position += direction * Time.deltaTime * 2f; // Use position for global movement
                yield return null;
            }
            gameObject.name = "about to be destroyed!";
            Destroy(gameObject,5f);
        }

        private IEnumerator LookTowardsStart()
        {
            Vector3 direction = m_startTransform.position - transform.position;
            direction.y = 0;
            Quaternion goalRotation = Quaternion.LookRotation(direction);
            float angle = Quaternion.Angle(goalRotation, transform.rotation);

            while (angle >= 2f)
            {
                angle = Quaternion.Angle(goalRotation, transform.rotation);

                transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, 10f * Time.deltaTime);

                yield return null;
            }
        }

    }
}
