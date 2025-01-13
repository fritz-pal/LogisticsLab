namespace dijkstra
{
    public class Edge
    {
        public string To { get; }
        public int Cost { get; }

        public Edge(string to, int cost)
        {
            To = to;
            Cost = cost;
        }
    }

}