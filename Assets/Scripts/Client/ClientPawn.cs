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

            m_TargetTransform = newGoal;

            m_clientTaskSystem.AddNewTask(LookTowardsTheCurrentGoal());
            m_clientTaskSystem.AddNewTask(GetInTheLineProcess());
            m_clientTaskSystem.AddNewTask(LookTowardsThePlayer());
        }

        public void AdvanceTheLine()
        {
            m_clientTaskSystem.ClearTasks();
            m_clientTaskSystem.AddNewTask(LookTowardsTheCurrentGoal());
            m_clientTaskSystem.AddNewTask(GetInTheLineProcess());
            m_clientTaskSystem.AddNewTask(LookTowardsThePlayer());
        }

        public void SetCurrentGoal(Transform newGoal)
        {
            m_TargetTransform = newGoal;
        }

        private IEnumerator GetInTheLineProcess()
        {
            m_Animator.CrossFadeInFixedTime(walkHashIndex, 0.25f);

            while (Vector3.Distance(transform.position, m_TargetTransform.position) > .2f)
            {
                Vector3 direction = m_TargetTransform.position - transform.position;
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

        private IEnumerator LookTowardsThePlayer()
        {
            // Calculate the direction to the player
            Vector3 direction = s_PlayerTransform.position - transform.position;

            // Zero out the y-component to only rotate around the Y axis
            direction.y = 0;

            // Ensure the direction vector is normalized (magnitude of 1)
            if (direction.sqrMagnitude > 0.01f) // Avoid errors with very small vectors
            {
                direction.Normalize();
            }

            // Calculate the goal rotation (Quaternion)
            Quaternion goalRotation = Quaternion.LookRotation(direction);

            // Set a duration for the rotation
            float rotationDuration = 1f;
            float timer = 0f;

            // Rotate smoothly over time
            while (timer < rotationDuration)
            {
                // Lerp between the current rotation and the goal rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, timer / rotationDuration);

                // Increment the timer
                timer += Time.deltaTime;

                // Yield until the next frame
                yield return null;
            }

            // Ensure final rotation is exactly the goal rotation
            transform.rotation = goalRotation;
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
                m_patienceBar.ShowAngryEmote();
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
            ClientDatabase.Instance.Release(gameObject);
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

        private IEnumerator LookTowardsTheCurrentGoal()
        {
            Vector3 direction = m_TargetTransform.position - transform.position;
            direction.y = 0;
            Quaternion goalRotation = Quaternion.LookRotation(direction);
            float angle = Quaternion.Angle(goalRotation, transform.rotation);

            while (angle >= 2f)
            {
                angle = Quaternion.Angle(goalRotation, transform.rotation);

                transform.rotation = Quaternion.Slerp(transform.rotation, goalRotation, 15f * Time.deltaTime);

                yield return null;
            }

        }

      


    }
}
