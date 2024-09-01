using UnityEngine;

public class AnimGraph : MonoBehaviour
{
    [SerializeField] private AnimationCurve _graph;

    public int GetCurrentMaxClients()
    {
        print(Mathf.CeilToInt(_graph.Evaluate(Time.time)));
        return Mathf.CeilToInt(_graph.Evaluate(Time.time));
    }
   
}
