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
}