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
    public class ModManagerTest : IClassFixture<ModManagerTestFixture>
    {
        public static FileSystemWapper fileSystemWapper;

        [Fact]
        void LoadTest()
        {
            var path = "C:/MOD/TEST/";
            var subPaths = new string[] { "M1", "M2", "M3" };

            var mockDirectory = new Mock<IDirectory>();
            foreach (var sub in subPaths)
            {
                mockDirectory.Setup(x => x.EnumerateDirectories($"{path}"))
                    .Returns<string>((x) => sub.Select(code => $"{path}/{sub}").ToArray());
            }

            fileSystemWapper.Directory = mockDirectory.Object;

            var mock = new Mock<IModBuilder>();

            foreach(var subPath in subPaths)
            {
                mock.Setup(x => x.Build($"{path}{subPath}")).Returns<string>((p) =>
                {
                    IMod mod = Mock.Of<IMod>(x => x.name == subPath && x.path == $"{path}{subPath}" && x.terrainDefs == new List<ITerrainDef>()
                    {
                        new TerrainDef(){ modName = subPath,  type = TerrainType.PLAIN,  code = subPath+"TEST1", path = $"TEST1.png"},
                        new TerrainDef(){ modName = subPath,  type = TerrainType.PLAIN,  code = subPath+"TEST2", path = $"TEST2.png"},
                        new TerrainDef(){ modName = subPath,  type = TerrainType.PLAIN,  code = subPath+"TEST3", path = $"TEST3.png"},
                        new TerrainDef(){ modName = subPath,  type = TerrainType.HILL,  code = subPath+"TEST3", path = $"TEST3.png"}
                    });
                    return mod;
                });
            }


            var modManager = ModManager.Load(path, mock.Object);
            modManager.dictTerrainDefs.Count().Should().Be(2);
            modManager.dictTerrainDefs[TerrainType.PLAIN].Count().Should().Be(9);
            modManager.dictTerrainDefs[TerrainType.HILL].Count().Should().Be(3);

        }
    }

    public class ModManagerTestFixture
    {

        public ModManagerTestFixture()
        {
            ModManagerTest.fileSystemWapper = new FileSystemWapper();
            SystemIO.FileSystem = ModManagerTest.fileSystemWapper;
        }

    }
}
