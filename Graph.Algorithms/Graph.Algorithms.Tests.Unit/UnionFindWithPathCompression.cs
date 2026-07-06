using FluentAssertions;

namespace Graph.Algorithms.Tests.Unit;

public class UnionFindWithPathCompression
{
    public class Internal(int size)
    {
        private readonly int[] _parents = Enumerable.Range(0, size).ToArray();

        public void Union(int i, int j)
        {
            var ri = Find(i);
            var rj = Find(j);
            _parents[ri] = rj;
        }

        public int Find(int i)
        {
            if (_parents[i] == i) return i; // found the rep
            return Find(_parents[i]);
        }
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
    public void Test(int i, int j, bool expected)
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
}
