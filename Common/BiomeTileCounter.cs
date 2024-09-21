using System;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.Tiles.Starved;

namespace TheGreatSidegrade.Common;

public class BiomeTileCounter : ModSystem {
    public int facturedBlockCount;
    public int nothingBlockCount;
    public int rottenBlockCount;
    public int spiralBlockCount;
    public int starvedBlockCount;

    public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
        starvedBlockCount = tileCounts[ModContent.TileType<StarvedGrass>()] +
            tileCounts[ModContent.TileType<StarvingStone>()] +
            tileCounts[ModContent.TileType<StarvedIce>()] +
            tileCounts[ModContent.TileType<StarvedJungleGrass>()] +
            tileCounts[ModContent.TileType<StarvedSand>()] +
            tileCounts[ModContent.TileType<StarvedSandstone>()] +
            tileCounts[ModContent.TileType<HardenedStarvedSand>()];
    }
}
