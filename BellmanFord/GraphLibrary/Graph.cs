
namespace GraphLibrary
{
    public class Graph<T> 
    {
        //private Grid<T> GridVis { get; set; }
        public List<Vertex<T>> Vertices { get; set; }
        public List<Edge<T>> Edges { get; set; }

        public int VertexCount => Vertices.Count;


        
        public Graph()
        {

            Vertices = new List<Vertex<T>>();
            
            Edges = new List<Edge<T>>();
        }

        
        public bool BellmanFord(Vertex<T> start, Vertex<T> end, out (List<Vertex<T>>, float cost) path)
        {
            Dictionary<Vertex<T>, float> totalDistances = new Dictionary<Vertex<T>, float>();
            List<Vertex<T>> visitedVertices = new List<Vertex<T>>();
            PriorityQueue<Vertex<T>, float> queuedDistances = new PriorityQueue<Vertex<T>, float>();
            Dictionary<Vertex<T>, (Vertex<T> founder, float cost)> map = [];

            foreach (var v in Vertices)
            {
                totalDistances.Add(v, float.PositiveInfinity);
            }

            //prepare start vertex
            totalDistances[start] = 0;

            queuedDistances.Enqueue(start, 0);

            for (int i = 0; i < VertexCount - 1; i++)
            {
               // while(queuedDistances.Count > 0)
               // {
                    Vertex<T> currentVertex = queuedDistances.Dequeue();

                    if(visitedVertices.Contains(currentVertex))
                    {
                        continue;
                    }

                    visitedVertices.Add(currentVertex);

                    foreach(var edge in currentVertex.Neighbors)
                    {
                        float distance = totalDistances[currentVertex] + edge.Distance;
                        if (distance < totalDistances[edge.EndingPoint])
                        {
                            totalDistances[edge.EndingPoint] = distance;

                            map[edge.EndingPoint] = (edge.StartingPoint, distance);

                            queuedDistances.Enqueue(edge.EndingPoint, totalDistances[edge.EndingPoint]);
                        }
                    }
              //  }
            }



            
            foreach (var edge in Vertices[VertexCount - 1].Neighbors)
            {
                float distance = totalDistances[Vertices[VertexCount - 1]] + edge.Distance;
                if (distance < totalDistances[edge.EndingPoint])
                {
                    path = default((List<Vertex<T>>, float));
                    return false;
                }
            }

            path = FindPath(start, end, map);

            return true;
        }

       
        public (List<Vertex<T>> path, float cost) FindPath(Vertex<T> start, Vertex<T> end, Dictionary<Vertex<T>, (Vertex<T> founder, float cost)> founderMap)
        {
            Stack<Vertex<T>> reversePath = new Stack<Vertex<T>>();
            var curr = end;
            float cost = 0;
            while (founderMap.ContainsKey(curr))
            {
                reversePath.Push(curr);
                cost += founderMap[curr].cost;
                curr = founderMap[curr].founder;
            }

            return (reversePath.ToList(), cost);
        }
        

        public void AddVertex(Vertex<T> vertex)
        {
            if (!SearchVertex(vertex) && vertex.NeighborCount == 0)
            {
                Vertices.Add(vertex);
            }
        }

        public bool RemoveVertex(Vertex<T> vertex)
        {
            if (Vertices.Contains(vertex))
            {
                foreach (Edge<T> edges in vertex.Neighbors)
                {
                    edges.EndingPoint.Neighbors.Remove(edges.EndingPoint.FindFirstEdge(vertex));
                    vertex.Neighbors.Remove(vertex.FindFirstEdge(edges.EndingPoint));
                }
                Vertices.Remove(vertex);
                return true;
            }
            return false;
        }

        private bool SearchVertex(Vertex<T> vertex)
        {
            bool check = Vertices.Contains(vertex);
            return vertex != null && Vertices.Contains(vertex);
        }

        public bool AddEdge(Vertex<T> a, Vertex<T> b, float distance)
        {
            if (SearchVertex(a) )
            {
                Edge<T> AConnector = new Edge<T>(a, b, distance);
                Edges.Add(AConnector);
                if (!a.Neighbors.Contains(AConnector))
                    a.Neighbors.Add(AConnector);

                return true;
            }
            return false;
        }

        public bool RemoveEdge(Vertex<T> a, Vertex<T> b)
        {
            if (SearchVertex(a) && SearchVertex(b) && a.HasEdge(b) && b.HasEdge(a))
            {
                a.Neighbors.Remove(a.FindFirstEdge(b));
                b.Neighbors.Remove(b.FindFirstEdge(a));
                return true;
            }
            return false;
        }

        public Vertex<T> Search(T vertex)
        {
            int count = -1;
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].Value.Equals(vertex))
                {
                    count = i;
                    break;
                }
            }

            if (count == -1)
            {
                return null;
            }
            return Vertices[count];
        }

        public Edge<T> GetEdge(Vertex<T> a, Vertex<T> b)
        {
            if (a != null && b != null && a.HasEdge(b) && b.HasEdge(a))
            {
                return a.FindFirstEdge(b);
            }
            return null;
        }


    }
}
