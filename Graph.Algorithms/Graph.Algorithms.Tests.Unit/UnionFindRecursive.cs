using FluentAssertions;

namespace Graph.Algorithms.Tests.Unit;

public class UnionFindRecursive
{
    public class Internal(int size)
    {
        private readonly int[] _parent = Enumerable.Range(0, size).ToArray();

        public void Union(int i, int j)
        {
            var ri = Find(i);
            var rj = Find(j);
            _parent[ri] = rj;
        }

        public int Find(int i)
        {
            if (i == _parent[i]) return _parent[i]; // found the rep
            return Find(_parent[i]);
        }
        
        public int[] Parent => _parent;
    }

    [Theory]
    [InlineData(1, 2, true)]
    [InlineData(3, 4, true)]
    [InlineData(1, 3, false)]
    [InlineData(1, 4, false)]
    [InlineData(2, 3, false)]
    [InlineData(2, 4, false)]
    [InlineData(0, 1, false)]
    [InlineData(0, 2, false)]
    [InlineData(0, 3, false)]
    [InlineData(0, 4, false)]
    public void ElementsInUnion_ShouldBeInSameSet_(int i, int j, bool expected)
    {
        // arrange
        var uf =  new Internal(5);
        uf.Union(1, 2);
        uf.Union(3, 4);
        
        // act
        var sameSet = uf.Find(i) ==  uf.Find(j);
        
        // assert
        sameSet.Should().Be(expected);
    }
    
    [Fact]
    public void IndexesOfElements_ShouldPointToTheirParents()
    {
        // arrange
        var uf =  new Internal(5);
        uf.Union(1, 2);
        uf.Union(3, 4);
        
        // assert
        uf.Parent.Should().Equal([0, 2, 2, 4, 4]);
    }
}
