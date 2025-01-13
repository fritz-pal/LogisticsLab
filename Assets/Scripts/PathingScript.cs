using System.Collections.Generic;
using UnityEngine;

public class PathingScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * returns a list of positions which then can be used to create a spline
     */
    public List<Vector2Int> GetPath(NodeGroup startNodeGroup, NodeGroup endNodeGroup)
    {
        //nodegroups are dijkstra-nodes, we iterate through our train network
        //costs between nodes are just number of tracks, or for the start its just always 1 between nodegroups,
        //since the distances are somewhat similar
        
        
        //look for unvisited node (if none can be found end it)
        //if unvisited node found, calculate 
        
        

        return null;
    }
    
}
