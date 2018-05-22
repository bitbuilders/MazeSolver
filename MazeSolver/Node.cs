using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    public class Node
    {
        public List<Node> Near { get; private set; }
        public string Tag { get; private set; }

        public Node(string tag)
        {
            Near = new List<Node>();
            Tag = tag;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Tag);
            sb.Append(" => ");
            foreach (Node n in Near)
            {
                sb.Append(n.Tag);
                sb.Append(",");
            }
            sb.Append("\n");

            return sb.ToString();
        }
    }
}
