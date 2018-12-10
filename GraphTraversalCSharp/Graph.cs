using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
12. Граф задан связными списками. Перечислить вершины, в которые можно попасть из заданной.
*/

namespace GraphTraversalCSharp
{
    public class Graph<T> where T : IComparable
    {
        public List<List<Vertex<T>>> AdjacencyList = new List<List<Vertex<T>>>();
        public int Time;
        public int Size;

        public Graph(int size, string[] strs)
        {
            Size = size;
            try
            {
                foreach (var str in strs)
                {
                    List<Vertex<T>> list = new List<Vertex<T>>();
                    var array = str.Split();
                    if (!string.IsNullOrEmpty(str))
                        foreach (var item in array)
                        {
                            int intVar = int.Parse(item);
                            if (intVar > Size)
                                throw new Exception("The vertex is missing");
                            Vertex<T> curVertex = new Vertex<T>(intVar);
                            list.Add(curVertex);
                        }
                    AdjacencyList.Add(list);
                }
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is FormatException)
                {
                    Console.WriteLine("String is empty (Graph)");
                }
            }
        }

        public Graph()
        {
        }

        public List<Vertex<T>> BreadthFirstSearch(Vertex<T> source)
        {
            foreach (var list in AdjacencyList)
            {
                foreach (var vertex in list)
                {
                    vertex.IsDiscovered = false;
                    vertex.Distance = -1;
                    vertex.Parent = null;
                }
            }
            source.IsDiscovered = true;
            source.Distance = 0;
            source.Parent = null;

            List<Vertex<T>> vertices = new List<Vertex<T>>();
            vertices.Add(source);

            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            queue.Enqueue(source);
            while (queue.Count != 0)
            {
                Vertex<T> curVertex = queue.Dequeue();
                foreach (var vertex in AdjacencyList[curVertex.Index - 1])
                {
                    if (vertex.IsDiscovered == false)
                    {
                        vertex.IsDiscovered = true;
                        vertex.Distance = curVertex.Distance++;
                        vertex.Parent = curVertex;
                        queue.Enqueue(vertex);
                        vertices.Add(vertex);
                    }
                }
            }
            return vertices;
        }

        public void PrintPath(Vertex<T> source, Vertex<T> end)
        {
            if (source == end)
                Console.WriteLine(source);
            else if (source.Parent == null)
                Console.WriteLine("Path from source to end missing");
            else
            {
                PrintPath(source, end.Parent);
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
                    vertex.Parent = null;
                }
            }
            Time = 0;
            foreach (var list in AdjacencyList)
            {
                foreach (var vertex in list)
                {
                    if (vertex.IsDiscovered == false)
                        DfsVisit(vertex);
                }
            }
        }

        private void DfsVisit(Vertex<T> vertex)
        {
            Time++;
            vertex.DiscoveryTime = Time;
            vertex.IsDiscovered = true;
            foreach (var curVertex in AdjacencyList[vertex.Index])
            {
                if (curVertex.IsDiscovered == false)
                {
                    curVertex.Parent = vertex;
                    DfsVisit(curVertex);
                }
            }
            Time++;
            vertex.FinishingTime = Time;
        }

        public void StronglyConnectedComponents()
        {
            /*
             * 1 Вызов DFS(G) для вычисления времен завершения u.f
               для каждой вершины и
               2 Вычисление GT
               3 Вызов DFS(GT), но в основном цикле процедуры DFS
               вершины рассматриваются в порядке убывания значений u.f, вычисленных в строке 1
               4 Вывод вершин каждого дерева в лесу поиска в глубину,
               полученного в строке 3, в качестве отдельного сильно связного компонента
             */
        }

        public bool IsEmpty()
        {
            return AdjacencyList.Count == 0;
        }
    }
}
