using UnityEngine;
using UnityEngine.Splines;

public class TrainPath : MonoBehaviour
{
    public GameObject train;
    public SplineAnimate splineAnimate;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CreateTrainPath();
        }
    }

    public void CreateTrainPath()
    {
        
    }
}
