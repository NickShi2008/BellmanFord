using GraphLibrary;
using System.Collections;

namespace GraphLibary.Tests
{
    public class NegativeCycleTest
    {
        public class GraphGenerator : IEnumerable<object[]>
        {
            public Graph<string> GenerateGraphPositive()
            {
                Graph<string> graph = new Graph<string>();

                for(int i = 0; i < 10; i++)
                {
                    graph.AddVertex(new Vertex<string>($"{i}"));
                }

                for (int i = 0; i < 10; i++)
                {
                    var currentVertex = graph.Search($"{i}");
                    
                    foreach(var ver in graph.Vertices)
                    {
                        graph.AddEdge(currentVertex, ver, 1);
                    }

                }

                return graph;
            }

            public Graph<string> GenerateGraphNegative()
            {
                Graph<string> graph = new Graph<string>();

                for (int i = 0; i < 10; i++)
                {
                    graph.AddVertex(new Vertex<string>($"{i}"));
                }

                for (int i = 0; i < 10; i++)
                {
                    var currentVertex = graph.Search($"{i}");

                    foreach (var ver in graph.Vertices)
                    {
                        graph.AddEdge(currentVertex, ver, -1);
                    }

                }

                return graph;
            }

            public List<object[]> Arr => [
            [GenerateGraphNegative(), "1", "2"]

             ];

            public IEnumerator<object[]> GetEnumerator() => Arr.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        [Theory]
        [ClassData(typeof(GraphGenerator))]
        public void Test1(Graph<string> graph, string start, string end)
        {
            
            bool check = graph.BellmanFord(graph.Search(start), graph.Search(end), out var path);

            Assert.False(check == true);

        }
    }
}