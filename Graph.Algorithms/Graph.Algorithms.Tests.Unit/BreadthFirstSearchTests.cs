using FluentAssertions;

namespace Graph.Algorithms.Tests.Unit;

public class BreadthFirstSearchTests
{
    // with a connected graph, you can visit every vertex from the source vertex 
    private static List<int> TraverseConnectedGraph(List<List<int>> adjacencyList)
    {
        var sourceVertex = 0; // src
        var vertexCount = adjacencyList.Count; // V
        var visitOrder = new List<int>(); // res
        var vertexDiscovered = new bool[vertexCount]; // visited
        BreadthFirstSearch(adjacencyList, sourceVertex, vertexDiscovered, visitOrder);
        return visitOrder;
    }

    // with a disconnected graph, you have to traverse the graph from every unvisited vertex
    private static List<int> TraverseDisconnectedGraph(List<List<int>> adjacencyList)
    {
        var vertexCount = adjacencyList.Count; // V
        var visitOrder = new List<int>(); // res
        var vertexDiscovered = new bool[vertexCount]; // visited
        for (var sourceVertex = 0; sourceVertex < vertexCount; sourceVertex++)
        {
            if (!vertexDiscovered[sourceVertex])
            {
                BreadthFirstSearch(adjacencyList, sourceVertex, vertexDiscovered, visitOrder);
            }
        }
        return visitOrder;
    }

    private static void BreadthFirstSearch(
        List<List<int>> adj, int sourceVertex, bool[] discovered, List<int> visitOrder)
    {
        var verticesToBeVisited = new Queue<int>(); // q
        discovered[sourceVertex] = true;
        verticesToBeVisited.Enqueue(sourceVertex);
        while (verticesToBeVisited.Count > 0)
        {
            var currentVertex = verticesToBeVisited.Dequeue();
            visitOrder.Add(currentVertex);
            // visit all the unvisited
            // neighbours of current node
            foreach (var vertex in adj[currentVertex])
            {
                if (discovered[vertex]) continue;
                discovered[vertex] = true;
                verticesToBeVisited.Enqueue(vertex);
            }
        }
    }

    public static IEnumerable<object[]> ConnectedGraphTestCases()
    {
        // graph:
        //   1-3
        //  /| |
        // 0 | |
        //  \| |
        //   2-4
        // expected:
        // 0, 1, 2, 3, 4
        yield return
        [
            new List<List<int>> { new() { 1, 2 }, new() { 0, 3 }, new() { 0, 4 }, new() { 1, 4 }, new() { 2, 3 } },
            new List<int> { 0, 1, 2, 3, 4 }
        ];
        // graph:
        // 1-2-4
        // |/|
        // 0 3
        // expected:
        // 0, 1, 2, 3, 4
        yield return
        [
            new List<List<int>> { new() { 1, 2 }, new() { 0, 2 }, new() { 0, 1, 3, 4 }, new() { 2 }, new() { 2 } },
            new List<int> { 0, 1, 2, 3, 4 }
        ];;
    }
    
    [Theory]
    [MemberData(nameof(ConnectedGraphTestCases))]
    public void TraverseConnectedGraph_ShouldReturnCorrectOrderOfTraversal(List<List<int>> adj, List<int> exp)
    {
        var res = TraverseConnectedGraph(adj);
        res.Should().Equal(exp);
    }
    
    public static IEnumerable<object[]> DisconnectedGraphTestCases()
    {
        // graph:
        // 1-2 4
        //   | |
        // 3-0 5
        // expected:
        // 0, 2, 3, 1, 4, 5
        yield return
        [
            new List<List<int>> { new() { 2, 3 }, new() { 2 }, new() { 0, 1 }, new() { 0 }, new() { 5 }, new() { 4 } },
            new List<int> { 0, 2, 3, 1, 4, 5 }
        ];
    }

    [Theory]
    [MemberData(nameof(DisconnectedGraphTestCases))]
    public void TraverseDisconnectedGraph_ShouldReturnCorrectOrderOfTraversal(List<List<int>> adj, List<int> exp)
    {
        var res = TraverseDisconnectedGraph(adj);
        res.Should().Equal(exp);
    }
}
