using Moq;
using System;
using System.IO.Abstractions;
using Xunit;

namespace XUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var file = Mock.Of<IFile>();
        }
    }
}
