using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using Fengj;

using Xunit;
using Fengj.Map;
using System.Linq;
using Fengj.API;
using HexMath;

namespace XUnitTest.HexMath
{
    public class AxialCoordTest
    {
        [Fact]
        public void GetRingTest()
        {
            AxialCoord coord = new AxialCoord(0, 0);

            var ringCoords = coord.GetRing(0);
            ringCoords.Count().Should().Be(1);

            ringCoords.Should().BeEquivalentTo(new AxialCoord[] { coord });

            ringCoords = coord.GetRing(1);
            ringCoords.Count().Should().Be(6);

            ringCoords.Should().BeEquivalentTo(coord.GetNeighbors());

            ringCoords = coord.GetRing(2);
            ringCoords.Count().Should().Be(12);

            ringCoords.Should().BeEquivalentTo(new AxialCoord[] { new AxialCoord(2, 0),
                                                                  new AxialCoord(2, -2),
                                                                  new AxialCoord(2, -1),
                                                                  new AxialCoord(-2, 0),
                                                                  new AxialCoord(-2, 1),
                                                                  new AxialCoord(-2, 2),
                                                                  new AxialCoord(-1, 2),
                                                                  new AxialCoord(-1, -1),
                                                                  new AxialCoord(1, -2),
                                                                  new AxialCoord(1, 1),
                                                                  new AxialCoord(0, 2),
                                                                  new AxialCoord(0, -2),
                                                                  });
        }


        [Fact]
        public void GetRingWithWidthTest()
        {
            AxialCoord coord = new AxialCoord(0, 0);

            var ringCoords = coord.GetRingWithWidth(0,3);
            ringCoords.Count().Should().Be(1+6+12);

            //ringCoords.Should().BeEquivalentTo(new AxialCoord[] { coord });

            //ringCoords = coord.GetRing(1);
            //ringCoords.Count().Should().Be(6);

            //ringCoords.Should().BeEquivalentTo(coord.GetNeighbors());

            //ringCoords = coord.GetRing(2);
            //ringCoords.Count().Should().Be(12);

            //ringCoords.Should().BeEquivalentTo(new AxialCoord[] { new AxialCoord(2, 0),
            //                                                      new AxialCoord(2, -2),
            //                                                      new AxialCoord(2, -1),
            //                                                      new AxialCoord(-2, 0),
            //                                                      new AxialCoord(-2, 1),
            //                                                      new AxialCoord(-2, 2),
            //                                                      new AxialCoord(-1, 2),
            //                                                      new AxialCoord(-1, -1),
            //                                                      new AxialCoord(1, -2),
            //                                                      new AxialCoord(1, 1),
            //                                                      new AxialCoord(0, 2),
            //                                                      new AxialCoord(0, -2),
            //                                                      });
        }
    }
}
