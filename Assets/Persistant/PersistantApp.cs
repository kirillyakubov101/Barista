using UnityEngine;

public class PersistantApp : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
