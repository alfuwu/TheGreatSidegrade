using ReLogic.Utilities;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheGreatSidegrade.Content.Tiles.Starved;

namespace TheGreatSidegrade.Content.WorldGeneration.Passes {
    public class EvilAltars : GenPass {
        public EvilAltars() : base("Evil Altars", 20f) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
            Main.tileSolid[484] = false;
            progress.Message = Lang.gen[26].Value;
            int num641 = (int)((Main.maxTilesX * Main.maxTilesY) * 3.3E-06);
            int altarTile = ModContent.TileType<AltarOfTheStarvingOne>();
            if (WorldGen.remixWorldGen)
                num641 *= 3;
            for (int num642 = 0; num642 < num641; num642++) {
                progress.Set(num642 / num641);
                for (int num643 = 0; num643 < 10000; num643++) {
                    int num644 = WorldGen.genRand.Next(281, Main.maxTilesX - 3 - 280);
                    while (num644 > Main.maxTilesX * 0.45 && num644 < Main.maxTilesX * 0.55)
                        num644 = WorldGen.genRand.Next(281, Main.maxTilesX - 3 - 280);
                    int num645 = WorldGen.genRand.Next((int)(Main.worldSurface * 2.0 + Main.rockLayer) / 3, (int)(Main.rockLayer + (Main.maxTilesY - 350) * 2) / 3);
                    if (WorldGen.remixWorldGen)
                        num645 = WorldGen.genRand.Next(100, (int)(Main.maxTilesY * 0.9));
                    while (WorldGen.oceanDepths(num644, num645) || Vector2D.Distance(new Vector2D(num644, num645), GenVars.shimmerPosition) < WorldGen.shimmerSafetyDistance) {
                        num644 = WorldGen.genRand.Next(281, Main.maxTilesX - 3 - 280);
                        while (num644 > Main.maxTilesX * 0.45 && num644 < Main.maxTilesX * 0.55)
                            num644 = WorldGen.genRand.Next(281, Main.maxTilesX - 3 - 280);
                        num645 = WorldGen.genRand.Next((int)(Main.worldSurface * 2.0 + Main.rockLayer) / 3, (int)(Main.rockLayer + (Main.maxTilesY - 350) * 2) / 3);
                        if (WorldGen.remixWorldGen)
                            num645 = WorldGen.genRand.Next(100, (int)(Main.maxTilesY * 0.9));
                    }
                    int style2 = 0;
                    if (!WorldGen.IsTileNearby(num644, num645, altarTile, 3))
                        WorldGen.Place3x2(num644, num645, (ushort)altarTile, style2);
                    if (Main.tile[num644, num645].TileType == altarTile)
                        break;
                }
            }
        }
    }
}
