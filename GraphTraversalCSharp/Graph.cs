using System;
using System.Collections.Generic;
using System.IO;
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
        private List<List<Vertex>> AdjacencyList;
        private List<Vertex> VerticesList;
        private int Time;
        public int Size { get; }

        public Graph(int size, string[] strs)
        {
            Size = size;
            VerticesList = new List<Vertex>();
            for (int i = 1; i <= Size; i++)
            {
                VerticesList.Add(new Vertex(i));
            }
            AdjacencyList = new List<List<Vertex>>();

            try
            {
                foreach (var str in strs)
                {
                    List<Vertex> list = new List<Vertex>();
                    var array = str.Split();
                    if (!string.IsNullOrEmpty(str))
                        foreach (var item in array)
                        {
                            int intVar = int.Parse(item) - 1;
                            if (intVar > Size)
                                throw new Exception("The vertex is missing");

                            Vertex curVertex = VerticesList[intVar];//vertex => vertex.Index == intVar
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
        public List<Vertex> BreadthFirstSearch(int sourceIndex)
        {
            foreach (var vertex in VerticesList)
            {
                vertex.IsDiscovered = false;
                vertex.Distance = int.MinValue;
                vertex.Parent = null;
            }

            Vertex source = VerticesList[sourceIndex - 1]; //Find(vertex => vertex.Index == sourceIndex)
            source.IsDiscovered = true;
            source.Distance = 0;
            source.Parent = null;

            List<Vertex> result = new List<Vertex> { source };

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
                        vertex.Distance = curVertex.Distance + 1;
                        vertex.Parent = curVertex;
                        queue.Enqueue(vertex);
                        result.Add(vertex);
                    }
                }
            }

            return result;
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
            List<Vertex> result = new List<Vertex>(Size);
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
                        result = DFSVisit(vertex, result);
                }
            }

            return result;
        }

        private List<Vertex> DFSVisit(Vertex vertex, List<Vertex> result)
        {
            Time++;
            vertex.DiscoveryTime = Time;
            vertex.IsDiscovered = true;

            foreach (var curVertex in AdjacencyList[vertex.Index - 1])
            {
                if (curVertex.IsDiscovered == false)
                {
                    curVertex.Parent = vertex;
                    DFSVisit(curVertex, result);
                }
            }

            Time++;
            vertex.FinishingTime = Time;
            result.Add(vertex);
            return result;
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

        public List<Vertex> TopologicalSort() // only for directed acyclic graph
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

        public void SaveTxtFormatGraph(string graphFile)
        {
            using (StreamWriter writer = new StreamWriter(graphFile))
            {
                writer.WriteLine(Size);
                writer.WriteLine(ToTxtFile());
            }
        }

        public string ToTxtFile()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < AdjacencyList.Count; i++)
            {
                for (int j = 0; j < AdjacencyList[i].Count; j++)
                {
                    sb.Append(i + 1 + " ");
                    sb.Append(AdjacencyList[i][j].Index);
                    sb.Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}