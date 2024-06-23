using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Barista
{
    public class ClientPawn : MonoBehaviour
    {
        [SerializeField] private float m_interpalationAlpha = 0.7f;


        private Transform m_goal = null;
        private Transform m_PlayerTransform = null;
        private WaitForSeconds m_delay = new WaitForSeconds(.5f);
        private Animator m_Animator;

        readonly int hasIndex = Animator.StringToHash("Idle");

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        public void InitSpawnedClient(Transform newGoal,Transform playerTranform)
        {
            m_goal = newGoal;
            m_PlayerTransform = playerTranform;

            StartCoroutine(GetInTheLineProcess());
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


            m_Animator.Play(hasIndex);

            if (m_goal.CompareTag("Goal"))
            {
                print("FIRST ONE IN LINE, TIME TO ORDER");
            }

            yield return LookTowardsThePlayer();
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
    }
}
