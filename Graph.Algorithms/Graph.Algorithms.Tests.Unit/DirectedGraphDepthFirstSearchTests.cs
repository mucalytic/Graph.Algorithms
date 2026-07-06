using FluentAssertions;

namespace Graph.Algorithms.Tests.Unit;

public class DirectedGraphDepthFirstSearchTests
{
    // with a connected graph, you can visit every vertex from the source vertex
    private static List<int> TraverseConnectedGraph(List<List<int>> adjacencyList, int src)
    {
        var visitOrder = new List<int>(); // res
        var visited = new bool[adjacencyList.Count];
        DepthFirstSearch(adjacencyList, visited, src, visitOrder);
        return visitOrder;
    }

    // with a disconnected graph, you have to traverse the graph from every unvisited vertex
    private static List<int> TraverseDisconnectedGraph(List<List<int>> adjacencyList)
    {
        var visitOrder = new List<int>(); // res
        var visited = new bool[adjacencyList.Count];
        // loop through all vertices to handle disconnected graphs
        for (var vertex = 0; vertex < adjacencyList.Count; vertex++)
        {
            if (!visited[vertex]) DepthFirstSearch(adjacencyList, visited, vertex, visitOrder);
        }
        return visitOrder;
    }

    private static void DepthFirstSearch(
        List<List<int>> adjacencyList, bool[] visited, int sourceVertex, List<int> visitOrder)
    {
        visited[sourceVertex] = true;
        visitOrder.Add(sourceVertex);
        // recursively visit all adjacent vertices that are not visited yet
        foreach (var vertex in adjacencyList[sourceVertex])
        {
            if (!visited[vertex]) DepthFirstSearch(adjacencyList, visited, vertex, visitOrder);
        }
    }

    public static IEnumerable<object[]> ConnectedGraphTestCases()
    {
        yield return
        [
            new List<List<int>> { new() { 0, 2 }, new() { 0, 3 }, new() { 2, 4 }, new() { 1, 0 } },
            new List<int> { 2, 4 },
            2 // src
        ];
        yield return
        [
            new List<List<int>> { new() { 1, 2 }, new() { 1, 0 }, new() { 2, 0 }, new() { 2, 3 }, new() { 4, 2 } },
            new List<int> { 1, 2, 0, 3 },
            1 // src
        ];;
    }
    
    [Theory]
    [MemberData(nameof(ConnectedGraphTestCases))]
    public void TraverseConnectedGraph_ShouldReturnCorrectOrderOfTraversal(List<List<int>> adj, List<int> exp, int src)
    {
        var res = TraverseConnectedGraph(adj, src);
        res.Should().Equal(exp);
    }

    public static IEnumerable<object[]> DisconnectedGraphTestCases()
    {
        yield return
        [
            new List<List<int>> { new() { 1, 2 }, new() { 2, 0 }, new() { 0, 3 }, new() { 4, 5 } },
            new List<int> { 0, 3, 1, 4, 5 }
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
