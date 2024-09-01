using Barista.Order;
using System.Collections;
using UnityEngine;

namespace Barista.Clients
{
    public class ClientPawn : MonoBehaviour
    {
        public Transform m_TargetTransform = null;
        public Transform m_CurrentTransform = null;


        private static Transform s_PlayerTransform = null;
        private static Transform s_startTransform = null;

        private WaitForSeconds m_delay = new WaitForSeconds(.5f);
        private Animator m_Animator;
        private ClientTaskSystem m_clientTaskSystem;
        private PatienceBar m_patienceBar;
        private bool m_isPatienceBarStarted = false;

        readonly int idleHashIndex = Animator.StringToHash("Idle");
        readonly int walkHashIndex = Animator.StringToHash("Walk");

        static int count = 0;

        public static void SetPlayerAndStartTransform(Transform playerTransform,Transform startTransform)
        {
            s_PlayerTransform = playerTransform;
            s_startTransform = startTransform;
        }

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

        public void InitSpawnedClient(Transform newGoal)
        {
            name = count.ToString();
            count++;
            m_TargetTransform = newGoal;

            m_clientTaskSystem.AddNewTask(GetInTheLineProcess(m_TargetTransform));
            m_clientTaskSystem.AddNewTask(LookTowardsTheGoal());
        }

        public void AdvanceTheLine(Transform newGoal)
        {
            m_clientTaskSystem.ClearTasks();
            m_clientTaskSystem.AddNewTask(GetInTheLineProcess(newGoal));
            m_clientTaskSystem.AddNewTask(LookTowardsTheGoal());
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
            if (m_TargetTransform.CompareTag("Goal") && m_TargetTransform != m_CurrentTransform && !m_patienceBar.HasFailed)
            {
                OrderHandler.Instance.TakeOrderFromClient(this);
            }

            m_CurrentTransform = m_TargetTransform;
            if(!m_isPatienceBarStarted)
            {
                m_isPatienceBarStarted = true;
                m_patienceBar.StartPatienceBar(); //the client reached either the line or the counter, now he waits
            }
            
        }

        private IEnumerator LookTowardsTheGoal()
        {
            Vector3 direction = m_TargetTransform.position - s_startTransform.forward;
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
            m_patienceBar.StopPatienceBar();

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

            while (Vector3.Distance(transform.position, s_startTransform.position) > .2f)
            {
                Vector3 direction = s_startTransform.position - transform.position;
                direction.Normalize();
                transform.position += direction * Time.deltaTime * 2f; // Use position for global movement
                yield return null;
            }
            gameObject.name = "about to be destroyed!";
            Destroy(gameObject,5f);
        }

        private IEnumerator LookTowardsStart()
        {
            Vector3 direction = s_startTransform.position - transform.position;
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
