using System.Collections.Generic;
using UnityEngine;

public class Track
{
    private (Node, Node) nodes;
    private List<Track> crossedTracks = new List<Track>();

    public Track(Vector2Int start, Vector2Int end, Direction direction)
    {
        // calculate end direction
        // calculate crossings
        // make splines
        Direction endDirection = direction;
        nodes = (new(start, direction), new(end, endDirection));
        nodes.Item1.SetSibling(nodes.Item2);
        nodes.Item2.SetSibling(nodes.Item1);
        GridManager.Instance.CreateNodeGroup(start, direction, new Node[] { nodes.Item1 });
        GridManager.Instance.CreateNodeGroup(end, endDirection, new Node[] { nodes.Item2 });
    }

    public void DebugTrack(){
        Debug.Log("Track from " + nodes.Item1.position + " to " + nodes.Item2.position);
    }

    public void DrawTrack(){
        Debug.DrawLine(new Vector3(nodes.Item1.position.x, nodes.Item1.position.y, 0), new Vector3(nodes.Item2.position.x, nodes.Item2.position.y, 0), Color.red);
    }
}