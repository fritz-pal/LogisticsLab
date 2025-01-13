namespace dijkstra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Graph
    {
        private Dictionary<string, List<Edge>> adjacencyList;

        public Graph()
        {
            adjacencyList = new Dictionary<string, List<Edge>>();
        }

        public void AddNode(string node)
        {
            if (!adjacencyList.ContainsKey(node))
            {
                adjacencyList[node] = new List<Edge>();
            }
        }

        public void AddEdge(string fromNode, string toNode, int cost)
        {
            if (!adjacencyList.ContainsKey(fromNode)) AddNode(fromNode);
            if (!adjacencyList.ContainsKey(toNode)) AddNode(toNode);

            adjacencyList[fromNode].Add(new Edge(toNode, cost));
        }

        public List<Edge> GetNeighbors(string node)
        {
            return adjacencyList.ContainsKey(node) ? adjacencyList[node] : new List<Edge>();
        }

        public Dictionary<string, int> Dijkstra(string startNode)
        {
            var distances = new Dictionary<string, int>();
            var priorityQueue = new PriorityQueue<string>();
            var visited = new HashSet<string>();

            foreach (var node in adjacencyList.Keys)
            {
                distances[node] = int.MaxValue;
            }
            distances[startNode] = 0;

            priorityQueue.Enqueue(startNode, 0);

            while (priorityQueue.Count > 0)
            {
                var (currentNode, currentDistance) = priorityQueue.Dequeue();

                if (visited.Contains(currentNode)) continue;
                visited.Add(currentNode);

                foreach (var edge in GetNeighbors(currentNode))
                {
                    var newDistance = currentDistance + edge.Cost;
                    if (newDistance < distances[edge.To])
                    {
                        distances[edge.To] = newDistance;
                        priorityQueue.Enqueue(edge.To, newDistance);
                    }
                }
            }

            return distances;
        }
    }
}