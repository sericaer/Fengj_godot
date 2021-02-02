using Fengj.API;
using Godot;
using System;

static class GlobalResource
{
    public static TileSet tileSet;

    internal static void BuildTileSet(ITerrainDef[] terrainDefs)
    {
		tileSet = new TileSet();

		foreach (var terrain in terrainDefs)
		{
			var id = tileSet.GetLastUnusedTileId();
			tileSet.CreateTile(id);
			tileSet.TileSetName(id, terrain.key);
			tileSet.TileSetTexture(0, ResourceLoader.Load<Texture>(terrain.path));
		}
	}
}
