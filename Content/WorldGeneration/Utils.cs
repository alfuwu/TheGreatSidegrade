using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheGreatSidegrade.Content.Tiles.Starved;

namespace TheGreatSidegrade.Content.WorldGeneration {
    public class Utils {
        public static void SquareTileFrame(int i, int j, bool resetFrame = true, bool resetSlope = false, bool largeHerb = false) {
            if (resetSlope) {
                Tile t = Main.tile[i, j];
                t.Slope = SlopeType.Solid;
                t.IsHalfBlock = false;
            }
            WorldGen.TileFrame(i - 1, j - 1, false, largeHerb);
            WorldGen.TileFrame(i - 1, j, false, largeHerb);
            WorldGen.TileFrame(i - 1, j + 1, false, largeHerb);
            WorldGen.TileFrame(i, j - 1, false, largeHerb);
            WorldGen.TileFrame(i, j, resetFrame, largeHerb);
            WorldGen.TileFrame(i, j + 1, false, largeHerb);
            WorldGen.TileFrame(i + 1, j - 1, false, largeHerb);
            WorldGen.TileFrame(i + 1, j, false, largeHerb);
            WorldGen.TileFrame(i + 1, j + 1, false, largeHerb);
        }

        public static void Swap<T>(ref T first, ref T second) where T : unmanaged {
            (second, first) = (first, second);
        }

        public static int TileCheck(int positionX) {
            for (int i = (int)(GenVars.worldSurfaceLow - 30); i < Main.maxTilesY; i++) {
                Tile tile = Framing.GetTileSafely(positionX, i);
                if ((tile.TileType == TileID.Dirt || tile.TileType == TileID.ClayBlock || tile.TileType == TileID.Stone || tile.TileType == TileID.Sand || tile.TileType == ModContent.TileType<StarvedSand>() || tile.TileType == TileID.Mud || tile.TileType == TileID.SnowBlock || tile.TileType == TileID.IceBlock) && tile.HasTile)
                    return i;
            }
            return 0;
        }
    }
}
