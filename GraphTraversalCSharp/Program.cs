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
            Graph graph = ReadFile("input.txt");
            int vertexIndex = ReadFileWithNumber("input2.txt");
            List<Vertex> vertices = new List<Vertex>();
            if (graph != null && !graph.IsEmpty())
                vertices = ProcessGraph(graph, vertexIndex);
            WriteFile(vertices, "output.txt");
        }

        private static List<Vertex> ProcessGraph(Graph graph, int index)
        {
            Vertex vertex = new Vertex(index);
            List<Vertex> vertices = graph.BreadthFirstSearch(vertex);
            return vertices;
        }

        private static Graph ReadFile(string fileName)
        {
            Graph graph = new Graph();
            using (StreamReader reader = new StreamReader(fileName))
            {
                int size = ReadNumber(reader);
                string[] numbersStrs = new string[size];
                for (int i = 0; i < size; i++)
                {
                    numbersStrs[i] = reader.ReadLine();
                }
                graph = new Graph(size, numbersStrs);
            }
            return graph;
        }

        private static int ReadFileWithNumber(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                return ReadNumber(reader);
            }
        }

        private static int ReadNumber(StreamReader reader)
        {
            var numberStr = reader.ReadLine();
            if (numberStr == null)
                throw new Exception("String is empty (ReadNumber)");
            var array = numberStr.Split();
            int number = int.Parse(array[0]);
            return number;
        }

        private static void WriteFile(List<Vertex> vertices, string fileName)
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
