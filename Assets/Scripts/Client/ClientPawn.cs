using System.Collections;
using TMPro;
using UnityEngine;

namespace Barista
{
    public class ClientPawn : MonoBehaviour
    {
        private Transform m_goal = null;
        private Transform m_PlayerTransform = null;
        private WaitForSeconds m_delay = new WaitForSeconds(.5f);
        [SerializeField] private float m_interpalationAlpha = 0.7f;
        public void InitSpawnedClient(Transform newGoal,Transform playerTranform)
        {
            m_goal = newGoal;
            m_PlayerTransform = playerTranform;

            StartCoroutine(GetInTheLineProcess());
            StartCoroutine(LookTowardsThePlayer());
        }

        private IEnumerator GetInTheLineProcess()
        {
            while(Vector3.Distance(transform.position, m_goal.position) > .2f)
            {
                transform.position = Vector3.Slerp(transform.position, m_goal.position, Time.deltaTime * m_interpalationAlpha);
                yield return null;
            }

            if(m_goal.CompareTag("Goal"))
            {
                print("FIRST ONE IN LINE, TIME TO ORDER");
            }
        }

        private IEnumerator LookTowardsThePlayer()
        {
            Vector3 direction = m_PlayerTransform.position - transform.position;
            Quaternion goalRotation = Quaternion.LookRotation(direction);

            while (transform.rotation.Compare(goalRotation,1))
            {
                transform.forward = Vector3.Lerp(transform.forward, m_goal.forward, Time.deltaTime * m_interpalationAlpha);
                yield return null;
            }
        }
    }
}
