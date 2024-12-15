using System.Collections.Generic;
using UnityEngine;

public class Track
{
    private (Node, Node) nodes;
    private List<Track> crossedTracks = new List<Track>();

    public Track(Vector2Int start, Vector2Int end, Direction startDirection, Direction endDirection)
    {
        // calculate crossings
        // make splines
        nodes = (new(start, startDirection), new(end, endDirection));
        nodes.Item1.SetSibling(nodes.Item2);
        nodes.Item2.SetSibling(nodes.Item1);
        GridManager.Instance.CreateNodeGroup(start, startDirection, new Node[] { nodes.Item1 });
        GridManager.Instance.CreateNodeGroup(end, endDirection, new Node[] { nodes.Item2 });
    }

    public void DebugTrack(){
        Debug.Log("Track from " + nodes.Item1.position + " to " + nodes.Item2.position);
    }

    public void DrawTrack(){
        Debug.DrawLine(new Vector3(nodes.Item1.position.x, nodes.Item1.position.y, 0), new Vector3(nodes.Item2.position.x, nodes.Item2.position.y, 0), Color.red);
    }

    public (Node, Node) GetNodes(){
        return nodes;
    }

    public static (float, float) GetAngles(Direction direction, Vector2Int start, Vector2Int end, float maxCurveAngle){
        Vector2Int angle = GridManager.VectorFromDirection(direction);
        float distance = Vector2Int.Distance(start, end);  
        float maxAngle = distance * maxCurveAngle;
        float angleBetween = Vector2.SignedAngle(angle, end - start);
        // Debug.Log("Angle between " + angleBetween + " max angle " + maxAngle);
        return (angleBetween, maxAngle);
    }
}