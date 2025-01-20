using System.Collections.Generic;
using UnityEngine;

public class NodeGroup
{
    private bool hasSignal;
    private List<Node> nodes = new List<Node>();
    private Station station;
    private Direction alignment;
    private Vector2Int position;

    public NodeGroup(Direction alignment, Vector2Int position)
    {
        this.alignment = alignment;
        this.position = position;
    }

    public void AddNode(Node node)
    {
        nodes.Add(node);
        node.SetNodeGroup(this);
        foreach (Node other in nodes){
            if((int)node.direction == ((int)other.direction + 4) % 8){
                node.AddTransition(other);
                other.AddTransition(node);
            }
        }
    }

    public void SetStation(Station station)
    {
        this.station = station;
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public bool HasStation()
    {
        return station != null;
    }

    public Station GetStation()
    {
        return station;
    }
    
    public void RemoveNode(Node node)
    {
        nodes.Remove(node);
    }

    public Direction GetAlignment()
    {
        return alignment;
    }

    public List<Node> GetNodes()
    {
        return nodes;
    }

    public override string ToString()
    {
        return "NodeGroup: " + position + " " + alignment + " Nodes: " + nodes.Count;
    }
}