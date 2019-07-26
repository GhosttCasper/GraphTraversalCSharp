using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTraversalCSharp
{
    public class Vertex //<T> where T : IComparable
    {
        public int Index { get; }
        public bool IsDiscovered { get; set; } // Color
        public Vertex Parent { get; set; }
        public int Distance { get; set; }
        public int DiscoveryTime { get; set; } // timestamp
        public int FinishingTime { get; set; }

        public Vertex(int index)
        {
            Index = index;
        }
    }
}
