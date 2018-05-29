using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class AdjacentNode
    {
        public AdjacentNode(Node node, double weight)
        {
            Node = node;
            Weight = weight;
        }

        public Node Node { get; private set; }
        public double Weight { get; private set; }
    }

    public class Node
    {
        public List<AdjacentNode> Near { get; private set; }
        public string Tag { get; private set; }

        public Node(string tag)
        {
            Near = new List<AdjacentNode>();
            Tag = tag;
        }

        public void AddAdjacent(Node node, double weight)
        {
            Near.Add(new AdjacentNode(node, weight));
        }

        public static double GetTotalPathWeight(List<AdjacentNode> path)
        {
            double weight = 0.0;

            foreach (AdjacentNode node in path)
            {
                weight += node.Weight;
            }

            return weight;
        }

        public static string GetStringFromPath(List<AdjacentNode> path)
        {
            StringBuilder sb = new StringBuilder();
            if (path.Count == 0)
            {
                return "";
            }

            sb.Append(path[0].Node.Tag);

            for (int i = 1; i < path.Count; ++i)
            {
                sb.Append(", ");
                sb.Append(path[i].Node.Tag);
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Tag);
            sb.Append(" => ");
            if (Near.Count > 0)
            {
                sb.Append($"{Near[0].Node.Tag}~{Near[0].Weight}");
            }
            for (int i = 1; i < Near.Count; ++i)
            {
                sb.Append($", {Near[i].Node.Tag}~{Near[i].Weight}");
            }
            sb.Append("\n");

            return sb.ToString();
        }
    }
}
