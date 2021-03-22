using Moq;
using PostSharp.Aspects;
using System;
using System.ComponentModel.DataAnnotations;
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

            SomeClass t = new SomeClass();
            t.Size = 200;
        }
    }

    class RangeAttribute : LocationInterceptionAspect
    {
        private int min;
        private int max;

        public RangeAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        public override void OnSetValue(LocationInterceptionArgs args)
        {
            int value = (int)args.Value;
            if (value < min) value = min;
            if (value > max) value = max;
            args.SetNewValue(value);
        }
    }

    class SomeClass
    {
        [Range(1, 50)]
        public int Size { get; set; }
    }
}
