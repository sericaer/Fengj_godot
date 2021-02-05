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

namespace XUnitTest.Modder
{
    public class TerrainDefTest : IClassFixture<TerrainDefTestFixture>
    {
        public static FileSystemWapper fileSystemWapper;

        [Fact]
        void BuildTest()
        {
            var modName = "TEST_MOD";
            var modPath = "C:/TEST_MOD/";


            var pngCodes = Enumerable.Range(0, 10); ;

            var mockDirectory = new Mock<IDirectory>();
            foreach (TerrainType type in Enum.GetValues(typeof(TerrainType)))
            {
                mockDirectory.Setup(x => x.EnumerateFiles($"{modPath}{TerrainDef.imagePath}{type}", "*.png"))
                    .Returns<string, string>((x, y) => pngCodes.Select(code => $"{x}/{code}.png").ToArray());
            }


            fileSystemWapper.Directory = mockDirectory.Object;

            var mockPath = new Mock<IPath>();
            mockPath.Setup(x => x.GetFileNameWithoutExtension(It.IsAny<string>())).Returns<string>(x => System.IO.Path.GetFileNameWithoutExtension(x));
            fileSystemWapper.Path = mockPath.Object;

            var rslt = TerrainDef.Builder.BuildArray(modName, modPath);

            foreach (TerrainType type in Enum.GetValues(typeof(TerrainType)))
            {
                foreach(var code in pngCodes)
                {
                    rslt.Should().Contain(x => x.type == type
                        && x.modName == modName
                        && x.code == code.ToString()
                        && x.path == $"{modPath}{TerrainDef.imagePath}{type}/{code}.png");
                }
            }
        }
    }

    public class TerrainDefTestFixture
    {

        public TerrainDefTestFixture()
        {
            TerrainDefTest.fileSystemWapper = new FileSystemWapper();
            SystemIO.FileSystem = TerrainDefTest.fileSystemWapper;
        }
        
    }

    public class FileSystemWapper : IFileSystem
    {
        public IFile File { get; set; }

        public IDirectory Directory { get; set; }

        public IFileInfoFactory FileInfo { get; set; }

        public IFileStreamFactory FileStream { get; set; }

        public IPath Path { get; set; }

        public IDirectoryInfoFactory DirectoryInfo { get; set; }

        public IDriveInfoFactory DriveInfo { get; set; }

        public IFileSystemWatcherFactory FileSystemWatcher { get; set; }
    }
}
