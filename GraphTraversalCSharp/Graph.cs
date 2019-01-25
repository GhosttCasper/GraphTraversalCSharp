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
    public class Graph //<T> where T : IComparable
    {
        public List<List<Vertex>> AdjacencyList = new List<List<Vertex>>();
        public int Time;
        public int Size;

        public Graph(int size, string[] strs)
        {
            Size = size;
            try
            {
                foreach (var str in strs)
                {
                    List<Vertex> list = new List<Vertex>();
                    var array = str.Split();
                    if (!string.IsNullOrEmpty(str))
                        foreach (var item in array)
                        {
                            int intVar = int.Parse(item);
                            if (intVar > Size)
                                throw new Exception("The vertex is missing");
                            Vertex curVertex = new Vertex(intVar);
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

        /// <summary>
        /// Поиск в ширину. Сложность 0(V + Е)
        /// </summary>
        public List<Vertex> BreadthFirstSearch(Vertex source)
        {
            foreach (var list in AdjacencyList)
            {
                foreach (var vertex in list)
                {
                    vertex.IsDiscovered = false;
                    vertex.Distance = int.MinValue;
                    vertex.Parent = null;
                }
            }
            source.IsDiscovered = true;
            source.Distance = 0;
            source.Parent = null;

            List<Vertex> vertices = new List<Vertex>();
            vertices.Add(source);

            Queue<Vertex> queue = new Queue<Vertex>();
            queue.Enqueue(source);
            while (queue.Count != 0)
            {
                Vertex curVertex = queue.Dequeue();
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

        public string PrintPath(Vertex source, Vertex end, string str)
        {
            if (source == end)
            {
                str += source.Index + " ";
                return str;
            }
            if (end.Parent == null)
            {
                Console.WriteLine("Path from source to end is missing");
                return str;
            }
            PrintPath(source, end.Parent, str);
            str += end.Index + " ";
            return str;
        }

        /// <summary>
        /// Поиск в глубину. Сложность 0(V + Е).
        /// </summary>
        public List<Vertex> DepthFirstSearch()
        {
            List<Vertex> vertices = new List<Vertex>();
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
                        vertices = DFSVisit(vertex, vertices);
                }
            }

            return vertices;
        }

        private List<Vertex> DFSVisit(Vertex vertex, List<Vertex> vertices)
        {
            Time++;
            vertex.DiscoveryTime = Time;
            vertex.IsDiscovered = true;
            foreach (var curVertex in AdjacencyList[vertex.Index - 1])
            {
                if (curVertex.IsDiscovered == false)
                {
                    curVertex.Parent = vertex;
                    DFSVisit(curVertex, vertices);
                }
            }
            Time++;
            vertex.FinishingTime = Time;
            vertices.Add(vertex);
            return vertices;
        }

        private void DFSVisit(Vertex vertex)
        {
            Time++;
            vertex.DiscoveryTime = Time;
            vertex.IsDiscovered = true;
            foreach (var curVertex in AdjacencyList[vertex.Index - 1])
            {
                if (curVertex.IsDiscovered == false)
                {
                    curVertex.Parent = vertex;
                    DFSVisit(curVertex);
                }
            }
            Time++;
            vertex.FinishingTime = Time;
        }

        public void DepthFirstSearchForTransposedGraph()
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
            List<Vertex> vertices = new List<Vertex>(Size);
            foreach (var list in AdjacencyList)
                foreach (var vertex in list)
                    vertices.Add(vertex);

            vertices.Sort((first, second) => second.FinishingTime.CompareTo(first.FinishingTime));
            foreach (var vertex in vertices)
                if (vertex.IsDiscovered == false)
                    DFSVisit(vertex);
        }

        public List<Vertex> TopologicalSort()  // only for directed acyclic graph
        {
            return DepthFirstSearch();
        }

        public void StronglyConnectedComponents() //время ©( V + Е)
        {
            DepthFirstSearch();
            Graph transposedGraph = TransposeGraph();
            transposedGraph.DepthFirstSearchForTransposedGraph();

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

        public Graph TransposeGraph()
        {
            Graph transposedGraph = new Graph { Size = Size };
            for (int i = 0; i < Size; i++)
            {
                foreach (var vertex in AdjacencyList[i])
                {
                    transposedGraph.AdjacencyList[vertex.Index - 1].Add(vertex);
                }
            }

            return transposedGraph;
        }

        public bool IsEmpty()
        {
            return AdjacencyList.Count == 0;
        }
    }
}
