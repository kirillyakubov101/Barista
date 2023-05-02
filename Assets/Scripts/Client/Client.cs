
using System.Collections;
using UnityEngine;


namespace Barista.Clients
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;
        [SerializeField] private AnimationCurve m_walkCurve;

        readonly int hasIndex = Animator.StringToHash("Idle");

        private static bool firstTime = true;

        public Transform m_goal;

        public void InitClient(Transform goal)
        {
            m_goal = goal;

            StartCoroutine(ProceedTowardsDesk());
        }

      
        private void MoveForward()
        {

        }


        private IEnumerator ProceedTowardsDesk()
        {
            Transform availableTransform = m_goal;
            Vector3 goal = availableTransform.position;
            Quaternion lookDir =  Quaternion.LookRotation(goal - transform.position);

            transform.rotation = lookDir;

            while(Vector3.Distance(transform.position, goal) > UnityEngine.Random.Range(0.1f,1))
            {
                transform.position = Vector3.Lerp(transform.position, goal, m_walkCurve.Evaluate(Time.time) * Time.deltaTime);
                yield return null; 
            }

            m_animator.Play(hasIndex);
            float timer = 0f;
            while(timer < 2f)
            {
                transform.forward = Vector3.Lerp(transform.forward, availableTransform.forward, Time.deltaTime * 2.5f);

                timer += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(8f);
            if(firstTime)
            {
                firstTime = false;
                ClientManager.Instance.DespawnClient();
                Destroy(gameObject);
            }
        }

    }
}
