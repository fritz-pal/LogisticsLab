using System.Collections.Generic;
using System.Data;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class GridManager : MonoBehaviour
{
    public GameObject gridPrefab;
    public int width, height;
    public GameObject railPrefab;
    public static GridManager Instance { get; private set; }
    public GameObject splineObject;
    private Dictionary<Vector2Int, NodeGroup> nodeGroups = new Dictionary<Vector2Int, NodeGroup>();
    private List<Track> tracks = new List<Track>();

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        for (int x = -width/2; x < width/2; x++)
        {
            for (int y = -height/2; y < height/2; y++)
            {
                GameObject grid = Instantiate(gridPrefab, new Vector3(x, y, 0), Quaternion.identity);
                grid.transform.parent = transform;
            }
        }
    }

    public void addTrack(Track track)
    {
        if (track == null)
        {
            Debug.Log("Track is null");
            return;
        }
        tracks.Add(track);
        CreateSpline(track.GetNodes().Item1.position, track.GetNodes().Item2.position, track.GetNodes().Item1.direction, track.GetNodes().Item2.direction, splineObject);
    }

    public bool CreateNodeGroup(Vector2Int position, Direction alignment, Node[] nodes)
    {
        if (nodeGroups.ContainsKey(position))
        {
            if (nodeGroups[position].GetAlignment() == alignment)
            {
                foreach (Node node in nodes)
                {
                    nodeGroups[position].AddNode(node);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        NodeGroup nodeGroup = new NodeGroup(alignment, position);
        nodeGroups.Add(position, nodeGroup);
        foreach (Node node in nodes)
        {
            nodeGroup.AddNode(node);
        }
        //Instantiate(railPrefab, new Vector3(position.x, position.y, 0), Quaternion.Euler(0, 0, (int)alignment * 45));
        return true;
    }

    public void CreateSpline(Vector2Int start, Vector2Int end, Direction startDirection, Direction endDirection, GameObject splineObject)
    {
        Vector2Int startVector = VectorFromDirection(startDirection);
        Vector2Int endVector = VectorFromDirection(endDirection);

        float bezierTangentLength = Mathf.Min(2, Vector2Int.Distance(start, end) / 3);
        Debug.Log("Bezier tangent length: " + bezierTangentLength);
        Spline spline = new();

        Vector3 startTangent = new Vector3(startVector.x, 0, startVector.y) * bezierTangentLength;
        Vector3 endTangent = new Vector3(endVector.x, 0, endVector.y) * -bezierTangentLength;

        Quaternion knotRotation = Quaternion.LookRotation(Vector3.up);
        spline.Add(new BezierKnot(new Vector3(start.x, start.y, 0), startTangent, startTangent, knotRotation));
        spline.Add(new BezierKnot(new Vector3(end.x, end.y, 0), endTangent, endTangent, knotRotation));

        splineObject.GetComponent<SplineContainer>().AddSpline(spline);
        splineObject.GetComponent<SplineInstantiate>().UpdateInstances();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DebugGrid();
        }

        // draw line for all tracks
        foreach (Track track in tracks)
        {
            track.DrawTrack();
        }
    }

    void DebugGrid(){
        foreach (KeyValuePair<Vector2Int, NodeGroup> nodeGroup in nodeGroups)
        {
            Debug.Log("NodeGroup at " + nodeGroup.Key + " with alignment " + nodeGroup.Value.GetAlignment() + " has " + nodeGroup.Value.GetNodes());
        }

        foreach (Track track in tracks)
        {
            track.DebugTrack();
        }
    }

    public static Direction DirectionFromVector(Vector2Int vector)
    {
        if (vector.x == 0 && vector.y == 1)
        {
            return Direction.NORTH;
        }
        else if (vector.x == -1 && vector.y == 1)
        {
            return Direction.NORTHWEST;
        }
        else if (vector.x == -1 && vector.y == 0)
        {
            return Direction.WEST;
        }
        else if (vector.x == -1 && vector.y == -1)
        {
            return Direction.SOUTHWEST;
        }
        else if (vector.x == 0 && vector.y == -1)
        {
            return Direction.SOUTH;
        }
        else if (vector.x == 1 && vector.y == -1)
        {
            return Direction.SOUTHEAST;
        }
        else if (vector.x == 1 && vector.y == 0)
        {
            return Direction.EAST;
        }
        else if (vector.x == 1 && vector.y == 1)
        {
            return Direction.NORTHEAST;
        }
        else
        {
            return Direction.NONE;
        }
    }

    public static Vector2Int AngleToVector(float angle)
    {
        float radians = angle * (Mathf.PI / 180);

        double x = Mathf.Cos(radians);
        double y = Mathf.Sin(radians);

        return new Vector2Int(Mathf.RoundToInt((float)x), Mathf.RoundToInt((float)y));
    }

    public static Vector2Int VectorFromDirection(Direction direction)
    {
        return direction switch
        {
            Direction.NORTH => new Vector2Int(0, 1),
            Direction.NORTHWEST => new Vector2Int(-1, 1),
            Direction.WEST => new Vector2Int(-1, 0),
            Direction.SOUTHWEST => new Vector2Int(-1, -1),
            Direction.SOUTH => new Vector2Int(0, -1),
            Direction.SOUTHEAST => new Vector2Int(1, -1),
            Direction.EAST => new Vector2Int(1, 0),
            Direction.NORTHEAST => new Vector2Int(1, 1),
            _ => new Vector2Int(0, 0),
        };
    }
}
