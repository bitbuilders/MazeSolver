using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class AdjacencyList
    {
        public List<Node> List { get; private set; }
        public string Start { get; set; }
        public string End { get; set; }

        public AdjacencyList()
        {
            List = new List<Node>();
        }

        public AdjacencyList Clone()
        {
            AdjacencyList list = new AdjacencyList();
            list.Start = Start;
            list.End = End;

            foreach (Node node in List)
            {
                list.List.Add(node);
            }

            return list;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Start: {Start}\n");
            sb.Append($"End: {End}\n");
            sb.Append("~~~~~~~~~~~~\n");
            foreach (Node n in List)
            {
                sb.Append(n.ToString());
            }
            sb.Append("~~~~~~~~~~~~\n");

            return sb.ToString();
        }
    }
}
