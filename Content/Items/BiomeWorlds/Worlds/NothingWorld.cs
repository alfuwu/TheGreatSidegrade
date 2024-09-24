using System.Collections.Generic;
using SubworldLibrary;
using Terraria.WorldBuilding;
using Terraria.ID;
using Terraria.ModLoader;
using TheGreatSidegrade.Common;

namespace TheGreatSidegrade.Content.Items.BiomeWorlds.Worlds;

[ExtendsFromMod("BiomeWorlds", "SubworldLibrary")]
public class NothingWorld : Subworld {
	public override int Width => 4200;

	public override int Height => 1200;

	public override bool ShouldSave => false;

	public override bool NoPlayerSaving => false;

	public override bool NormalUpdates => true;

	public override List<GenPass> Tasks => [new NothingWorldGenPass()];

	public override void OnEnter() {
		SubworldSystem.noReturn = false;
	}
}

[ExtendsFromMod("BiomeWorlds", "SubworldLibrary")]
public class NothingWorldGenPass : BaseWorldGenPass {
    internal override GreatlySidegradedWorld.WorldEvil WorldEvilType => GreatlySidegradedWorld.WorldEvil.Nothing;

    public NothingWorldGenPass() : base() {
        replacementTiles.Add(TileID.Plants, TileID.Plants);
        replacementTiles.Add(TileID.Vines, TileID.Vines);
        replacementTiles.Add(TileID.Grass, TileID.Grass);
        replacementTiles.Add(TileID.Stone, TileID.Stone);
        replacementTiles.Add(TileID.Sand, TileID.Sand);
        replacementTiles.Add(TileID.Sandstone, TileID.Sandstone);
        replacementTiles.Add(TileID.HardenedSand, TileID.HardenedSand);
    }
}
