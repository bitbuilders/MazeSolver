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

        public bool Contains(Node node)
        {
            bool contains = false;

            if (A == node || B == node)
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

        public bool EqualsReverse(Edge edge)
        {
            return A == edge.B && B == edge.A;
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

        public static void BuildKruskalMST(AdjacencyList list, out string result)
        {
            List<List<Node>> trees = CreateTrees(list);
            List<List<Edge>> mstTrees = CreateEdgesForTrees(trees);
            foreach (var edges in mstTrees)
            {
                SortEdges(edges);
            }
            
            List<List<Edge>> chosenEdges = new List<List<Edge>>();

            for (int i = 0; i < mstTrees.Count; ++i)
            {
                List<Edge> tree = mstTrees[i];
                List<Edge> chosen = new List<Edge>();
                chosenEdges.Add(chosen);
                TraverseMST(mstTrees[i][0], 0, ref tree, ref chosen);
            }

            List<string> hubs = new List<string>();
            foreach (var tree in chosenEdges)
            {
                GetHubPlacements(ref hubs, tree);
            }

            string output = MSTTreesToString(chosenEdges, hubs);

            result = output;
        }

        private static void GetHubPlacements(ref List<string> hubs, List<Edge> tree)
        {
            double smallestDiff = double.MaxValue;
            Node node = null;
            for (int i = 0; i < tree.Count; i++)
            {
                //
            }

            if (node != null)
            {
                hubs.Add($"{node.Tag} (Diff = {smallestDiff})");
            }
        }

        private static bool IsLeaf(Node node, List<Edge> tree)
        {
            int neighbors = 0;
            foreach (Edge edge in tree)
            {
                if (edge.Contains(node))
                {
                    neighbors++;
                }
            }

            return neighbors == 1;
        }

        private static string MSTTreesToString(List<List<Edge>> trees, List<string> hubs)
        {
            StringBuilder sb = new StringBuilder();
            int treeCount = 1;
            foreach (var tree in trees)
            {
                List<Node> nodes = new List<Node>();
                double weight = 0.0;
                sb.Append($"\n---MST Tree {treeCount++}---\nNodes routed: ");
                if (tree.Count > 0)
                {
                    nodes.Add(tree[0].A);
                    nodes.Add(tree[0].B);
                    sb.Append($"{tree[0].A.Tag}, {tree[0].B.Tag}");
                    weight += tree[0].Length;
                }

                for (int i = 1; i < tree.Count; i++)
                {
                    Edge edge = tree[i];
                    if (!nodes.Contains(edge.A))
                    {
                        nodes.Add(edge.A);
                        sb.Append($", {edge.A.Tag}");
                    }
                    if (!nodes.Contains(edge.B))
                    {
                        nodes.Add(edge.B);
                        sb.Append($", {edge.B.Tag}");
                    }
                    weight += edge.Length;
                }
                if (nodes.Count == 0)
                {
                    sb.Append("Could not be solved\n");
                }
                else
                {
                    sb.Append($"\nTotal length required: {weight} ft.\n");
                    sb.Append($"Optimal hub placement: {hubs[treeCount - 2]}");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }

        private static void TraverseMST(Edge current, int index, ref List<Edge> tree, ref List<Edge> chosen)
        {
            if (index >= tree.Count)
                return;

            chosen.Add(current);

            if (HasCycle(chosen))
            {
                chosen.Remove(current);
            }

            index++;
            if (index < tree.Count)
                TraverseMST(tree[index], index, ref tree, ref chosen);
        }

        private static bool HasCycle(List<Edge> tree)
        {
            bool hasCycle = false;

            List<Edge> path = new List<Edge>();
            TraverseCycle(tree[0], null, tree, ref path, ref hasCycle);

            return hasCycle;
        }

        private static void TraverseCycle(Edge current, Edge previous, List<Edge> tree, ref List<Edge> path, ref bool has)
        {
            if (path.Contains(current) || has)
            {
                has = true;
                return;
            }

            has = false;
            path.Add(current);

            List<Edge> nearby = GetAttachments(current, previous, tree);
            foreach (Edge e in nearby)
            {
                TraverseCycle(e, current, tree, ref path, ref has);
            }
        }

        private static List<Edge> GetAttachments(Edge edge, Edge previous, List<Edge> tree)
        {
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < tree.Count; i++)
            {
                if (tree[i] != edge && tree[i].Contains(edge) && tree[i] != previous)
                {
                    edges.Add(tree[i]);
                }
            }

            return edges;
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
                            if (e.Equals(edge) || e.EqualsReverse(edge))
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
