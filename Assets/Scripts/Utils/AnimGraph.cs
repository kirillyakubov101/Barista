using UnityEngine;

public class AnimGraph : MonoBehaviour
{
    [SerializeField] private AnimationCurve _graph;

    private int enterCustomersAmount = 0;

    float counter = 0f;
    private void Update()
    {
        counter += Time.deltaTime;
       
        if(counter > 5f)
        {
            enterCustomersAmount = Mathf.CeilToInt(_graph.Evaluate(Time.time));
            print(enterCustomersAmount);
            counter = 0f;
        }
    }
}
