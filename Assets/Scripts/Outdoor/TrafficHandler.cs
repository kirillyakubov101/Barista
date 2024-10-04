using UnityEngine;

namespace Barista.Outdoor
{
    public class TrafficHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Vehicle vehicle))
            {
                vehicle.ChangeDirection();
            }
            
        }
    }
}
