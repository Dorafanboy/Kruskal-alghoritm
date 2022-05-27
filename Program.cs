using System;
using System.Collections.Generic;
using System.IO;

namespace Siaod7
{
    public class Edge
    {
        public int U;
        public int V;
        public double Weight;
    }

    public class Kruskal
    {
        private const int MAX = 100;
        private int _edgesCount;
        private int _verticlesCount;
        private List<Edge> _edges;
        private int[,] _tree;
        private int[] _sets;

        public List<Edge> Edges => _edges;
        public int VerticlesCount => _verticlesCount;
        public List<Edge> SetEdges
        {
            set
            {
                _edges = value;
            }
        }

        public Kruskal(StreamReader reader)
        {
            _tree = new int[MAX, 3];
            _sets = new int[MAX];

            char[] splits = new char[] { ' ', ',', '\t' };
            string line = reader.ReadLine();
            string[] lines = line.Split(splits, StringSplitOptions.RemoveEmptyEntries);
            _verticlesCount = int.Parse(lines[0]);
            _edgesCount = int.Parse(lines[1]);
           
            _edges = new List<Edge>();

            _edges.Add(null);

            for (int e = 1; e <= _edgesCount; ++e)
			{
                line = reader.ReadLine();
                lines = line.Split(splits, StringSplitOptions.RemoveEmptyEntries);

                _edges.Add(new Edge
                {
                    U = int.Parse(lines[0]),
                    V = int.Parse(lines[1]),
                    Weight = double.Parse(lines[2])
                });
            }

            for (int i = 1; i <= _verticlesCount; i++)
                _sets[i] = i;
        }

        private void ArrangeEdges(int k)
        {
            Edge temp;
            for (int i = 1; i < k; i++)
            {
                for (int j = 1; j <= k - i; j++)
                {
                    if (_edges[j].Weight > _edges[j + 1].Weight)
                    {
                        temp = _edges[j];
                        _edges[j] = _edges[j + 1];
                        _edges[j + 1] = temp;
                    }
                }
            }
        }

        public int Find(int x)
        {
            if (_sets[x] == x)
                return x;
            return _sets[x] = Find(_sets[x]);
        }

        private void Join(int v1, int v2)
        {
            if (v1 < v2)
                _sets[v2] = v1;
            else
                _sets[v1] = v2;
        }

        public void BuildSpanningTree()
        {
            int k = _edgesCount;
            int i, t = 1;
            this.ArrangeEdges(k);
            for (i = 1; i <= k; i++)
            {
                for (i = 1; i < k; i++)
                {
                    if (this.Find(_edges[i].U) != this.Find(_edges[i].V))
                    {
                        _tree[t, 0] = (int)_edges[i].Weight;
                        _tree[t, 1] = _edges[i].U;
                        _tree[t, 2] = _edges[i].V;
                        this.Join(Find(_edges[i].U), Find(_edges[i].V));
                        t++;
                    }
                }
            }
        }

        public void DisplayInfo()
        {
            Console.WriteLine("Рёбрами минимального остовного дерева являются:");
            for (int i = 1; i < _verticlesCount; i++)				
                Console.WriteLine("{0}--({1})--{2}", ToChar(_tree[i, 1]), _tree[i, 0], ToChar(_tree[i, 2]));
        }

        private static char ToChar(int u)
        {
            return (char)(u + 64);
        }
    }

    public class Program
    {
        public static void GraphVizualization(StreamReader reader)
        {
            int u, v;
            int w, e;

            char[] splits = new char[] { ' ', ',', '\t' };
            string line = reader.ReadLine();
            string[] parts = line.Split(splits, StringSplitOptions.RemoveEmptyEntries);

            int V = int.Parse(parts[0]);
            int E = int.Parse(parts[1]);
            Console.WriteLine("Количество вершин: " + V);
            Console.WriteLine("Количество ребер: " + E);

            for (e = 1; e <= E; ++e)
            {
                line = reader.ReadLine();
                parts = line.Split(splits, StringSplitOptions.RemoveEmptyEntries);
                u = int.Parse(parts[0]);
                v = int.Parse(parts[1]);
                w = int.Parse(parts[2]);

                Console.WriteLine("{0}--({1})--{2}", ToChar(u), w, ToChar(v));
            }
            Console.Write("\n");
            reader.Close();

        }

        private static char ToChar(int u)
        {
            return (char)(u + 64);
        }

        static void Main(string[] args)
        {
            string fname = "graph.txt";
            StreamReader sr = new StreamReader(fname);
            GraphVizualization(sr);
            StreamReader sr1 = new StreamReader(fname);
            Kruskal kruskal = new Kruskal(sr1);

            kruskal.BuildSpanningTree();
            kruskal.DisplayInfo();
        }
    }
}