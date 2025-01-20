using System.Collections.Generic;

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
            destinationDirection = Direction.NONE;
        
            //get node with witch we start, from node group:
            foreach (Node n in startNodeGroup.GetNodes())
            {
                if(n.GetTransitions()[0].direction == trainDirection)
                    startNode = n;
            }
        
            //call our recursive method
            GoToNextNode(startNode, 0);
        
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
                    }
            }
            //once recursive method finished, just go from the destination node back to the start using the previous node entry,
            //and this will then create our path
            List<Node> path = new List<Node>();
            Node pathNode = finalDestinationNode;
            destinationDirection = finalDestinationNode.direction;
            while (pathNode != startNode)
            {
                path.Add(pathNode);
                pathNode = visitedNodes[pathNode].Item2;
            }
            return path;
        }
    }

    public List<NodeGroup> GetPath(NodeGroup start, Direction trainDirection, NodeGroup end)
    {
        List<Node> pathNodes = GetPathNodes(start, trainDirection, end);
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
        //check if done/end etc.
        if (cost < lowestCost)
            lowestCost = cost;
        else
            return;

        foreach (Node n in destinationNodes)
            if (n == node)
                return;
        
        foreach (Node transitionNode in node.GetTransitions())
        {   
            Node nextNode = transitionNode.GetSibling();
            
            //look for the next node in our visited nodes list:
            if (visitedNodes.ContainsKey(nextNode))
            {
                //is costs of current path are lower, replace it in list and continue:
                if (cost < visitedNodes[nextNode].Item1)
                {
                    //TODO update entry, instead of remove and add
                    visitedNodes.Remove(nextNode);
                    visitedNodes.Add(nextNode, (cost + 1, node));
                    GoToNextNode(nextNode, cost + 1);
                }
                //if cost of current path is higher or equal, then just abort
                else
                {
                    return;
                }
            }
            else
            {
                //if it does not exist just create a new entry
                visitedNodes.Add(nextNode, (cost + 1, node));
                GoToNextNode(nextNode, cost + 1);
            }
        }
    }
    
}
