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
            if ((edge.A == A && edge.B == B))
                equal = true;

            return equal;
        }

        public int CompareTo(object obj)
        {
            return (int)(Length - ((Edge)obj).Length);
        }

        public override string ToString()
        {
            return $"{A.Tag} <= {Length} => {B.Tag}";
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

        public static bool BuildKruskalMST(AdjacencyList list, out string result)
        {
            bool solved = false;

            List<List<Node>> trees = CreateTrees(list);
            List<List<Edge>> mstTrees = CreateEdgesForTrees(trees);
            foreach (var edges in mstTrees)
            {
                SortEdges(edges);
            }
            Console.WriteLine(trees.Count);
            foreach (List<Edge> tree in mstTrees)
            {
                foreach (Edge edge in tree)
                {
                    Console.WriteLine(edge);
                }
                Console.WriteLine();
            }

            StringBuilder sb = new StringBuilder();



            result = sb.ToString();

            return solved;
        }

        private static bool HasCycle(List<Edge> tree)
        {
            bool hasCycle = false;



            return hasCycle;
        }

        private static List<List<Node>> CreateTrees(AdjacencyList list)
        {
            List<List<Node>> trees = new List<List<Node>>();
            trees.Add(new List<Node>() { list.List[0] });

            for (int i = 1; i < list.List.Count; ++i)
            {
                Node currentNode = list.List[i];
                for (int j = 0; j < trees.Count; ++j)
                {
                    List<Node> tree = null;
                    foreach (Node node in trees[j])
                    {
                        bool hasRoute = false;
                        HasRouteTo(currentNode, node, new List<Node>(), ref hasRoute);
                        if (hasRoute)
                        {
                            tree = trees[j];
                            break;
                        }
                    }
                    if (tree != null)
                    {
                        tree.Add(currentNode);
                        break;
                    }
                    else if (j == trees.Count - 1 || trees == null)
                    {
                        trees.Add(new List<Node>() { currentNode });
                        break; // Don't really need this break
                    }
                }
            }

            return trees;
        }

        private static List<List<Edge>> CreateEdgesForTrees(List<List<Node>> trees)
        {
            List<List<Edge>> treeEdges = new List<List<Edge>>();

            foreach (List<Node> tree in trees)
            {
                treeEdges.Add(new List<Edge>());
                foreach (Node n in tree)
                {
                    foreach (AdjacentNode an in n.Near)
                    {
                        Edge edge = new Edge(n, an.Node, an.Weight);
                        bool contains = false;
                        foreach (Edge e in treeEdges[treeEdges.Count - 1])
                        {
                            if (e.Equals(edge))
                            {
                                contains = true;
                                break;
                            }
                        }
                        if (!contains)
                        {
                            treeEdges[treeEdges.Count - 1].Add(edge);
                        }
                    }
                }
            }

            return treeEdges;
        }

        private static void HasRouteTo(Node current, Node destination, List<Node> path, ref bool hasRoute)
        {
            if (path.Contains(current))
                return;

            path.Add(current);
            if (current == destination || hasRoute)
            {
                hasRoute = true;
                return;
            }
            
            foreach (AdjacentNode node in current.Near)
            {
                HasRouteTo(node.Node, destination, new List<Node>(path), ref hasRoute);
            }
        }

        private static void SortEdges(List<Edge> edges)
        {
            edges.Sort();
        }
    }
}
