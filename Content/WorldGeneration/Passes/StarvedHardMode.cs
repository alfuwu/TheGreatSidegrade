using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using TheGreatSidegrade.Common;
using TheGreatSidegrade.Content.Tiles.Starved;
using TheGreatSidegrade.Content.Walls.Starved;

namespace TheGreatSidegrade.Content.WorldGeneration.Passes;

public class StarvedHardMode {
    public static void Method(GenerationProgress progress, GameConfiguration configuration) {
        WorldGen.IsGeneratingHardMode = true;
        Main.rand ??= new UnifiedRandom((int) DateTime.Now.Ticks);
        float num1 = Main.rand.Next(300, 400) * (1f / 1000f);
        float num2 = Main.rand.Next(200, 300) * (1f / 1000f);
        int i1 = (int)(Main.maxTilesX * num1);
        int i2 = (int)(Main.maxTilesX * (1.0 - num1));
        int num3 = 1;
        if (Main.rand.NextBool(2)) {
            i2 = (int)(Main.maxTilesX * num1);
            i1 = (int)(Main.maxTilesX * (1.0 - num1));
            num3 = -1;
        }
        int num4 = 1;
        if (Main.dungeonX < Main.maxTilesX / 2)
            num4 = -1;
        if (num4 < 0) {
            if (i2 < i1)
                i2 = (int)(Main.maxTilesX * num2);
            else
                i1 = (int)(Main.maxTilesX * num2);
        } else if (i2 > i1) {
            i2 = (int)(Main.maxTilesX * (1.0 - num2));
        } else {
            i1 = (int)(Main.maxTilesX * (1.0 - num2));
        }
        GERunner(i1, 0, (3 * num3), 5f, true);
        GERunner(i2, 0, (3 * -num3), 5f, false);
    }

    public static void GERunner(int i, int j, float speedX = 0f, float speedY = 0f, bool good = true) {
        Main.rand ??= new UnifiedRandom((int)DateTime.Now.Ticks);
        int num = Main.rand.Next(200, 250);
        float num2 = Main.maxTilesX / 4200;
        num = (int)(num * num2);
        double num3 = num;
        Vector2 vector = default;
        vector.X = i;
        vector.Y = j;
        Vector2 vector2 = default;
        vector2.X = Main.rand.Next(-10, 11) * 0.1f;
        vector2.Y = Main.rand.Next(-10, 11) * 0.1f;
        if (speedX != 0f || speedY != 0f) {
            vector2.X = speedX;
            vector2.Y = speedY;
        }
        bool flag = true;
        while (flag) {
            int num4 = (int)(vector.X - num3 * 0.5);
            int num5 = (int)(vector.X + num3 * 0.5);
            int num6 = (int)(vector.Y - num3 * 0.5);
            int num7 = (int)(vector.Y + num3 * 0.5);
            if (num4 < 0)
                num4 = 0;
            if (num5 > Main.maxTilesX)
                num5 = Main.maxTilesX;
            if (num6 < 0)
                num6 = 0;
            if (num7 > Main.maxTilesY)
                num7 = Main.maxTilesY;
            for (int k = num4; k < num5; k++) {
                for (int l = num6; l < num7; l++) {
                    if (!((double)(Math.Abs(k - vector.X) + Math.Abs(l - vector.Y)) < num * 0.5 * (1.0 + Main.rand.Next(-10, 11) * 0.015)))
                        continue;
                    if (good) {
                        if (Main.tile[k, l].WallType == (ushort) ModContent.WallType<StarvingStoneWall>())
                            Main.tile[k, l].WallType = 28;
                        if (Main.tile[k, l].WallType == 63 || Main.tile[k, l].WallType == 65 || Main.tile[k, l].WallType == 66 || Main.tile[k, l].WallType == 68 || Main.tile[k, l].WallType == 69 || Main.tile[k, l].WallType == 81)
                            Main.tile[k, l].WallType = 70;
                        else if (Main.tile[k, l].WallType == 216 || Main.tile[k, l].WallType == (ushort) ModContent.WallType<StarvedNaturalWall1>())
                            Main.tile[k, l].WallType = 219;
                        else if (Main.tile[k, l].WallType == 187 || Main.tile[k, l].WallType == (ushort) ModContent.WallType<StarvedNaturalWall2>())
                            Main.tile[k, l].WallType = 222;
                        if (Main.tile[k, l].WallType == 3 || Main.tile[k, l].WallType == 83)
                            Main.tile[k, l].WallType = 28;
                        if (Main.tile[k, l].TileType == 2 || Main.tile[k, l].TileType == (ushort) ModContent.TileType<StarvedGrass>()) {
                            Main.tile[k, l].TileType = 109;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 1 || Main.tile[k, l].TileType == (ushort) ModContent.TileType<StarvingStone>()) {
                            Main.tile[k, l].TileType = 117;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 53 || Main.tile[k, l].TileType == 123 || Main.tile[k, l].TileType == (ushort) ModContent.TileType<StarvedSand>()) {
                            Main.tile[k, l].TileType = 116;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 23 || Main.tile[k, l].TileType == 199 || Main.tile[k, l].TileType == (ushort) ModContent.TileType<StarvedGrass>()) {
                            Main.tile[k, l].TileType = 109;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 25 || Main.tile[k, l].TileType == 203 || Main.tile[k, l].TileType == (ushort) ModContent.TileType<StarvingStone>()) {
                            Main.tile[k, l].TileType = 117;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 112 || Main.tile[k, l].TileType == 234 || Main.tile[k, l].TileType == (ushort) ModContent.TileType<StarvedSand>()) {
                            Main.tile[k, l].TileType = 116;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 161 || Main.tile[k, l].TileType == 163 || Main.tile[k, l].TileType == 200 || Main.tile[k, l].TileType == (ushort)ModContent.TileType<StarvedIce>()) {
                            Main.tile[k, l].TileType = 164;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 396 || Main.tile[k, l].TileType == (ushort) ModContent.TileType<StarvedSandstone>()) {
                            Main.tile[k, l].TileType = 403;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 397 || Main.tile[k, l].TileType == (ushort) ModContent.TileType<HardenedStarvedSand>()) {
                            Main.tile[k, l].TileType = 402;
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 661 || Main.tile[k, l].TileType == 662) {
                            Main.tile[k, l].TileType = 60;
                            Utils.SquareTileFrame(k, l);
                        }
                    } else if (GreatlySidegradedWorld.worldEvil != GreatlySidegradedWorld.WorldEvil.Corruption && GreatlySidegradedWorld.worldEvil != GreatlySidegradedWorld.WorldEvil.Crimson) {
                        if (Main.tile[k, l].WallType == 28)
                            Main.tile[k, l].WallType = (ushort) ModContent.WallType<StarvingStoneWall>();
                        if (Main.tile[k, l].WallType == 63 || Main.tile[k, l].WallType == 65 || Main.tile[k, l].WallType == 66 || Main.tile[k, l].WallType == 68)
                            Main.tile[k, l].WallType = (ushort) ModContent.WallType<StarvedGrassWall>();
                        else if (Main.tile[k, l].WallType == 216)
                            Main.tile[k, l].WallType = (ushort) ModContent.WallType<StarvedNaturalWall1>();
                        else if (Main.tile[k, l].WallType == 187)
                            Main.tile[k, l].WallType = (ushort) ModContent.WallType<StarvedNaturalWall2>();
                        if (Main.tile[k, l].TileType == 2) {
                            Main.tile[k, l].TileType = (ushort) ModContent.TileType<StarvedGrass>();
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 1) {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvingStone>();
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 53 || Main.tile[k, l].TileType == 123) {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedSand>();
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 109) {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedGrass>();
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 117) {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvingStone>();
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 116) {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedSand>();
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 161 || Main.tile[k, l].TileType == 164) {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedIce>();
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 396) {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<StarvedSandstone>();
                            Utils.SquareTileFrame(k, l);
                        } else if (Main.tile[k, l].TileType == 397) {
                            Main.tile[k, l].TileType = (ushort)ModContent.TileType<HardenedStarvedSand>();
                            Utils.SquareTileFrame(k, l);
                        }
                    }
                }
            }
            vector += vector2;
            vector2.X += Main.rand.Next(-10, 11) * 0.05f;
            if (vector2.X > speedX + 1f)
                vector2.X = speedX + 1f;
            if (vector2.X < speedX - 1f)
                vector2.X = speedX - 1f;
            if (vector.X < -num || vector.Y < -num || vector.X > Main.maxTilesX + num || vector.Y > Main.maxTilesX + num)
                flag = false;
        }
    }
}
