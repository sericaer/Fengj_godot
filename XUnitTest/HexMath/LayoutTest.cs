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
    public class LayoutTest
    {
        [Fact]
        public void PixelToHexTest()
        {
            AxialCoord h = new AxialCoord(3, 4);
            Layout flat = new Layout(Layout.flat, new Point(10.0, 15.0), new Point(35.0, 71.0));

            flat.PixelToHex(flat.HexToPixel(h)).Should().Be(h);
        }
    }
}
