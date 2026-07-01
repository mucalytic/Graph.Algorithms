using FluentAssertions;

namespace Graph.Algorithms.Tests.Unit;

public class UndirectedGraphTests
{
    private static List<List<int>> CreateAdjacencyMatrixFromEdges(int V, int[,] edges)
    {
        var mat = new List<List<int>>();
        // initialise the matrix with 0
        for (var i = 0; i < V; i++)
        {
            var row = new List<int>(new int[V]);
            mat.Add(row);
        }
        // add each edge to the adjacency matrix
        for (var i = 0; i < edges.GetLength(0); i++)
        {
            var u = edges[i, 0];
            var v = edges[i, 1];
            mat[u][v] = 1;
            // since the graph is undirected
            mat[v][u] = 1;
        }
        return mat;
    }
    
    [Fact]
    public void CreateAdjacencyMatrixFromEdges_Should_CreateAnAdjacencyMatrixFromEdges()
    {
        // 1-2
        // | |
        // 0-+
        var V = 3;
        // list of edges (u, v)
        int[,] edges = {{0, 1},{0, 2},{1, 2}};
        // Build the graph using edges
        var mat = CreateAdjacencyMatrixFromEdges(V, edges);
        // output should be:
        // 0 1 1
        // 1 0 1
        // 1 1 0
        List<List<int>> exp = [[0, 1, 1], [1, 0, 1], [1, 1, 0]];
        mat.Should().BeEquivalentTo(exp);
    }
}