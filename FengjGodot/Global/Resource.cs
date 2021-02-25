using Fengj.API;
using Godot;
using System;
using System.Collections.Generic;

static class GlobalResource
{
    public static TileSet tileSet;

    internal static void BuildTileSet(IEnumerable<ITerrainDef> terrainDefs)
    {
		tileSet = new TileSet();

		foreach (var terrain in terrainDefs)
		{
			var id = tileSet.GetLastUnusedTileId();
			tileSet.CreateTile(id);
			tileSet.TileSetName(id, terrain.path);
			tileSet.TileSetTexture(id, ResourceLoader.Load<Texture>(terrain.path));

			GD.Print($"BuildTileSet {0} {1}", id.ToString(), terrain.path);
		}
	}
}
