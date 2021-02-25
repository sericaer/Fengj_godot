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


    interface ICell
    {
        AxialCoord axialCoord { get; set; }

        TerrainType terrainType { get; set; }

        ITerrainDef terrainDef { get; }
        int detectLevel { get; set; }

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

        public int detectLevel { get; set; }
        public List<IComponent> components => _components;


        private List<IComponent> _components;

        public Cell(AxialCoord axialCoord, ITerrainDef def)
        {
            this.axialCoord = axialCoord;

            this.terrainType = def.type;
            this.terrainCode = def.code;
            this.detectLevel = 0;

            this._components = new List<IComponent>();
            Intergrate();
        }

        //public Dictionary<DIRECTION, ICell> GetNeighboursWithDirect()
        //{
        //    return map.GetNears(vectIndex.x, vectIndex.y);
        //}

        //public IEnumerable<ICell> GetNeighbours(int distance)
        //{
        //    return map.GetCellsWithDistance(vectIndex.x, vectIndex.y, distance);
        //}

        private void Intergrate()
        {
            this.WhenPropertyChanges(x => x.detectLevel).Subscribe(_ => map.changedCell = this);
        }
    }
}