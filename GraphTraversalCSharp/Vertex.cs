using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTraversalCSharp
{
    public class Vertex<T> where T : IComparable
    {
        public bool IsDiscovered;
        public Vertex<T> Parent;
        public int Distance;
        public int Index;
        public int DiscoveryTime;
        public int FinishingTime;

        public Vertex(int index)
        {
            Index = index;
        }
    }
}
