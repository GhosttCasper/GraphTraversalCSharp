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
    class Program
    {
        static void Main(string[] args)
        {
            Graph<int> graph = ReadFile("input.txt");
            int vertexIndex = ReadFileWithNumber("input2.txt");
            List<Vertex<int>> vertices = new List<Vertex<int>>();
            if (graph != null && !graph.IsEmpty())
                vertices = ProcessGraph(graph, vertexIndex);
            WriteFile(vertices, "output.txt");
        }

        private static List<Vertex<int>> ProcessGraph(Graph<int> graph, int index)
        {
            Vertex<int> vertex = new Vertex<int>(index);
            List<Vertex<int>> vertices = graph.BreadthFirstSearch(vertex);
            return vertices;
        }

        private static Graph<int> ReadFile(string fileName)
        {
            Graph<int> graph = new Graph<int>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                var numberStr = reader.ReadLine();
                if (numberStr == null)
                    throw new Exception("String is empty (ReadFile)");
                var array = numberStr.Split();
                int size = int.Parse(array[0]);
                string[] numbersStrs = new string[size];
                for (int i = 0; i < size; i++)
                {
                    numbersStrs[i] = reader.ReadLine();
                }
                graph = new Graph<int>(size, numbersStrs);
            }
            return graph;
        }

        private static int ReadFileWithNumber(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                var numberStr = reader.ReadLine();
                if (numberStr == null)
                    throw new Exception("String is empty (ReadFileWithNumber)");
                var array = numberStr.Split();
                int number = int.Parse(array[0]);
                return number;
            }
        }

        private static void WriteFile(List<Vertex<int>> vertices, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                HashSet<int> hashSet = new HashSet<int>();
                foreach (var vertex in vertices)
                {
                    hashSet.Add(vertex.Index);
                }
                string indexes = "";
                foreach (var index in hashSet)
                {
                    indexes += index.ToString();
                    indexes += " ";
                }
                writer.WriteLine(indexes);
            }
        }
    }
}
