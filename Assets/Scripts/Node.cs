using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private List<Node> transitions = new();
    private Node sibling;
    public Vector2Int position;
    public Direction direction;
    private NodeGroup nodeGroup;
    private Track track;

    public Node(Vector2Int position, Direction direction)
    {
        this.position = position;
        this.direction = direction;
    }

    public void AddTransition(Node node)
    {
        transitions.Add(node);
    }

    public void SetTrack(Track track)
    {
        this.track = track;
    }

    public Track GetTrack()
    {
        return track;
    }

    public void RemoveTransition(Node node)
    {
        transitions.Remove(node);
    }

    public void SetSibling(Node node)
    {
        sibling = node;
    }

    public void SetNodeGroup(NodeGroup nodeGroup)
    {
        this.nodeGroup = nodeGroup;
    }

    public NodeGroup GetNodeGroup()
    {
        return nodeGroup;
    }

    public List<Node> GetTransitions()
    {
        return transitions;
    }

    public Node GetSibling()
    {
        return sibling;
    }

    public override string ToString()
    {
        return "Node: " + position + " " + direction;
    }
}