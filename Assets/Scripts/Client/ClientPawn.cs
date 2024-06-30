using Barista.Order;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Barista.Clients
{
    public enum ClientState
    {
        Waiting,
        Moving
    }

    public class ClientPawn : MonoBehaviour
    {
        [SerializeField] private float m_interpalationAlpha = 0.7f;


        private Transform m_goal = null;
        private Transform m_PlayerTransform = null;
        private Transform m_startTransform = null;
        private WaitForSeconds m_delay = new WaitForSeconds(.5f);
        private Animator m_Animator;
        private ClientState m_clientState;
        private ClientTaskSystem m_clientTaskSystem;

        readonly int idleHashIndex = Animator.StringToHash("Idle");
        readonly int walkHashIndex = Animator.StringToHash("Walk");

        private Coroutine m_Coroutine_Walking;
        private Coroutine m_Coroutine_Turning;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_clientTaskSystem = GetComponent<ClientTaskSystem>();
        }

        private void Start()
        {
            m_Animator.CrossFadeInFixedTime(walkHashIndex, 0.25f);
        }


        public void InitSpawnedClient(Transform newGoal,Transform playerTranform,Transform startTransform)
        {
            m_goal = newGoal;
            m_PlayerTransform = playerTranform;
            m_startTransform = startTransform;
            SetClientState(ClientState.Moving); //set client state to moving


            m_clientTaskSystem.AddNewTask(GetInTheLineProcess());
            m_clientTaskSystem.AddNewTask(LookTowardsThePlayer());
        }

        public void AdvanceTheLine()
        {
            m_Coroutine_Walking = StartCoroutine(GetInTheLineProcess());
        }

        private IEnumerator GetInTheLineProcess()
        {
            while(Vector3.Distance(transform.position, m_goal.position) > .2f)
            {
                Vector3 direction = m_goal.position - transform.position;
                direction.Normalize();
                transform.position += direction * Time.deltaTime * 2f; // Use position for global movement
                yield return null;
            }

            //Reached his goal position
            m_Animator.CrossFadeInFixedTime(idleHashIndex, 0.25f);
            SetClientState(ClientState.Waiting); //set client state to Waiting

            //if the client reached the counter (he is the first in line) he will make the order
            if (m_goal.CompareTag("Goal"))
            {
               //
            }
        }

        private IEnumerator LookTowardsThePlayer()
        {
            Vector3 direction = m_PlayerTransform.position - transform.position;
            Quaternion goalRotation = Quaternion.LookRotation(direction);

            while (transform.rotation.Compare(goalRotation,1))
            {
                transform.forward = Vector3.Lerp(transform.forward, m_goal.forward, Time.deltaTime * 15f);
                yield return null;
            }
        }

        private void RecieveOrder(bool isCorrectOrder)
        {
            OrderHandler.Instance.OnOrderComplete -= RecieveOrder;

            if (isCorrectOrder)
            {
                ClientsFlowHandler.Instance.ProcessClientLeft();

                if(m_Coroutine_Walking!= null)
                {
                    StopCoroutine(m_Coroutine_Walking);
                }
                if(m_Coroutine_Turning != null)
                {
                    StopCoroutine(m_Coroutine_Turning);
                }

                StartCoroutine(LeaveProcess());
                StartCoroutine(LookTowardsStart());
            }
            else
            {
                ClientsFlowHandler.Instance.ProcessClientLeft();

                if (m_Coroutine_Walking != null)
                {
                    StopCoroutine(m_Coroutine_Walking);
                }
                if (m_Coroutine_Turning != null)
                {
                    StopCoroutine(m_Coroutine_Turning);
                }

                StartCoroutine(LeaveProcess());
                StartCoroutine(LookTowardsStart());

            }
        }

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

            Destroy(gameObject);
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

        //Getters Setters
        public Transform GetGoalTransform() { return m_goal; }
        public void SetGoalTransform(Transform goalTransform) { m_goal = goalTransform; }

        public void SetClientState(ClientState state)
        {
            this.m_clientState = state;
        }

        public ClientState GetClientState()
        {
            return m_clientState;
        }
    }
}
