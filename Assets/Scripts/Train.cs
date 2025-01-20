using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Train : MonoBehaviour
{

    public float speed = 1.0f;
    private bool isMoving = false;
    public bool isRunning = false;
    public List<Station> schedule = new();
    public Station currentStation;
    public Direction direction;
    public SplineAnimate splineAnimate;
    private PathingScript pathingScript;

    void Start()
    {
        pathingScript = new PathingScript();
    }

    void Update()
    {
        if (!isMoving)
        {
            if (currentStation == null) return;
            Vector2Int position2D = currentStation.GetNodeGroup().GetPosition();
            gameObject.transform.position = new Vector3(position2D.x, position2D.y, 0);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, (int)direction * 45);
            if (isRunning)
            {
                if (schedule.Count > 0)
                {
                    if (schedule.Contains(currentStation))
                    {
                        int index = (schedule.IndexOf(currentStation) + 1) % schedule.Count;
                        isMoving = true;
                        CalculatePath(schedule[index].GetNodeGroup());
                        currentStation = schedule[index];
                        splineAnimate.Play();
                    }
                }
            }
        }
    }

    public void CalculatePath(NodeGroup destination)
    {
        NodeGroup start = currentStation.GetNodeGroup();
        Debug.Log("from" + start.GetStation().GetName() + " to " + destination.GetStation().GetName());
        List<NodeGroup> path = pathingScript.GetPath(start, direction, destination);
        //splineAnimate = new SplineAnimate();
        //splineAnimate.Container =
        Debug.Log("Path calculated: " + path);
    }

    public void HandleClick()
    {
        GridManager.Instance.OpenTrainPopup(this);
    }

    public void ToggleIsRunning()
    {
        isRunning = !isRunning;
    }
}
