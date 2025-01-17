using System.Collections.Generic;
using UnityEngine;

public class Node
{
    private List<Node> transitions;
    private Node sibling;
    public Vector2Int position;
    public Direction direction;

    public Node(Vector2Int position, Direction direction)
    {
        this.position = position;
        this.direction = direction;
    }

    public void AddTransition(Node node)
    {
        transitions.Add(node);
    }

    public void RemoveTransition(Node node)
    {
        transitions.Remove(node);
    }

    public void SetSibling(Node node)
    {
        sibling = node;
    }

    public List<Node> GetTransitions()
    {
        return transitions;
    }

    public Node GetSibling()
    {
        return sibling;
    }
}