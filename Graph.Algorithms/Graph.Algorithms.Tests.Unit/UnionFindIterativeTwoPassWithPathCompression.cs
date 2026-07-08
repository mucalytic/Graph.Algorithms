using FluentAssertions;

namespace Graph.Algorithms.Tests.Unit;

public class UnionFindIterativeTwoPassWithPathCompression
{
    public class Internal
    {
        private readonly int[] _parent;
        private readonly int[] _size;

        public Internal(int size)
        {
            _size = new int[size];
            _parent = new int[size];
            for (var i = 0; i < size; i++)
            {
                _parent[i] = i;
                _size[i] = 1;
            }
        }
        
        public void Union(int i, int j)
        {
            var ri = Find(i);
            var rj = Find(j);
            if (ri == rj) return;
            if (_size[ri] < _size[rj])
            {
                _size[rj] += _size[ri];
                _parent[ri] = rj;
            }
            else
            {
                _size[ri] +=  _size[rj];
                _parent[rj] = ri;
            }
        }

        public int Find(int i)
        {
            var root = i;
            while (_parent[root] != root)
            {
                root = _parent[root];
            }
            while (root != i) // full path compression is forced here
            {
                var n = _parent[i];
                _parent[i] = root;
                i = n;
            }
            return root;
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
    public void IndexesOfElements_ShouldPointToTheirParent_WhenUnionOnly()
    {
        // arrange
        var uf =  new Internal(7);
        uf.Union(1, 2);
        uf.Union(2, 3);
        uf.Union(3, 4);
        uf.Union(5, 6);
        
        // assert
        uf.Parent.Should().Equal([0, 1, 1, 1, 1, 5, 5]);
    }
    
    [Fact]
    public void IndexesOfElements_ShouldPointToRoot_WhenUnionFindDone()
    {
        // arrange
        var uf =  new Internal(7);
        uf.Union(1, 2);
        uf.Union(2, 3);
        uf.Union(3, 4);
        uf.Union(5, 6);
                
        // assert
        uf.Find(1).Should().Be(uf.Find(4));
        uf.Find(5).Should().Be(uf.Find(6));
        uf.Parent.Should().Equal([0, 1, 1, 1, 1, 5, 5]);
    }
}
