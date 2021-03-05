using Fengj.API;
using HexMath;
using ReactiveMarbles.PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Fengj.Map
{
    interface IComponent
    {
        TerrainCMPType type { get; set; }
    }

    enum DetectType
    {
        UN_VISIBLE,
        VISION_VISIBLE,
        TERRAIN_VISIBLE
    }

    interface ICell
    {
        AxialCoord axialCoord { get; set; }

        TerrainType terrainType { get; set; }

        ITerrainDef terrainDef { get; }

        DetectType detectType { get; set; }

        List<IComponent> components { get; }
    }

    public class TerrainComponent : IComponent
    {
        public TerrainCMPType type { get; set; }

        public TerrainComponent(TerrainCMPType type)
        {
            this.type = type;
        }
    }

    class Cell : ICell, INotifyPropertyChanged
    {
        public static MapData map;

        public static Func<TerrainType, string, ITerrainDef> funcGetTerrainDef;

        public AxialCoord axialCoord { get; set; }

        //public (int x, int y) vectIndex { get; set; }

        public TerrainType terrainType { get; set; }

        public string terrainCode;

        public event PropertyChangedEventHandler PropertyChanged;

        public ITerrainDef terrainDef => funcGetTerrainDef(terrainType, terrainCode);

        public DetectType detectType
        {
            get
            {
                return _detectType;
            }
            set
            {
                _detectType = value;

                if (detectType == DetectType.TERRAIN_VISIBLE)
                {
                    var nearUnVisibleCells = axialCoord.GetRingWithWidth(1, 2)
                        .Where(x=> map.HasCell(x))
                        .Select(x => map.GetCell(x))
                        .Where(x => x.detectType == DetectType.UN_VISIBLE);
                    foreach (var cell in nearUnVisibleCells)
                    {
                        cell.detectType = DetectType.VISION_VISIBLE;
                    }
                }
            }
        }

        public List<IComponent> components => _components;

        private List<IComponent> _components;

        private DetectType _detectType;

        public Cell(AxialCoord axialCoord, ITerrainDef def)
        {
            this.axialCoord = axialCoord;

            this.terrainType = def.type;
            this.terrainCode = def.code;
            this.detectType = DetectType.UN_VISIBLE;

            this._components = new List<IComponent>();
            Intergrate();
        }

        private void Intergrate()
        {
            this.WhenPropertyChanges(x => x.detectType).Subscribe(_ => map.changedCell = this);
        }
    }
}