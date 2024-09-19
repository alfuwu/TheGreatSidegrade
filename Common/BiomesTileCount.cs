using System;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.Evils.Starved.Tiles;

namespace TheGreatSidegrade.Common {
    public class BiomesTileCount : ModSystem {
        public int facturedBlockCount;
        public int nothingBlockCount;
        public int rottenBlockCount;
        public int spiralBlockCount;
        public int starvedBlockCount;

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts) {
            starvedBlockCount = tileCounts[ModContent.TileType<StarvedGrass>()];
            starvedBlockCount = tileCounts[ModContent.TileType<StarvingStone>()];
        }
    }
}
