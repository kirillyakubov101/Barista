using System.Linq;
using UnityEngine;

namespace Barista
{
    public class RandomizeStaticClientTable : MonoBehaviour
    {
        [SerializeField] private Transform[] Table1;
        [SerializeField] private Transform[] Table2;
        [SerializeField] private Transform[] Table3;

        private Transform[][] m_allTables;

        private void Start()
        {
            m_allTables = new Transform[][] { Table1, Table2, Table3 };

            m_allTables
              .SelectMany(table => table) // Flatten the jagged array into a single sequence
              .Where(element => element != null) // Ensure no null elements
              .ToList() // Execute the LINQ query
              .ForEach(element =>
              {
                  bool randomState = Random.value > 0.5f; // Randomly true or false
                  element.gameObject.SetActive(randomState); // Enable or disable the GameObject
              });
        }
    }
}
