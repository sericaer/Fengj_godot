using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fengj.API;
using FluentAssertions;
using Moq;
using Fengj.Modder;
using Xunit;
using Fengj;
using Fengj.IO;
using System.IO.Abstractions;
using System.IO;

namespace XUnitTest.Modder
{
    class ModManagerTest
    {
        [Fact]
        void LoadTest()
        {
            var path = "C:/MOD/TEST";

            var mock = new Mock<IModBuilder>();
            mock.Setup(x => x.Build(path)).Returns<string>((p) =>
            {
                IMod mod = Mock.Of<IMod>(x=>x.name == "TEST");
                return mod;
            });

            ModManager.Load(path, mock.Object);
        }
    }
}
