using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            RestartProgram();
        }

        static void RestartProgram()
        {
            string file = GetFileFromUser();
            List<AdjacencyList> lists = new List<AdjacencyList>();
            ParseMazes(file, lists);
            
            foreach (AdjacencyList list in lists)
            {
                Console.WriteLine(list);
            }
        }

        static void ParseMazes(string filePath, List<AdjacencyList> lists)
        {
            string[] lines = File.ReadAllLines(filePath);

            List<string> nodeLines = new List<string>();
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i] == "" || char.IsLetter(lines[i][0])) 
                {
                    nodeLines.Add(lines[i]);
                }
            }

            foreach (string s in lines)
            {
                //Console.WriteLine(s);
            }

            AdjacencyList currentMaze = new AdjacencyList();
            List<Node> currentNodes = new List<Node>();
            int previousMazeCounts = 0;
            for (int i = 0; i < nodeLines.Count; ++i)
            {
                if (i - previousMazeCounts == 0)
                {
                    string[] nodes = nodeLines[i].Split(',');
                    for (int j = 0; j < nodes.Length; ++j)
                    {
                        Node n = new Node(nodes[j]);
                        currentNodes.Add(n);
                    }
                }
                else if (i - previousMazeCounts == 1)
                {
                    string[] endPoints = nodeLines[i].Split(',');
                    currentMaze.Start = endPoints[0];
                    currentMaze.Start = endPoints[1];
                }
                else
                {
                    string[] nodes = nodeLines[i].Split(',');
                    Node node = null;
                    for (int x = 0; x < currentNodes.Count; ++x)
                    {
                        if (nodes[0] == currentNodes[x].Tag)
                        {
                            node = currentNodes[x];
                        }
                    }

                    for (int j = 1; j < nodes.Length; ++j)
                    {
                        for (int x = 0; x < currentNodes.Count; ++x)
                        {
                            if (nodes[j] == currentNodes[x].Tag)
                            {
                                node.Near.Add(currentNodes[x]);
                            }
                        }
                    }

                    if (node != null) currentMaze.List.Add(node);
                }
                if (nodeLines[i] == "" || i >= nodeLines.Count - 1)
                {
                    previousMazeCounts = i + 1;
                    lists.Add(currentMaze.Clone());
                    currentMaze.List.Clear();
                    currentNodes.Clear();
                }
            }
        }

        static string GetFileFromUser()
        {
            string filePath = "";

            do
            {
                filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("That's not a valid file path, please try again.\n");
                }
            } while (!File.Exists(filePath));

            return filePath;
        }
    }
}
