using System;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.Localization;
using TheGreatSidegrade.Content.Tiles.Starved;
using Terraria.ID;
using ReLogic.Utilities;

namespace TheGreatSidegrade.Content.WorldGeneration.Passes;

public class Fractured {
    public static void Method(GenerationProgress progress, GameConfiguration _) {
        progress.Message = Language.GetTextValue($"{TheGreatSidegrade.Localization}.World.Generation.Fractured.Message");
        int num778 = Main.maxTilesX;
        int num779 = 0;
        int num780 = Main.maxTilesX;
        int num781 = 0;
        for (int num782 = 0; num782 < Main.maxTilesX; num782++) {
            for (int num783 = 0; num783 < Main.worldSurface; num783++) {
                if (Main.tile[num782, num783].HasTile) {
                    if (Main.tile[num782, num783].TileType == TileID.JungleGrass) {
                        if (num782 < num778)
                            num778 = num782;

                        if (num782 > num779)
                            num779 = num782;
                    } else if (Main.tile[num782, num783].TileType == TileID.SnowBlock || Main.tile[num782, num783].TileType == TileID.IceBlock) {
                        if (num782 < num780)
                            num780 = num782;

                        if (num782 > num781)
                            num781 = num782;
                    }
                }
            }
        }

        int num784 = 10;
        num778 -= num784;
        num779 += num784;
        num780 -= num784;
        num781 += num784;
        int num785 = 500;
        int num786 = 100;
        double num787 = Main.maxTilesX * 0.00045;
        if (WorldGen.remixWorldGen) {
            num787 *= 2.0;
        } else if (WorldGen.tenthAnniversaryWorldGen) {
            num785 *= 2;
            num786 *= 2;
        }

        if (WorldGen.drunkWorldGen)
            num787 /= 2.0;

        for (int num811 = 0; num811 < num787; num811++) {
            int num812 = num780;
            int num813 = num781;
            int num814 = num778;
            int num815 = num779;
            double value16 = num811 / num787;
            progress.Set(value16);
            bool flag53 = false;
            int num816 = 0;
            int num817 = 0;
            int num818 = 0;
            while (!flag53) {
                flag53 = true;
                int num819 = Main.maxTilesX / 2;
                int num820 = 200;
                num816 = !WorldGen.drunkWorldGen ? WorldGen.genRand.Next(num785, Main.maxTilesX - num785) : GenVars.crimsonLeft ? WorldGen.genRand.Next((int)(Main.maxTilesX * 0.5), Main.maxTilesX - num785) : WorldGen.genRand.Next(num785, (int)(Main.maxTilesX * 0.5));
                num817 = num816 - WorldGen.genRand.Next(200) - 100;
                num818 = num816 + WorldGen.genRand.Next(200) + 100;
                if (num817 < GenVars.evilBiomeBeachAvoidance)
                    num817 = GenVars.evilBiomeBeachAvoidance;

                if (num818 > Main.maxTilesX - GenVars.evilBiomeBeachAvoidance)
                    num818 = Main.maxTilesX - GenVars.evilBiomeBeachAvoidance;

                if (num816 < num817 + GenVars.evilBiomeAvoidanceMidFixer)
                    num816 = num817 + GenVars.evilBiomeAvoidanceMidFixer;

                if (num816 > num818 - GenVars.evilBiomeAvoidanceMidFixer)
                    num816 = num818 - GenVars.evilBiomeAvoidanceMidFixer;

                if (num817 < GenVars.dungeonLocation + num786 && num818 > GenVars.dungeonLocation - num786)
                    flag53 = false;

                if (!WorldGen.remixWorldGen) {
                    if (!WorldGen.tenthAnniversaryWorldGen) {
                        if (num816 > num819 - num820 && num816 < num819 + num820)
                            flag53 = false;

                        if (num817 > num819 - num820 && num817 < num819 + num820)
                            flag53 = false;

                        if (num818 > num819 - num820 && num818 < num819 + num820)
                            flag53 = false;
                    }

                    if (num816 > GenVars.UndergroundDesertLocation.X && num816 < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width)
                        flag53 = false;

                    if (num817 > GenVars.UndergroundDesertLocation.X && num817 < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width)
                        flag53 = false;

                    if (num818 > GenVars.UndergroundDesertLocation.X && num818 < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width)
                        flag53 = false;

                    if (num817 < num813 && num818 > num812) {
                        num812++;
                        num813--;
                        flag53 = false;
                    }

                    if (num817 < num815 && num818 > num814) {
                        num814++;
                        num815--;
                        flag53 = false;
                    }
                }
            }

            int num821 = 0;
            for (int num822 = num817; num822 < num818; num822++) {
                if (num821 > 0)
                    num821--;

                if (num822 == num816 || num821 == 0) {
                    for (int num823 = (int)GenVars.worldSurfaceLow; num823 < Main.worldSurface - 1; num823++) {
                        if (Main.tile[num822, num823].HasTile || Main.tile[num822, num823].WallType > 0) {
                            if (num822 == num816) {
                                num821 = 20;
                                FracturedRunner(num822, num823, WorldGen.genRand.Next(150) + 150, makeOrb: true);
                            } else if (WorldGen.genRand.NextBool(35) && num821 == 0) {
                                num821 = 30;
                                bool makeOrb = true;
                                FracturedRunner(num822, num823, WorldGen.genRand.Next(50) + 50, makeOrb);
                            }

                            break;
                        }
                    }
                }

                for (int num824 = (int)GenVars.worldSurfaceLow; num824 < Main.worldSurface - 1; num824++) {
                    if (Main.tile[num822, num824].HasTile) {
                        int num825 = num824 + WorldGen.genRand.Next(10, 14);
                        for (int num826 = num824; num826 < num825; num826++)
                            if (Main.tile[num822, num826].TileType == TileID.JungleGrass && num822 >= num817 + WorldGen.genRand.Next(5) && num822 < num818 - WorldGen.genRand.Next(5))
                                Main.tile[num822, num826].TileType = (ushort)ModContent.TileType<StarvedJungleGrass>();

                        break;
                    }
                }
            }

            double num827 = Main.worldSurface + 40.0;
            for (int num828 = num817; num828 < num818; num828++) {
                num827 += WorldGen.genRand.Next(-2, 3);
                if (num827 < Main.worldSurface + 30.0)
                    num827 = Main.worldSurface + 30.0;

                if (num827 > Main.worldSurface + 50.0)
                    num827 = Main.worldSurface + 50.0;

                bool flag54 = false;
                for (int num829 = (int)GenVars.worldSurfaceLow; num829 < num827; num829++) {
                    if (Main.tile[num828, num829].HasTile) {
                        if (Main.tile[num828, num829].TileType == TileID.Sand && num828 >= num817 + WorldGen.genRand.Next(5) && num828 <= num818 - WorldGen.genRand.Next(5))
                            Main.tile[num828, num829].TileType = (ushort)ModContent.TileType<StarvedSand>();

                        if (num829 < Main.worldSurface - 1 && !flag54) {
                            if (Main.tile[num828, num829].TileType == TileID.Dirt) {
                                WorldGen.grassSpread = 0;
                                WorldGen.SpreadGrass(num828, num829, TileID.Dirt, ModContent.TileType<StarvedGrass>());
                            } else if (Main.tile[num828, num829].TileType == TileID.Mud) {
                                WorldGen.grassSpread = 0;
                                WorldGen.SpreadGrass(num828, num829, TileID.Mud, ModContent.TileType<StarvedJungleGrass>());
                            }
                        }

                        flag54 = true;
                        //if (Main.tile[num828, num829].WallType == WallID.HardenedSand)
                        //    Main.tile[num828, num829].WallType = (ushort)ModContent.WallType<HardendShattersandWall>();
                        //else if (Main.tile[num828, num829].WallType == WallID.Sandstone)
                        //    Main.tile[num828, num829].WallType = (ushort)ModContent.WallType<ShattersandstoneWall>();

                        if (Main.tile[num828, num829].TileType == TileID.Stone) {
                            if (num828 >= num817 + WorldGen.genRand.Next(5) && num828 <= num818 - WorldGen.genRand.Next(5))
                                Main.tile[num828, num829].TileType = (ushort)ModContent.TileType<StarvingStone>();
                        } else if (Main.tile[num828, num829].TileType == TileID.Grass) {
                            Main.tile[num828, num829].TileType = (ushort)ModContent.TileType<StarvedGrass>();
                        } else if (Main.tile[num828, num829].TileType == TileID.JungleGrass) {
                            Main.tile[num828, num829].TileType = (ushort)ModContent.TileType<StarvedJungleGrass>();
                        } else if (Main.tile[num828, num829].TileType == TileID.IceBlock) {
                            Main.tile[num828, num829].TileType = (ushort)ModContent.TileType<StarvedIce>();
                        } else if (Main.tile[num828, num829].TileType == TileID.Sandstone) {
                            Main.tile[num828, num829].TileType = (ushort)ModContent.TileType<StarvedSandstone>();
                        } else if (Main.tile[num828, num829].TileType == TileID.HardenedSand) {
                            Main.tile[num828, num829].TileType = (ushort)ModContent.TileType<HardenedStarvedSand>();
                        }
                    }
                }
            }

            for (int num830 = num817; num830 < num818; num830++) {
                for (int num831 = 0; num831 < Main.maxTilesY - 50; num831++) {
                    if (Main.tile[num830, num831].HasTile && Main.tile[num830, num831].TileType == ModContent.TileType<StarvingEgg>()) {
                        int num832 = num830 - 13;
                        int num833 = num830 + 13;
                        int num834 = num831 - 13;
                        int num835 = num831 + 13;
                        for (int num836 = num832; num836 < num833; num836++) {
                            if (num836 > 10 && num836 < Main.maxTilesX - 10) {
                                for (int num837 = num834; num837 < num835; num837++) {
                                    Tile tile = Main.tile[num836, num837];
                                    if (Math.Abs(num836 - num830) + Math.Abs(num837 - num831) < 9 + WorldGen.genRand.Next(11) && WorldGen.genRand.NextBool(3) && tile.TileType != ModContent.TileType<StarvingEgg>()) {
                                        tile.HasTile = true;
                                        tile.TileType = 25;
                                        if (Math.Abs(num836 - num830) <= 1 && Math.Abs(num837 - num831) <= 1)
                                            tile.HasTile = false;
                                    }

                                    if (tile.TileType != ModContent.TileType<StarvingEgg>() && Math.Abs(num836 - num830) <= 2 + WorldGen.genRand.Next(3) && Math.Abs(num837 - num831) <= 2 + WorldGen.genRand.Next(3))
                                        tile.HasTile = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Starved generation method.
    /// </summary>
    /// <param name="i">The x coordinate to start the generation at.</param>
    /// <param name="j">The y coordinate to start the generation at.</param>
    public static void FracturedRunner(int i, int j, int steps, bool makeOrb = false) {
        bool flag = false;
        bool flag2 = false;
        bool flag3 = false;
        if (!makeOrb)
            flag2 = true;

        double num = steps;
        Vector2D vector2D = default;
        vector2D.X = i;
        vector2D.Y = j;
        Vector2D vector2D2 = default;
        vector2D2.X = WorldGen.genRand.Next(-10, 11) * 0.1;
        vector2D2.Y = WorldGen.genRand.Next(11) * 0.2 + 0.5;
        int num2 = 5;
        double num3 = WorldGen.genRand.Next(5) + 7;
        while (num3 > 0.0) {
            if (num > 0.0) {
                num3 += WorldGen.genRand.Next(3);
                num3 -= WorldGen.genRand.Next(3);
                if (num3 < 7.0)
                    num3 = 7.0;

                if (num3 > 20.0)
                    num3 = 20.0;

                if (num == 1.0 && num3 < 10.0)
                    num3 = 10.0;
            } else if (vector2D.Y > Main.worldSurface + 45.0) {
                num3 -= WorldGen.genRand.Next(4);
            }

            if (vector2D.Y > Main.rockLayer && num > 0.0)
                num = 0.0;

            num -= 1.0;
            if (!flag && vector2D.Y > Main.worldSurface + 20.0) {
                flag = true;
                WorldGen.ChasmRunnerSideways((int)vector2D.X, (int)vector2D.Y, -1, WorldGen.genRand.Next(20, 40));
                WorldGen.ChasmRunnerSideways((int)vector2D.X, (int)vector2D.Y, 1, WorldGen.genRand.Next(20, 40));
            }

            int num4;
            int num5;
            int num6;
            int num7;
            if (num > num2) {
                num4 = (int)(vector2D.X - num3 * 0.5);
                num5 = (int)(vector2D.X + num3 * 0.5);
                num6 = (int)(vector2D.Y - num3 * 0.5);
                num7 = (int)(vector2D.Y + num3 * 0.5);
                if (num4 < 0)
                    num4 = 0;

                if (num5 > Main.maxTilesX - 1)
                    num5 = Main.maxTilesX - 1;

                if (num6 < 0)
                    num6 = 0;

                if (num7 > Main.maxTilesY)
                    num7 = Main.maxTilesY;

                for (int k = num4; k < num5; k++) {
                    for (int l = num6; l < num7; l++) {
                        Tile tile = Main.tile[k, l];
                        if (Math.Abs(k - vector2D.X) + Math.Abs(l - vector2D.Y) < num3 * 0.5 * (1.0 + WorldGen.genRand.Next(-10, 11) * 0.015) && tile.TileType != ModContent.TileType<StarvingEgg>() && tile.TileType != ModContent.TileType<Hungrite>())
                            tile.HasTile = false;
                    }
                }
            }

            if (num <= 2.0 && vector2D.Y < Main.worldSurface + 45.0)
                num = 2.0;

            if (num <= 0.0) {
                if (!flag2) {
                    flag2 = true;
                    AddStarvingEgg((int)vector2D.X, (int)vector2D.Y);
                } else if (!flag3) {
                    flag3 = false;
                    bool flag4 = false;
                    int num8 = 0;
                    while (!flag4) {
                        int num9 = WorldGen.genRand.Next((int)vector2D.X - 25, (int)vector2D.X + 25);
                        int num10 = WorldGen.genRand.Next((int)vector2D.Y - 50, (int)vector2D.Y);
                        if (num9 < 5)
                            num9 = 5;

                        if (num9 > Main.maxTilesX - 5)
                            num9 = Main.maxTilesX - 5;

                        if (num10 < 5)
                            num10 = 5;

                        if (num10 > Main.maxTilesY - 5)
                            num10 = Main.maxTilesY - 5;

                        if (num10 > Main.worldSurface) {
                            if (!WorldGen.IsTileNearby(num9, num10, ModContent.TileType<AltarOfTheStarvingOne>(), 3))
                                WorldGen.Place3x2(num9, num10, (ushort)ModContent.TileType<AltarOfTheStarvingOne>());

                            if (Main.tile[num9, num10].TileType == ModContent.TileType<AltarOfTheStarvingOne>()) {
                                flag4 = true;
                                continue;
                            }

                            num8++;
                            if (num8 >= 10000)
                                flag4 = true;
                        } else {
                            flag4 = true;
                        }
                    }
                }
            }

            vector2D += vector2D2;
            vector2D2.X += WorldGen.genRand.Next(-10, 11) * 0.01;
            if (vector2D2.X > 0.3)
                vector2D2.X = 0.3;

            if (vector2D2.X < -0.3)
                vector2D2.X = -0.3;

            num4 = (int)(vector2D.X - num3 * 1.1);
            num5 = (int)(vector2D.X + num3 * 1.1);
            num6 = (int)(vector2D.Y - num3 * 1.1);
            num7 = (int)(vector2D.Y + num3 * 1.1);
            if (num4 < 1)
                num4 = 1;

            if (num5 > Main.maxTilesX - 1)
                num5 = Main.maxTilesX - 1;

            if (num6 < 0)
                num6 = 0;

            if (num7 > Main.maxTilesY)
                num7 = Main.maxTilesY;

            for (int m = num4; m < num5; m++) {
                for (int n = num6; n < num7; n++) {
                    if (Math.Abs(m - vector2D.X) + Math.Abs(n - vector2D.Y) < num3 * 1.1 * (1.0 + WorldGen.genRand.Next(-10, 11) * 0.015)) {
                        Tile tile = Main.tile[m, n];
                        if (tile.TileType != ModContent.TileType<StarvingStone>() && n > j + WorldGen.genRand.Next(3, 20))
                            tile.HasTile = true;

                        if (steps <= num2)
                            tile.HasTile = true;

                        if (tile.TileType != ModContent.TileType<StarvingEgg>())
                            tile.TileType = (ushort)ModContent.TileType<StarvingStone>();
                    }
                }
            }

            for (int num11 = num4; num11 < num5; num11++) {
                for (int num12 = num6; num12 < num7; num12++) {
                    if (Math.Abs(num11 - vector2D.X) + Math.Abs(num12 - vector2D.Y) < num3 * 1.1 * (1.0 + WorldGen.genRand.Next(-10, 11) * 0.015)) {
                        Tile tile = Main.tile[num11, num12];
                        if (tile.TileType != ModContent.TileType<StarvingEgg>())
                            tile.TileType = (ushort)ModContent.TileType<StarvingStone>();

                        if (steps <= num2)
                            tile.HasTile = true;

                        if (num12 > j + WorldGen.genRand.Next(3, 20))
                            tile.WallType = (ushort)ModContent.WallType<Walls.Starved.StarvedUnsafe1>();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adds a Hungry Egg at the given coordinates
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    /// <param name="style">Unused.</param>
    public static void AddStarvingEgg(int x, int y, int style = 0) {
        if (x < 10 || x > Main.maxTilesX - 10)
            return;
        if (y < 10 || y > Main.maxTilesY - 10)
            return;
        for (int i = x - 1; i < x + 1; i++)
            for (int j = y - 1; j < y + 1; j++)
                if (Main.tile[i, j].HasTile && Main.tile[i, j].TileType == (ushort)ModContent.TileType<StarvingEgg>())
                    return;
        short num = 0;
        Tile bottomLeft = Main.tile[x - 1, y - 1];
        bottomLeft.HasTile = true;
        bottomLeft.TileType = (ushort)ModContent.TileType<StarvingEgg>();
        bottomLeft.TileFrameX = num;
        bottomLeft.TileFrameY = 0;
        Tile bottomRight = Main.tile[x, y - 1];
        bottomRight.HasTile = true;
        bottomRight.TileType = (ushort)ModContent.TileType<StarvingEgg>();
        bottomRight.TileFrameX = (short)(18 + num);
        bottomRight.TileFrameY = 0;
        Tile topLeft = Main.tile[x - 1, y];
        topLeft.HasTile = true;
        topLeft.TileType = (ushort)ModContent.TileType<StarvingEgg>();
        topLeft.TileFrameX = num;
        topLeft.TileFrameY = 18;
        Tile topRight = Main.tile[x, y];
        topRight.HasTile = true;
        topRight.TileType = (ushort)ModContent.TileType<StarvingEgg>();
        topRight.TileFrameX = (short) (18 + num);
        topRight.TileFrameY = 18;
    }
}
