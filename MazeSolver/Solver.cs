using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class Edge : IComparable
    {
        public Edge(Node a, Node b, double length)
        {
            A = a;
            B = b;
            Length = length;
        }
        public double Length { get; }
        public Node A { get; }
        public Node B { get; }

        public bool Contains(Edge edge)
        {
            bool contains = false;

            if (edge.A == A || edge.B == A || edge.A == B || edge.B == B)
                contains = true;

            return contains;
        }

        public override bool Equals(object obj)
        {
            bool equal = false;

            Edge edge = (Edge)obj;
            if ((edge.A == A && edge.B == B) || (edge.A == B && edge.B == A))
                equal = true;

            return equal;
        }

        public int CompareTo(object obj)
        {
            return (int)(Length - ((Edge)obj).Length);
        }
    }

    public static class Solver
    {
        public static bool SolveShortest(AdjacencyList list, out string path)
        {
            path = "";

            List<AdjacentNode> listPath = new List<AdjacentNode>();
            for (int i = 0; i < list.List.Count; ++i)
            {
                if (list.List[i].Tag == list.Start)
                {
                    TraverseShortest(new AdjacentNode(list.List[i], 0.0f), new List<AdjacentNode>(), ref listPath, list.End);
                    break;
                }
            }

            path = Node.GetStringFromPath(listPath);

            return !string.IsNullOrEmpty(path);
        }

        private static void TraverseShortest(AdjacentNode current, List<AdjacentNode> path, ref List<AdjacentNode> route, string end)
        {
            if (path.Contains(current))
                return;

            path.Add(current);

            if (current.Node.Tag == end)
            {
                if (route.Count == 0 || Node.GetTotalPathWeight(path) < Node.GetTotalPathWeight(route))
                    route = path;
                return;
            }
            
            for (int i = 0; i < current.Node.Near.Count; ++i)
            {
                TraverseShortest(current.Node.Near[i], new List<AdjacentNode>(path), ref route, end);
            }
        }

        public static bool BuildKruskalMST(AdjacencyList list)
        {
            bool solved = false;

            List<Edge> edges = CreateEdges(list);
            SortEdges(edges);

            CreateTrees(edges);

            return solved;
        }

        static List<Edge> currentEdges;

        private static List<List<Edge>> CreateTrees(List<Edge> startingEdges)
        {
            currentEdges = startingEdges;
            List<List<Edge>> trees = new List<List<Edge>>();
            List<Edge> seen = new List<Edge>();

            TraverseTrees(currentEdges[0], seen);

            Console.WriteLine($"Seen: {seen.Count}, Actual: {startingEdges.Count}");
            if (seen.Count < currentEdges.Count)
            {

            }

            return trees;
        }

        private static void TraverseTrees(Edge current, List<Edge> seen)
        {
            if (seen.Contains(current))
                return;

            seen.Add(current);
            List<Edge> possibleEdges = GetPossibleEdges(current);
            for (int i = 0; i < possibleEdges.Count; ++i)
            {
                TraverseTrees(possibleEdges[i], seen);
            }
        }

        private static List<Edge> GetPossibleEdges(Edge edge)
        {
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < currentEdges.Count; ++i)
            {
                if (currentEdges[i].Contains(edge) && !edges.Contains(edge))
                {
                    edges.Add(edge);
                }
            }

            return edges;
        }

        private static List<Edge> CreateEdges(AdjacencyList list)
        {
            List<Edge> edges = new List<Edge>();
            foreach (Node node in list.List)
            {
                foreach (AdjacentNode adjNode in node.Near)
                {
                    Edge e = new Edge(node, adjNode.Node, adjNode.Weight);
                    bool contains = false;
                    foreach (Edge edge in edges)
                    {
                        if (edge.Equals(e))
                        {
                            contains = true;
                            break;
                        }
                    }
                    if (!contains)
                    {
                        edges.Add(e);
                    }
                }
            }

            return edges;
        }

        private static void SortEdges(List<Edge> edges)
        {
            edges.Sort();
        }
    }
}
