using System.Collections.Generic;
using UnityEngine;

public class PathingScript
{
    private Dictionary<Node, (int, Node)> visitedNodes;
    //private int lowestCost;
    private Node startNode;
    private List<Node> destinationNodes;
    private Direction destinationDirection;

    /*
     * returns a list of nodes, that represent the shortest path from a given start node group to a destination node group
     * (takes into account direction)
     */
    private List<Node> GetPathNodes(NodeGroup startNodeGroup, Direction trainDirection, NodeGroup endNodeGroup) //TODO make this shit thread safe
    {
        lock (this) //lock instance, so simultaneous method calls can not mess with the data fields
        {
            
            visitedNodes = new Dictionary<Node, (int, Node)>();
            //lowestCost = int.MaxValue;
            startNode = null;
            destinationNodes = endNodeGroup.GetNodes();
            Debug.Assert(destinationNodes.Count > 0, "Error: No destination nodes set");
            destinationDirection = Direction.NONE;

            //get node with which we start, from node group:
            foreach (Node n in startNodeGroup.GetNodes())
            {
                if ((int)n.direction == ((int)trainDirection + 4) % 8)
                {
                    startNode = n;
                    break;
                }
            }
            Debug.Assert(startNode != null, "Error: No start node set");

            //call our recursive method
            Debug.Log("start recursive method");
            GoToNextNode(startNode, 0);
            Debug.Log("recursive method ended");

            //search dict for all destination nodes and pick the one with the lowest cost 
            int lowestDestinationCost = int.MaxValue;
            Node finalDestinationNode = null;
            foreach (Node n in destinationNodes)
            {
                if (visitedNodes.ContainsKey(n))
                    if (visitedNodes[n].Item1 < lowestDestinationCost)
                    {
                        lowestDestinationCost = visitedNodes[n].Item1;
                        finalDestinationNode = n;
                        destinationDirection = finalDestinationNode.direction;
                    }
            }
            if (finalDestinationNode == null)
                return null; //return null if no destination is found
            
            //once recursive method finished, just go from the destination node back to the start using the previous node entry,
            //and this will then create our path
            List<Node> path = new();
            Node pathNode = finalDestinationNode;
            Debug.Log("Path Nodes:");
            while (pathNode != startNode)
            {
                path.Add(pathNode);
                Debug.Log(pathNode.ToString());
                pathNode = visitedNodes[pathNode].Item2;
            }
            //last node (start node) was missing when testing -> we decided, that we dont need it
            return path;
        }
    }

    public List<Node> GetPath(NodeGroup start, Direction trainDirection, NodeGroup end)
    {
        List<Node> pathNodes = GetPathNodes(start, trainDirection, end);
        if (pathNodes == null)
            return null;
        pathNodes.Reverse();
        return pathNodes;
    }

    public Direction GetDestinationDirection()
    {
        return GridManager.FlipDirection(destinationDirection);
    }

    private void GoToNextNode(Node node, int cost)
    {
        if (node == null)
            return;
        Debug.Log("Current Node: " + node.ToString() + ", Current Cost: " + cost);
        
        //check if done/end etc.
        //TODO this was used to improve performance, by not following a path when more expensive, but did not work the fist time around - maybe implament later
        /*
        if (cost < lowestCost)
            lowestCost = cost;
        else
            return;
        */

        foreach (Node n in destinationNodes)
            if (n == node)
                return;

        foreach (Node transitionNode in node.GetTransitions())
        {
            Debug.Log("Looking at Transition Node: " + transitionNode.ToString());
            Node nextNode = transitionNode.GetSibling();
            Debug.Assert(nextNode != null, "Sibling of node is null");

            //look for the next node in our visited nodes list:
            if (visitedNodes.ContainsKey(nextNode))
            {
                //is costs of current path are lower, replace it in list and continue:
                if (cost < visitedNodes[nextNode].Item1)
                {
                    //TODO update entry, instead of remove and add
                    visitedNodes.Remove(nextNode);
                    visitedNodes.Add(nextNode, (cost + 1, node));
                    Debug.Log("recursive method call");
                    GoToNextNode(nextNode, cost + 1);
                }
                //if cost of current path is higher or equal, then just abort
                else
                {
                    Debug.Log("return from recursive method call");
                    return;
                }
            }
            else
            {
                //if it does not exist just create a new entry
                visitedNodes.Add(nextNode, (cost + 1, node));
                Debug.Log("recursive method call");
                GoToNextNode(nextNode, cost + 1);
            }
        }
    }
}
