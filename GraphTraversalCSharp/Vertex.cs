using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTraversalCSharp
{
    public class Vertex //<T> where T : IComparable
    {
        public bool IsDiscovered; // Color
        public Vertex Parent;
        public int Distance;
        public int Index;
        public int DiscoveryTime; // timestamp
        public int FinishingTime;

        public Vertex(int index)
        {
            Index = index;
        }
    }
}
