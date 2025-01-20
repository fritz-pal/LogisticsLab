using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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
    public GameObject splineObject;
    public float waitTime = 4.0f;
    private PathingScript pathingScript;
    private Vector3 velocity;
    private float timeAtLastMove = 0.0f;
    public TextMeshProUGUI errorText;

    void Start()
    {
        pathingScript = new PathingScript();
        errorText.text = "";
    }

    void Update()
    {
        if (!isMoving)
        {
            if (currentStation == null) return;
            Vector2Int position2D = currentStation.GetNodeGroup().GetPosition();
            gameObject.transform.SetPositionAndRotation(new Vector3(position2D.x, position2D.y, 0), Quaternion.Euler((7 - (int)direction) * 45 - 45, 90, -90));
            if (isRunning && Time.time - timeAtLastMove > waitTime)
            {
                if (schedule.Count > 0)
                {
                    if (schedule.Contains(currentStation))
                    {
                        int index = (schedule.IndexOf(currentStation) + 1) % schedule.Count;
                        
                        if (CalculatePath(schedule[index].GetNodeGroup()))
                        {
                            currentStation = schedule[index];
                            isMoving = true;
                            errorText.text = "";
                        }
                        else
                        {
                            Debug.Log("No path to destination: " + schedule[index].GetName());
                            errorText.text = "No path to destination: " + schedule[index].GetName();
                            timeAtLastMove = Time.time;
                        }
                    }
                }
            }
        }
        else
        {
            var native = new NativeSpline(splineObject.GetComponent<SplineContainer>().Splines[0]);
            SplineUtility.GetNearestPoint(native, transform.position, out float3 nearest, out float t);
            transform.position = nearest;

            Vector3 forward = Vector3.Normalize(native.EvaluateTangent(t));
            Vector3 up = native.EvaluateUpVector(t);
            transform.rotation = Quaternion.LookRotation(forward, up) * Quaternion.Inverse(Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(0, 1, 0)));

            Vector3 trainForward = transform.forward;

            if (Vector3.Dot(velocity, transform.forward) < 0)
                trainForward *= -1;

            velocity = trainForward * speed;
            transform.position += velocity * Time.deltaTime;

            float distance = Vector3.Distance(transform.position, new Vector3(currentStation.GetNodeGroup().GetPosition().x, currentStation.GetNodeGroup().GetPosition().y, 0));
            if (distance < 0.2f)
            {
                isMoving = false;
                timeAtLastMove = Time.time;
            }
        }
    }

    public bool CalculatePath(NodeGroup destination)
    {
        NodeGroup start = currentStation.GetNodeGroup();
        Debug.Log("calculating from" + start.GetStation().GetName() + " to " + destination.GetStation().GetName());
        List<Node> path = pathingScript.GetPath(start, direction, destination);
        Debug.Log("Path calculated");
        if (path == null || path.Count == 0)
            return false;

        SplineContainer container = splineObject.GetComponent<SplineContainer>();
        Spline spline = container.Splines[0];
        spline.Clear();

        Node next = path[0];
        Quaternion knotRotation = Quaternion.LookRotation(Vector3.up);
        float tangentLength = Mathf.Min(2, Vector2Int.Distance(start.GetPosition(), next.position) / 3);
        Vector3 startTangent = new Vector3(GridManager.VectorFromDirection(direction).x, 0, GridManager.VectorFromDirection(direction).y) * tangentLength;
        spline.Add(new BezierKnot(new Vector3(start.GetPosition().x, start.GetPosition().y, 0), startTangent, startTangent, knotRotation));

        for (int i = 0; i < path.Count; i++)
        {
            Node currentNode = path[i];

            Vector2Int inVector = GridManager.VectorFromDirection(GridManager.FlipDirection(currentNode.direction));
            Vector3 inTangent = new Vector3(inVector.x, 0, inVector.y) * tangentLength;
            if (i != path.Count - 1)
            {
                tangentLength = Mathf.Min(2, Vector2Int.Distance(currentNode.position, path[i + 1].position) / 3);
            }
            Vector2Int outVector = GridManager.VectorFromDirection(currentNode.direction);
            Vector3 outTangent = new Vector3(outVector.x, 0, outVector.y) * tangentLength;
            spline.Add(new BezierKnot(new Vector3(currentNode.position.x, currentNode.position.y, 0), -inTangent, -outTangent, knotRotation));
        }

        direction = pathingScript.GetDestinationDirection();
        return true;
    }

    public void HandleClick()
    {
        GridManager.Instance.OpenTrainPopup(this);
    }

    public void ToggleIsRunning()
    {
        if (schedule.Count < 2) 
        {
            Debug.Log("Not enough stations in schedule");
            errorText.text = "Not enough stations in schedule";
            return;
        }
        if (schedule.Count != new HashSet<Station>(schedule).Count)
        {
            Debug.Log("There are duplicate stations in the schedule");
            errorText.text = "There are duplicate stations in the schedule";
            return;
        }
        currentStation ??= schedule[0];
        isRunning = !isRunning;
    }
}
