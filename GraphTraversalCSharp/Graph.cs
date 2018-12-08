using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//resharper

namespace GraphTraversalCSharp
{
    public class Graph<T> where T : IComparable
    {
        public List<List<Vertex<T>>> AdjacencyList = new List<List<Vertex<T>>>();
        public int time;

        public void BreadthFirstSearch(Vertex<T> source)
        {
            foreach (var list in AdjacencyList)
            {
                foreach (var vertex in list)
                {
                    vertex.IsDiscovered = false;
                    vertex.distance = -1;
                    vertex.parent = null;
                }
            }
            source.IsDiscovered = true;
            source.distance = 0;
            source.parent = null;
            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            queue.Enqueue(source);
            while (queue.Count != 0)
            {
                Vertex<T> curVertex = queue.Dequeue();
                foreach (var vertex in AdjacencyList[curVertex.index])
                {
                    if (vertex.IsDiscovered == false)
                    {
                        vertex.IsDiscovered = true;
                        vertex.distance = curVertex.distance++;
                        vertex.parent = curVertex;
                        queue.Enqueue(vertex);
                    }
                }
            }
        }

        public void PrintPath(Vertex<T> source, Vertex<T> end)
        {
            if (source == end)
                Console.WriteLine(source);
            else if (source.parent == null)
                Console.WriteLine("Path from source to end missing");
            else
            {
                PrintPath(source, end.parent);
                Console.WriteLine(end);
            }
        }

        public void DepthFirstSearch()
        {
            foreach (var list in AdjacencyList)
            {
                foreach (var vertex in list)
                {
                    vertex.IsDiscovered = false;
                    vertex.parent = null;
                }
            }
            time = 0;
            foreach (var list in AdjacencyList)
            {
                foreach (var vertex in list)
                {
                    if (vertex.IsDiscovered == false)
                        DFSVisit(vertex);
                }
            }
        }

        private void DFSVisit(Vertex<T> vertex)
        {
            time++;
            vertex.discoveryTime = time;
            vertex.IsDiscovered = true;
            foreach (var curVertex in AdjacencyList[vertex.index])
            {
                if (curVertex.IsDiscovered == false)
                {
                    curVertex.parent = vertex;
                    DFSVisit(curVertex);
                }
            }
            time++;
            vertex.finishingTime = time;
        }
    }

    public class Vertex<T> where T : IComparable
    {
        public bool IsDiscovered;
        public Vertex<T> parent;
        public int distance;
        public int index;
        public int discoveryTime;
        public int finishingTime;
    }

    public class Edge<T> where T : IComparable
    {

    }
}
