using FluentAssertions;

namespace Graph.Algorithms.Tests.Unit;

public class DepthFirstSearchTests
{
    // with a connected graph, you can visit every vertex from the source vertex
    private static List<int> TraverseConnectedGraph(List<List<int>> adj)
    {
        var res = new List<int>();
        var visited = new bool[adj.Count];
        DepthFirstSearch(adj, visited, 0, res);
        return res;
    }

    // with a disconnected graph, you have to traverse the graph from every unvisited vertex
    private static List<int> TraverseDisconnectedGraph(List<List<int>> adj)
    {
        var res = new List<int>();
        var visited = new bool[adj.Count];
        // loop through all vertices to handle disconnected graphs
        for (var i = 0; i < adj.Count; i++)
        {
            if (!visited[i]) DepthFirstSearch(adj, visited, i, res);
        }
        return res;
    }

    private static void DepthFirstSearch(List<List<int>> adj, bool[] visited, int s, List<int> res)
    {
        visited[s] = true;
        res.Add(s);
        // recursively visit all adjacent vertices that are not visited yet
        foreach (var i in adj[s])
        {
            if (!visited[i]) DepthFirstSearch(adj, visited, i, res);
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
        // 0, 1, 3, 4, 2
        yield return
        [
            new List<List<int>> { new() { 1, 2 }, new() { 0, 3 }, new() { 0, 4 }, new() { 1, 4 }, new() { 2, 3 } },
            new List<int> { 0, 1, 3, 4, 2 }
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
        // 0, 2, 1, 3, 4, 5
        yield return
        [
            new List<List<int>> { new() { 2, 3 }, new() { 2 }, new() { 0, 1 }, new() { 0 }, new() { 5 }, new() { 4 } },
            new List<int> { 0, 2, 1, 3, 4, 5 }
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
