using System.Collections.Generic;
using UnityEngine;

public class PathingScript
{
    private Dictionary<Node, (int, Node)> visitedNodes;
    private int lowestCost;
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
            lowestCost = int.MaxValue;
            startNode = null;
            destinationNodes = endNodeGroup.GetNodes();
            Debug.Assert(destinationNodes.Count > 0, "Error: No destination nodes set");
            destinationDirection = Direction.NONE;

            //get node with which we start, from node group:
            Debug.Log("Train direction: " + trainDirection);
            Debug.Log(startNodeGroup.ToString());
            foreach (Node n in startNodeGroup.GetNodes())
            {
                Debug.Log("Direction: " + n.direction);
                if ((int)n.direction == ((int)trainDirection + 4) % 8)
                {
                    startNode = n;
                    break;
                }
            }
            
            //Debug.Assert(startNode != null, "Error: No start node set");
            Debug.Log("Start Node: " + startNode.ToString());

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
            
            //TODO make sure case of no path found is handled correctly - what does the recoursive method do in that case???
            if (finalDestinationNode == null)
                return null; //return null if no destination is found
            
            //once recursive method finished, just go from the destination node back to the start using the previous node entry,
            //and this will then create our path
            List<Node> path = new List<Node>();
            Node pathNode = finalDestinationNode;
            Debug.Log("Path Nodes:");
            while (pathNode != startNode)
            {
                path.Add(pathNode);
                Debug.Log(pathNode.ToString());
                pathNode = visitedNodes[pathNode].Item2;
            }
            return path;
        }
    }

    public List<NodeGroup> GetPath(NodeGroup start, Direction trainDirection, NodeGroup end)
    {
        List<Node> pathNodes = GetPathNodes(start, trainDirection, end);
        if (pathNodes == null)
            return null;
        List<NodeGroup> path = new List<NodeGroup>();
        foreach (Node n in pathNodes)
        {
            if (path.Count == 0 || (path.Count > 0 && path[0] != n.GetNodeGroup()))
                path.Insert(0, n.GetNodeGroup());
        }
        return path;
    }

    public Direction GetDestinationDirection()
    {
        return destinationDirection;
    }

    private void GoToNextNode(Node node, int cost)
    {
        if (node == null)
            return;
        Debug.Log("Current Node: " + node.ToString() + ", Current Cost: " + cost);
        
        //check if done/end etc.
        //TODO
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
                    Debug.Log("recursive methodcall");
                    GoToNextNode(nextNode, cost + 1);
                }
                //if cost of current path is higher or equal, then just abort
                else
                {
                    Debug.Log("return from recursive method");
                    return;
                }
            }
            else
            {
                //if it does not exist just create a new entry
                visitedNodes.Add(nextNode, (cost + 1, node));
                Debug.Log("recursive methodcall");
                GoToNextNode(nextNode, cost + 1);
            }
        }
    }
}
