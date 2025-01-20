using System.Collections.Generic;
using UnityEngine;

public class Station
{
    private Vector2Int position;
    private NodeGroup nodeGroup;
    private string name;
    private GameObject stationObject;
    
    
    public Station(Vector2Int position, NodeGroup nodeGroup, string name, GameObject stationObject)
    {
        this.position = position;
        this.name = name;
        this.stationObject = stationObject;
        this.nodeGroup = nodeGroup;
    }

    public string GetName()
    {
        return name;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public NodeGroup GetNodeGroup()
    {
        return nodeGroup;
    }

    public Vector2Int GetPosition()
    {
        return position;
    }

    public void SwitchDirection()
    {
        nodeGroup.SetAlignment(GridManager.FlipDirection(nodeGroup.GetAlignment()));
        Vector2Int vector = GridManager.VectorFromDirection((Direction)(((int)nodeGroup.GetAlignment() + 2) % 8));
        position = nodeGroup.GetPosition() - vector;
        stationObject.transform.SetPositionAndRotation(new Vector3(position.x, position.y, -0.2f), Quaternion.Euler((int)nodeGroup.GetAlignment() * -45, 90, -90));
    }
}