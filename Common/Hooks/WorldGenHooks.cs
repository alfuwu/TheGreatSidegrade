using Avalon.Prefixes;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.Tiles.Fractured;
using TheGreatSidegrade.Content.Tiles.Nothing;
using TheGreatSidegrade.Content.Tiles.Rotten;
using TheGreatSidegrade.Content.Tiles.Spiral;
using TheGreatSidegrade.Content.Tiles.Starved;

namespace TheGreatSidegrade.Common.Hooks;

public class WorldGenHooks {
    public static void TileRunner(On_WorldGen.orig_TileRunner orig, int i, int j, double strength, int steps, int type, bool addTile, double speedX, double speedY, bool noYChange, bool overRide, int ignoreTileType) {
        if ((type == TileID.Demonite || type == TileID.Crimtane /* nvm it does im just stupid */ || (TheGreatSidegrade.HasAvalon && TheGreatSidegrade.Avalon.TryFind("BacciliteOre", out ModTile tile) && type == tile.Type)) && !GreatlySidegradedWorld.IsVanillaEvil())
            type = GreatlySidegradedWorld.worldEvil switch {
                GreatlySidegradedWorld.WorldEvil.Fractured => ModContent.TileType<Fractite>(),
                GreatlySidegradedWorld.WorldEvil.Nothing => ModContent.TileType<Oblivium>(),
                GreatlySidegradedWorld.WorldEvil.Rotten => ModContent.TileType<Rust>(),
                GreatlySidegradedWorld.WorldEvil.Spiral => ModContent.TileType<Spite>(),
                GreatlySidegradedWorld.WorldEvil.Starved => ModContent.TileType<Hungrite>(),
                _ => type
            };
        orig(i, j, strength, steps, type, addTile, speedX, speedY, noYChange, overRide, ignoreTileType);
    }

    public static void AddUpAlignmentCounts(On_WorldGen.orig_AddUpAlignmentCounts orig, bool clearCounts) {
        orig(clearCounts);
        if (clearCounts) {
            GreatlySidegradedWorld.totalFract2 = 0;
            GreatlySidegradedWorld.totalVoid2 = 0;
            GreatlySidegradedWorld.totalRot2 = 0;
            GreatlySidegradedWorld.totalTwisted2 = 0;
            GreatlySidegradedWorld.totalStarved2 = 0;
        }

        WorldGen.totalSolid2 +=
            WorldGen.tileCounts[ModContent.TileType<StarvingStone>()] +
            WorldGen.tileCounts[ModContent.TileType<StarvedGrass>()] +
            WorldGen.tileCounts[ModContent.TileType<StarvedJungleGrass>()] +
            //WorldGen.tileCounts[ModContent.TileType<StarvedGrassMowed>()] +
            WorldGen.tileCounts[ModContent.TileType<StarvedSand>()] +
            WorldGen.tileCounts[ModContent.TileType<StarvedIce>()] +
            WorldGen.tileCounts[ModContent.TileType<StarvedSandstone>()] +
            WorldGen.tileCounts[ModContent.TileType<HardenedStarvedSand>()];

        Array.Clear(WorldGen.tileCounts, 0, WorldGen.tileCounts.Length);
    }

    public static void CountTiles(On_WorldGen.orig_CountTiles orig, int X) {
        orig(X);
        if (X == 0) {
            GreatlySidegradedWorld.totalFract = GreatlySidegradedWorld.totalFract2;
            GreatlySidegradedWorld.totalVoid = GreatlySidegradedWorld.totalVoid2;
            GreatlySidegradedWorld.totalRot = GreatlySidegradedWorld.totalRot2;
            GreatlySidegradedWorld.totalTwisted = GreatlySidegradedWorld.totalTwisted2;
            GreatlySidegradedWorld.totalStarved = GreatlySidegradedWorld.totalStarved2;
            GreatlySidegradedWorld.tFract = (byte) Math.Round(((double) GreatlySidegradedWorld.totalFract / WorldGen.totalSolid )* 100.0);
            GreatlySidegradedWorld.tVoid = (byte) Math.Round(((double) GreatlySidegradedWorld.totalVoid / WorldGen.totalSolid) * 100.0);
            GreatlySidegradedWorld.tRot = (byte) Math.Round(((double) GreatlySidegradedWorld.totalRot / WorldGen.totalSolid) * 100.0);
            GreatlySidegradedWorld.tTwisted = (byte) Math.Round(((double) GreatlySidegradedWorld.totalTwisted / WorldGen.totalSolid) * 100.0);
            GreatlySidegradedWorld.tStarved = (byte) Math.Round(((double) GreatlySidegradedWorld.totalStarved / WorldGen.totalSolid) * 100.0);
            if (GreatlySidegradedWorld.tFract == 0 && GreatlySidegradedWorld.totalFract > 0)
                GreatlySidegradedWorld.tFract = 1;
            if (GreatlySidegradedWorld.tVoid == 0 && GreatlySidegradedWorld.totalVoid > 0)
                GreatlySidegradedWorld.tVoid = 1;
            if (GreatlySidegradedWorld.tRot == 0 && GreatlySidegradedWorld.totalRot > 0)
                GreatlySidegradedWorld.tRot = 1;
            if (GreatlySidegradedWorld.tTwisted == 0 && GreatlySidegradedWorld.totalTwisted > 0)
                GreatlySidegradedWorld.tTwisted = 1;
            if (GreatlySidegradedWorld.tStarved == 0 && GreatlySidegradedWorld.totalStarved > 0)
                GreatlySidegradedWorld.tStarved = 1;
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.TileCounts);
            GreatlySidegradedWorld.totalFract2 = 0;
            GreatlySidegradedWorld.totalVoid2 = 0;
            GreatlySidegradedWorld.totalRot2 = 0;
            GreatlySidegradedWorld.totalTwisted2 = 0;
            GreatlySidegradedWorld.totalStarved2 = 0;
        }
    }
}