using FluentAssertions;

namespace Graph.Algorithms.Tests.Unit;

public class BreadthFirstSearchTests
{
    private static List<int> TraverseConnectedGraph(List<List<int>> adj)
    {
        var V = adj.Count;
        var q = new Queue<int>();
        var res = new List<int>();
        var visited = new bool[V];
        var src = 0;
        visited[src] = true;
        q.Enqueue(src);
        while (q.Count > 0)
        {
            var curr = q.Dequeue();
            res.Add(curr);
            // visit all the unvisited
            // neighbours of current node
            foreach (var x in adj[curr])
            {
                if (visited[x]) continue;
                visited[x] = true;
                q.Enqueue(x);
            }
        }
        return res;
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

    private static List<int> TraverseDisconnectedGraph(List<List<int>> adj)
    {
        var V = adj.Count;
        var res = new List<int>();
        var visited = new bool[V];
        for (var i = 0; i < V; i++)
        {
            if (!visited[i]) BreadthFirstSearch(adj, i, visited, res);
        }
        return res;
    }

    private static void BreadthFirstSearch(List<List<int>> adj, int src, bool[] visited, List<int> res)
    {
        var q = new Queue<int>();
        visited[src] = true;
        q.Enqueue(src);
        while (q.Count > 0)
        {
            var curr = q.Dequeue();
            res.Add(curr);
            // visit all the unvisited
            // neighbours of current node
            foreach (var x in adj[curr])
            {
                if (visited[x]) continue;
                visited[x] = true;
                q.Enqueue(x);
            }
        }
    }

    [Theory]
    [MemberData(nameof(DisconnectedGraphTestCases))]
    public void TraverseDisconnectedGraph_ShouldReturnCorrectOrderOfTraversal(List<List<int>> adj, List<int> exp)
    {
        var res = TraverseDisconnectedGraph(adj);
        res.Should().Equal(exp);
    }
}
