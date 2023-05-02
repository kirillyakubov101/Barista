using UnityEngine;

namespace MyUtils
{
    [ExecuteInEditMode]
    public class MeshCombiiner : MonoBehaviour
    {
        public GameObject[] objectsToCombine; // Array of GameObjects whose meshes will be combined

        public void CombineMeshes()
        {
            // Create a new combined mesh
            Mesh combinedMesh = new Mesh();

            // Get all MeshFilters from the GameObjects in the objectsToCombine array
            MeshFilter[] meshFilters = new MeshFilter[objectsToCombine.Length];
            for (int i = 0; i < objectsToCombine.Length; i++)
            {
                meshFilters[i] = objectsToCombine[i].GetComponent<MeshFilter>();
            }

            // Combine all meshes into the combined mesh
            combinedMesh.CombineMeshes(GetMeshCombineInstances(meshFilters));

            // Assign the combined mesh to this GameObject
            GetComponent<MeshFilter>().sharedMesh = combinedMesh;

            // Disable all MeshRenderers on the objectsToCombine array
            foreach (GameObject obj in objectsToCombine)
            {
                MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }

        private static CombineInstance[] GetMeshCombineInstances(MeshFilter[] meshFilters)
        {
            CombineInstance[] combineInstances = new CombineInstance[meshFilters.Length];
            for (int i = 0; i < meshFilters.Length; i++)
            {
                combineInstances[i].mesh = meshFilters[i].sharedMesh;
                combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }
            return combineInstances;
        }

        //Call CombineMeshes() automatically when the script is validated in the Editor mode
        private void OnValidate()
        {
            CombineMeshes();
        }
    }
}

