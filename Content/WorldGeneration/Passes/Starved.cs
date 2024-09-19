﻿using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using Microsoft.Xna.Framework;
using TheGreatSidegrade.Content.Evils.Starved.Tiles;
using TheGreatSidegrade.Content.Evils.Starved.Walls;

namespace TheGreatSidegrade.Content.WorldGeneration.Passes {
    public class Starved {
        public static void Method(GenerationProgress progress, GameConfiguration configuration) {
            progress.Message = "The world hungers";
            int num208 = 0;
            while (num208 < Main.maxTilesX * 0.00045) {
                float num209 = (float)(num208 / (Main.maxTilesX * 0.00045));
                bool flag12 = false;
                int num210 = 0;
                int num211 = 0;
                int num212 = 0;
                while (!flag12) {
                    int num213 = 0;
                    flag12 = true;
                    int num214 = Main.maxTilesX / 2;
                    int num215 = 200;
                    if (GenVars.dungeonX < Main.maxTilesX * 0.5)
                        num210 = WorldGen.genRand.Next(600, Main.maxTilesX - 320);
                    else
                        num210 = WorldGen.genRand.Next(320, Main.maxTilesX - 600);
                    num211 = num210 - WorldGen.genRand.Next(200) - 100;
                    num212 = num210 + WorldGen.genRand.Next(200) + 100;
                    if (num211 < 285)
                        num211 = 285;
                    if (num212 > Main.maxTilesX - 285) {
                        num212 = Main.maxTilesX - 285;
                    }
                    if (GenVars.dungeonX < 0 && num211 < 400)
                        num211 = 400;
                    else if (GenVars.dungeonX > 0 && num211 > Main.maxTilesX - 400)
                        num211 = Main.maxTilesX - 400;
                    if (num210 > num214 - num215 && num210 < num214 + num215)
                        flag12 = false;
                    if (num211 > num214 - num215 && num211 < num214 + num215)
                        flag12 = false;
                    if (num212 > num214 - num215 && num212 < num214 + num215)
                        flag12 = false;
                    if (num210 > GenVars.UndergroundDesertLocation.X && num210 < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width)
                        flag12 = false;
                    if (num211 > GenVars.UndergroundDesertLocation.X && num211 < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width)
                        flag12 = false;
                    if (num212 > GenVars.UndergroundDesertLocation.X && num212 < GenVars.UndergroundDesertLocation.X + GenVars.UndergroundDesertLocation.Width)
                        flag12 = false;
                    for (int num216 = num211; num216 < num212; num216++) {
                        for (int num217 = 0; num217 < (int)Main.worldSurface; num217 += 5) {
                            if (Main.tile[num216, num217].HasTile && Main.tileDungeon[Main.tile[num216, num217].TileType]) {
                                flag12 = false;
                                break;
                            }
                            if (!flag12)
                                break;
                        }
                    }
                    if (num213 < 200 && GenVars.JungleX > num211 && GenVars.JungleX < num212) {
                        num213++;
                        flag12 = false;
                    }
                }
                StarvedRunner(num210, (int)GenVars.worldSurfaceLow - 10 + (Main.maxTilesY / 8));
                for (int num218 = num211; num218 < num212; num218++) {
                    int num219 = (int)GenVars.worldSurfaceLow;
                    while (num219 < Main.worldSurface - 1.0) {
                        if (Main.tile[num218, num219].HasTile) {
                            int num220 = num219 + WorldGen.genRand.Next(10, 14);
                            for (int num221 = num219; num221 < num220; num221++)
                                if ((Main.tile[num218, num221].TileType == TileID.Mud || Main.tile[num218, num221].TileType == TileID.JungleGrass) && num218 >= num211 + WorldGen.genRand.Next(5) && num218 < num212 - WorldGen.genRand.Next(5))
                                    Main.tile[num218, num221].TileType = TileID.Dirt;
                            break;
                        }
                        num219++;
                    }
                }
                double num222 = Main.worldSurface + 40.0;
                for (int num223 = num211; num223 < num212; num223++) {
                    num222 += WorldGen.genRand.Next(-2, 3);
                    if (num222 < Main.worldSurface + 30.0)
                        num222 = Main.worldSurface + 30.0;
                    if (num222 > Main.worldSurface + 50.0)
                        num222 = Main.worldSurface + 50.0;
                    int num57 = num223;
                    bool flag13 = false;
                    int num224 = (int)GenVars.worldSurfaceLow;
                    while (num224 < num222) {
                        if (Main.tile[num57, num224].HasTile) {
                            if (Main.tile[num57, num224].TileType == TileID.Sand && num57 >= num211 + WorldGen.genRand.Next(5) && num57 <= num212 - WorldGen.genRand.Next(5))
                                Main.tile[num57, num224].TileType = (ushort)ModContent.TileType<StarvedSand>();
                            if (Main.tile[num57, num224].TileType == TileID.Dirt && num224 < Main.worldSurface - 1.0 && !flag13) {
                                //ExxoAvalonOriginsWorld.grassSpread = 0;
                                WorldGen.SpreadGrass(num57, num224, 0, ModContent.TileType<StarvedGrass>(), true, default);
                            }
                            flag13 = true;
                            if (Main.tile[num57, num224].TileType == TileID.Stone && num57 >= num211 + WorldGen.genRand.Next(5) && num57 <= num212 - WorldGen.genRand.Next(5))
                                Main.tile[num57, num224].TileType = (ushort)ModContent.TileType<StarvingStone>();
                            if (Main.tile[num57, num224].TileType == TileID.Grass)
                                Main.tile[num57, num224].TileType = (ushort)ModContent.TileType<StarvedGrass>();
                            if (Main.tile[num57, num224].TileType == TileID.IceBlock)
                                Main.tile[num57, num224].TileType = (ushort)ModContent.TileType<StarvedIce>();
                            if (Main.tile[num57, num224].TileType == TileID.HardenedSand)
                                Main.tile[num57, num224].TileType = (ushort)ModContent.TileType<HardenedStarvedSand>();
                            if (Main.tile[num57, num224].TileType == TileID.Sandstone)
                                Main.tile[num57, num224].TileType = (ushort)ModContent.TileType<StarvedSandstone>();
                        }
                        num224++;
                    }
                }
                int num225 = WorldGen.genRand.Next(10, 15);
                for (int num226 = 0; num226 < num225; num226++) {
                    int num227 = 0;
                    bool flag14 = false;
                    int num228 = 0;
                    while (!flag14) {
                        num227++;
                        int num229 = WorldGen.genRand.Next(num211 - num228, num212 + num228);
                        int num230 = WorldGen.genRand.Next((int)(Main.worldSurface - num228 / 2), (int)(Main.worldSurface + 100.0 + num228));
                        if (num227 > 100) {
                            num228++;
                            num227 = 0;
                        }
                        if (!Main.tile[num229, num230].HasTile) {
                            while (!Main.tile[num229, num230].HasTile)
                                num230++;
                            num230--;
                        } else {
                            while (Main.tile[num229, num230].HasTile && num230 > Main.worldSurface)
                                num230--;
                        }
                        if (num228 > 10 || (Main.tile[num229, num230 + 1].HasTile && Main.tile[num229, num230 + 1].TileType == TileID.Crimstone)) {
                            WorldGen.Place3x2(num229, num230, (ushort)ModContent.TileType<AltarOfTheStarvingOne>());
                            if (Main.tile[num229, num230].TileType == (ushort)ModContent.TileType<AltarOfTheStarvingOne>())
                                flag14 = true;
                        }
                        if (num228 > 100)
                            flag14 = true;
                    }
                }
                num208++;
            }
        }
        /// <summary>
        /// Starved generation method.
        /// </summary>
        /// <param name="i">The x coordinate to start the generation at.</param>
        /// <param name="j">The y coordinate to start the generation at.</param>
        private static void StarvedRunner(int i, int j) {
            int j2 = j;
            int radius = WorldGen.genRand.Next(50, 61);
            int rad2 = WorldGen.genRand.Next(20, 26);
            j = Utils.TileCheck(i) + radius + 50;
            Vector2 center = new(i, j);
            List<Vector2> points = [];
            List<Vector2> pointsToGoTo = [];
            List<double> angles = [];
            List<Vector2> outerCircles = []; // the circles at the ends of the first tunnels
            List<Vector2> secondaryCircles = []; // the circles at the ends of the outer circles
            List<Vector2> secondCircleStartPoints = [];
            List<Vector2> secondCircleEndpoints = [];
            List<double> secondCirclePointsAroundCircle = [];
            List<Vector2> exclusions = [];
            List<Vector2> excludedPointsForOuterTunnels = [];
            #region make the main circle
            for (int k = i - radius; k <= i + radius; k++) {
                for (int l = j - radius; l <= j + radius; l++) {
                    float dist = Vector2.Distance(new(k, l), new(i, j));
                    Tile tile = Main.tile[k, l];
                    if (dist <= radius && dist >= (radius - 29))
                        tile.HasTile = false;
                    if (((dist <= radius && dist >= radius - 7) || (dist <= radius - 22 && dist >= radius - 29)) && Main.tile[k, l].TileType != (ushort)ModContent.TileType<StarvingEgg>()) {
                        tile.HasTile = true;
                        tile.IsHalfBlock = false;
                        tile.Slope = SlopeType.Solid;
                        tile.TileType = (ushort)ModContent.TileType<StarvingStone>();
                    }
                    if (dist <= radius - 6 && dist >= radius - 23)
                        tile.WallType = (ushort)ModContent.WallType<StarvingStoneWall>();
                }
            }
            #endregion
            int radiusModifier = radius - 7; // makes the tunnels go deeper into the main circle (more subtracted means further in)
            Vector2 posToPlaceAnotherCircle = Vector2.Zero;
            #region find the points for making the tunnels to the outer circles
            for (int m = 0; m < 16; m++) {
                double positionAroundCircle = (WorldGen.genRand.Next(0, 62831852) / 10000000);
                Vector2 randPoint = new(center.X + ((int)Math.Round(radiusModifier * Math.Cos(positionAroundCircle))), center.Y + ((int)Math.Round(radiusModifier * Math.Sin(positionAroundCircle))));
                posToPlaceAnotherCircle = randPoint;
                Vector2 item2 = center;
                if (randPoint.X > center.X) {
                    if (randPoint.X > center.X + radius / 2) {
                        if (randPoint.Y > center.Y) {
                            if (randPoint.Y > center.Y + radius / 2) {
                                item2 = new(randPoint.X + 50f, randPoint.Y + 50f);
                                if (WorldGen.genRand.NextBool(2)) {
                                    outerCircles.Add(item2);
                                    secondaryCircles.Add(item2);
                                    excludedPointsForOuterTunnels.Add(randPoint);
                                }
                            } else {
                                item2 = new(randPoint.X + 50f, randPoint.Y + 25f);
                                if (WorldGen.genRand.NextBool(2)) {
                                    outerCircles.Add(item2);
                                    secondaryCircles.Add(item2);
                                    excludedPointsForOuterTunnels.Add(randPoint);
                                }
                            }
                        } else if (randPoint.Y < center.Y - radius / 2) {
                            item2 = new(randPoint.X + 50f, randPoint.Y - 50f);
                            if (WorldGen.genRand.NextBool(2)) {
                                outerCircles.Add(item2);
                                secondaryCircles.Add(item2);
                                excludedPointsForOuterTunnels.Add(randPoint);
                            }
                        } else {
                            item2 = new(randPoint.X + 50f, randPoint.Y - 25f);
                            if (WorldGen.genRand.NextBool(2)) {
                                outerCircles.Add(item2);
                                secondaryCircles.Add(item2);
                                excludedPointsForOuterTunnels.Add(randPoint);
                            }
                        }
                    } else if (randPoint.Y > center.Y) {
                        if (randPoint.Y > center.Y + radius / 2) {
                            item2 = new(randPoint.X + 25f, randPoint.Y + 50f);
                            if (WorldGen.genRand.NextBool(2)) {
                                outerCircles.Add(item2);
                                secondaryCircles.Add(item2);
                                excludedPointsForOuterTunnels.Add(randPoint);
                            }
                        } else {
                            item2 = new(randPoint.X + 25f, randPoint.Y + 25f);
                            if (WorldGen.genRand.NextBool(2)) {
                                outerCircles.Add(item2);
                                secondaryCircles.Add(item2);
                                excludedPointsForOuterTunnels.Add(randPoint);
                            }
                        }
                    } else if (randPoint.Y < center.Y - radius / 2) {
                        item2 = new(randPoint.X + 25f, randPoint.Y - 50f);
                        if (WorldGen.genRand.NextBool(2)) {
                            outerCircles.Add(item2);
                            secondaryCircles.Add(item2);
                            excludedPointsForOuterTunnels.Add(randPoint);
                        }
                    } else {
                        item2 = new(randPoint.X + 25f, randPoint.Y - 25f);
                        if (WorldGen.genRand.NextBool(2)) {
                            outerCircles.Add(item2);
                            secondaryCircles.Add(item2);
                            excludedPointsForOuterTunnels.Add(randPoint);
                        }
                    }
                } else if (randPoint.X < center.X - radius / 2) {
                    if (randPoint.Y > center.Y) {
                        if (randPoint.Y > center.Y + radius / 2) {
                            item2 = new(randPoint.X - 50f, randPoint.Y + 50f);
                            if (WorldGen.genRand.NextBool(2)) {
                                outerCircles.Add(item2);
                                secondaryCircles.Add(item2);
                                excludedPointsForOuterTunnels.Add(randPoint);
                            }
                        } else {
                            item2 = new(randPoint.X - 50f, randPoint.Y + 25f);
                            if (WorldGen.genRand.NextBool(2)) {
                                outerCircles.Add(item2);
                                secondaryCircles.Add(item2);
                                excludedPointsForOuterTunnels.Add(randPoint);
                            }
                        }
                    } else if (randPoint.Y < center.Y - radius / 2) {
                        item2 = new(randPoint.X - 50f, randPoint.Y - 50f);
                        if (WorldGen.genRand.NextBool(2)) {
                            outerCircles.Add(item2);
                            secondaryCircles.Add(item2);
                            excludedPointsForOuterTunnels.Add(randPoint);
                        }
                    } else {
                        item2 = new(randPoint.X - 50f, randPoint.Y - 25f);
                        if (WorldGen.genRand.NextBool(2)) {
                            outerCircles.Add(item2);
                            secondaryCircles.Add(item2);
                            excludedPointsForOuterTunnels.Add(randPoint);
                        }
                    }
                } else if (randPoint.Y > center.Y) {
                    if (randPoint.Y > center.Y + radius / 2) {
                        item2 = new(randPoint.X - 25f, randPoint.Y + 50f);
                        if (WorldGen.genRand.NextBool(2)) {
                            outerCircles.Add(item2);
                            secondaryCircles.Add(item2);
                            excludedPointsForOuterTunnels.Add(randPoint);
                        }
                    } else {
                        item2 = new(randPoint.X - 25f, randPoint.Y + 25f);
                        if (WorldGen.genRand.NextBool(2)) {
                            outerCircles.Add(item2);
                            secondaryCircles.Add(item2);
                            excludedPointsForOuterTunnels.Add(randPoint);
                        }
                    }
                } else if (randPoint.Y < center.Y - radius / 2) {
                    item2 = new(randPoint.X - 25f, randPoint.Y - 50f);
                    if (WorldGen.genRand.NextBool(2)) {
                        outerCircles.Add(item2);
                        secondaryCircles.Add(item2);
                        excludedPointsForOuterTunnels.Add(randPoint);
                    }
                } else {
                    item2 = new(randPoint.X - 25f, randPoint.Y - 25f);
                    if (WorldGen.genRand.NextBool(2)) {
                        outerCircles.Add(item2);
                        secondaryCircles.Add(item2);
                        excludedPointsForOuterTunnels.Add(randPoint);
                    }
                }
                points.Add(randPoint);
                pointsToGoTo.Add(item2);
                angles.Add(positionAroundCircle);
            }
            #endregion

            // make outer circles
            #region outer circles and tunnels
            if (secondaryCircles.Count != 0) {
                for (int z = 0; z < secondaryCircles.Count; z++) {
                    if (secondaryCircles[z].Y < center.Y - 10) continue;
                    int outerTunnelsRadiusMod = rad2 - 6;
                    double pointsAroundCircle2 = (WorldGen.genRand.Next(0, 62831852) / 10000000);
                    Vector2 randPointAroundCircle = new(outerCircles[z].X + ((int)Math.Round(outerTunnelsRadiusMod * Math.Cos(pointsAroundCircle2))), outerCircles[z].Y + ((int)Math.Round(outerTunnelsRadiusMod * Math.Sin(pointsAroundCircle2))));
                    for (int m = 0; m < 16; m++) {
                        Vector2 endpoint = secondaryCircles[z];
                        #region endpoint calculation
                        if (randPointAroundCircle.X > outerCircles[z].X) {
                            if (randPointAroundCircle.X > outerCircles[z].X + rad2 / 2) {
                                if (randPointAroundCircle.Y > outerCircles[z].Y) {
                                    if (randPointAroundCircle.Y > outerCircles[z].Y + rad2 / 2)
                                        endpoint = new(randPointAroundCircle.X + 15f, randPointAroundCircle.Y + 15f);
                                    else
                                        endpoint = new(randPointAroundCircle.X + 15f, randPointAroundCircle.Y + 7f);
                                } else if (randPointAroundCircle.Y < outerCircles[z].Y - rad2 / 2) {
                                    endpoint = new(randPointAroundCircle.X + 15f, randPointAroundCircle.Y - 15f);
                                } else {
                                    endpoint = new(randPointAroundCircle.X + 15f, randPointAroundCircle.Y - 7f);
                                }
                            } else if (randPointAroundCircle.Y > outerCircles[z].Y) {
                                if (randPointAroundCircle.Y > outerCircles[z].Y + rad2 / 2)
                                    endpoint = new(randPointAroundCircle.X + 7f, randPointAroundCircle.Y + 15f);
                                else
                                    endpoint = new(randPointAroundCircle.X + 7f, randPointAroundCircle.Y + 7f);
                            } else if (randPointAroundCircle.Y < outerCircles[z].Y - rad2 / 2) {
                                endpoint = new(randPointAroundCircle.X + 7f, randPointAroundCircle.Y - 15f);
                            } else {
                                endpoint = new(randPointAroundCircle.X + 7f, randPointAroundCircle.Y - 7f);
                            }
                        } else if (randPointAroundCircle.X < outerCircles[z].X - rad2 / 2) {
                            if (randPointAroundCircle.Y > outerCircles[z].Y) {
                                if (randPointAroundCircle.Y > outerCircles[z].Y + rad2 / 2)
                                    endpoint = new(randPointAroundCircle.X - 15f, randPointAroundCircle.Y + 15f);
                                else
                                    endpoint = new(randPointAroundCircle.X - 15f, randPointAroundCircle.Y + 7f);
                            } else if (randPointAroundCircle.Y < outerCircles[z].Y - rad2 / 2) {
                                endpoint = new(randPointAroundCircle.X - 15f, randPointAroundCircle.Y - 15f);
                            } else {
                                endpoint = new(randPointAroundCircle.X - 15f, randPointAroundCircle.Y - 7f);
                            }
                        } else if (randPointAroundCircle.Y > outerCircles[z].Y) {
                            if (randPointAroundCircle.Y > outerCircles[z].Y + rad2 / 2)
                                endpoint = new(randPointAroundCircle.X - 7f, randPointAroundCircle.Y + 15f);
                            else
                                endpoint = new(randPointAroundCircle.X - 7f, randPointAroundCircle.Y + 7f);
                        } else if (randPointAroundCircle.Y < outerCircles[z].Y - rad2 / 2) {
                            endpoint = new(randPointAroundCircle.X - 7f, randPointAroundCircle.Y - 15f);
                        } else {
                            endpoint = new(randPointAroundCircle.X - 7f, randPointAroundCircle.Y - 7f);
                        }
                        #endregion
                        secondCircleStartPoints.Add(randPointAroundCircle);
                        secondCircleEndpoints.Add(endpoint);
                        secondCirclePointsAroundCircle.Add(pointsAroundCircle2);
                    }
                }
            }

            #endregion
            // make tunnels going outwards from the main circle
            for (int n = 0; n < points.Count; n++) {
                if (points[n].Y < center.Y - 10) continue;
                BoreTunnel2((int)points[n].X, (int)points[n].Y, (int)pointsToGoTo[n].X, (int)pointsToGoTo[n].Y, 10f, (ushort)ModContent.TileType<StarvingStone>());
                BoreTunnel2((int)points[n].X, (int)points[n].Y, (int)pointsToGoTo[n].X, (int)pointsToGoTo[n].Y, 5f, 65535);
                MakeEndingCircle((int)pointsToGoTo[n].X, (int)pointsToGoTo[n].Y, 13f, (ushort)ModContent.TileType<StarvingStone>());
                MakeCircle((int)pointsToGoTo[n].X, (int)pointsToGoTo[n].Y, 8f, 65535);
            }
            if (outerCircles.Count != 0) {
                for (int q = 0; q < outerCircles.Count; q++) {
                    if (outerCircles[q].Y < center.Y - 10)
                        continue;
                    MakeEndingCircle((int)outerCircles[q].X, (int)outerCircles[q].Y, rad2, (ushort)ModContent.TileType<StarvingStone>());
                    MakeCircle((int)outerCircles[q].X, (int)outerCircles[q].Y, rad2 - 6, 65535);
                    MakeCircle((int)outerCircles[q].X, (int)outerCircles[q].Y, rad2 - 13, (ushort)ModContent.TileType<StarvingStone>());
                    exclusions.Add(outerCircles[q]);
                }
            }
            int num8 = radius - 7;
            for (int num9 = 0; num9 < 20; num9++) {
                double d = WorldGen.genRand.Next(0, 62831852) / 10000000;
                Vector2 vector2 = new(center.X + ((int)Math.Round(num8 * Math.Cos(d))), center.Y + ((int)Math.Round(num8 * Math.Sin(d))));
                if (exclusions.Contains(vector2))
                    continue;
                MakeCircle((int)vector2.X, (int)vector2.Y, 4f, (ushort)ModContent.TileType<StarvingStone>());
            }

            // make tunnels going outwards from the outer circles
            for (int n = 0; n < secondCircleStartPoints.Count; n++) {
                if (excludedPointsForOuterTunnels.Count != 0 && n < excludedPointsForOuterTunnels.Count)
                    if (Vector2.Distance(excludedPointsForOuterTunnels[n], secondCircleEndpoints[n]) < 55)
                        continue;
                BoreTunnel2((int)secondCircleStartPoints[n].X, (int)secondCircleStartPoints[n].Y, (int)secondCircleEndpoints[n].X, (int)secondCircleEndpoints[n].Y, 7f, (ushort)ModContent.TileType<StarvingStone>());
                BoreTunnel2((int)secondCircleStartPoints[n].X, (int)secondCircleStartPoints[n].Y, (int)secondCircleEndpoints[n].X, (int)secondCircleEndpoints[n].Y, 3f, 65535);
                // ending circles
                MakeCircle((int)secondCircleEndpoints[n].X, (int)secondCircleEndpoints[n].Y, 3f, 65535); // air
                MakeEndingCircle((int)secondCircleEndpoints[n].X, (int)secondCircleEndpoints[n].Y, 5f, (ushort)ModContent.TileType<StarvingStone>()); // starving stone
            }
            // fill main tunnels with air
            for (int n = 0; n < points.Count; n++) {
                if (points[n].Y < center.Y - 10) {
                    exclusions.Add(pointsToGoTo[n]);
                    continue;
                }
                BoreTunnel2((int)points[n].X, (int)points[n].Y, (int)pointsToGoTo[n].X, (int)pointsToGoTo[n].Y, 3f, 65535);
            }
            // make secondary circles inner area filled
            if (outerCircles.Count != 0) {
                for (int q = 0; q < outerCircles.Count; q++) {
                    if (outerCircles[q].Y < center.Y - 10) continue;
                    MakeCircle((int)outerCircles[q].X, (int)outerCircles[q].Y, rad2 - 6, 65535);
                    MakeCircle((int)outerCircles[q].X, (int)outerCircles[q].Y, rad2 - 13, (ushort)ModContent.TileType<StarvingStone>());
                }
            }
            for (int num5 = i - radius; num5 <= i + radius; num5++) {
                for (int num6 = j - radius; num6 <= j + radius; num6++) {
                    float num7 = Vector2.Distance(new(num5, num6), new(i, j));
                    Tile tile = Main.tile[num5, num6];
                    if (num7 < radius - 7 && num7 > radius - 22)
                        tile.HasTile = false;
                }
            }
            for (int num10 = 0; num10 < pointsToGoTo.Count; num10++) {
                if (exclusions.Contains(pointsToGoTo[num10]))
                    continue;
                AddStarvingEgg((int)pointsToGoTo[num10].X, (int)pointsToGoTo[num10].Y);
            }
            for (int num10 = 0; num10 < secondCircleEndpoints.Count; num10++) {
                if (exclusions.Contains(secondCircleEndpoints[num10]))
                    continue;
                AddStarvingEgg((int)secondCircleEndpoints[num10].X, (int)secondCircleEndpoints[num10].Y);
            }
            BoreTunnel2(i, j - radius - 50, i, j - radius + 7, 5, ushort.MaxValue);
            for (int x = i - 12; x < i + 12; x++) {
                for (int y = j - radius - 50; y < j - radius + 8; y++) {
                    Tile tile = Main.tile[x, y];
                    if (x >= i + 7 || x <= i - 7) {
                        tile.HasTile = true;
                        tile.IsHalfBlock = false;
                        tile.Slope = SlopeType.Solid;
                        tile.TileType = (ushort)ModContent.TileType<StarvingStone>();
                    }
                    if (x <= i + 7 && x >= i - 7) {
                        tile.WallType = (ushort)ModContent.WallType<StarvingStoneWall>();
                        tile.HasTile = false;
                    }
                }
            }
            for (int x = i - 12; x < i + 12; x++) {
                for (int y = j - radius - 50; y < j - radius + 8; y++) {
                    if (x == i + 9 || x == i - 9) {
                        int rn = WorldGen.genRand.Next(13, 17);
                        if (y % rn == 0)
                            MakeCircle(x, y, 3, (ushort)ModContent.TileType<StarvingStone>());
                    }
                }
            }
        }
        /// <summary>
        /// A helper method to generate a tunnel using MakeCircle().
        /// </summary>
        /// <param name="x0">Starting x coordinate.</param>
        /// <param name="y0">Starting y coordinate.</param>
        /// <param name="x1">Ending x coordinate.</param>
        /// <param name="y1">Ending y coordinate.</param>
        /// <param name="r">Radius.</param>
        /// <param name="type">Type to generate.</param>
        public static void BoreTunnel2(int x0, int y0, int x1, int y1, float r, ushort type) {
            bool flag = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (flag) {
                Utils.Swap(ref x0, ref y0);
                Utils.Swap(ref x1, ref y1);
            }
            if (x0 > x1) {
                Utils.Swap(ref x0, ref x1);
                Utils.Swap(ref y0, ref y1);
            }
            int num = x1 - x0;
            int num2 = Math.Abs(y1 - y0);
            int num3 = num / 2;
            int num4 = (y0 < y1) ? 1 : -1;
            int num5 = y0;
            for (int i = x0; i <= x1; i++) {
                if (flag)
                    MakeCircle(num5, i, r, type);
                else
                    MakeCircle(i, num5, r, type);
                num3 -= num2;
                if (num3 < 0) {
                    num5 += num4;
                    num3 += num;
                }
            }
        }
        /// <summary>
        /// Makes a circle for the Contagion generation. Fills all tiles with Chunkstone Walls.
        /// </summary>
        /// <param name="x">The x coordinate of the center of the circle.</param>
        /// <param name="y">The y coordinate of the center of the circle.</param>
        /// <param name="r">The radius of the circle.</param>
        /// <param name="type">The type to generate - if ushort.MaxValue, will generate air.</param>
        public static void MakeCircle(int x, int y, float r, ushort type) {
            int num = (int)(x - r);
            int num2 = (int)(y - r);
            int num3 = (int)(x + r);
            int num4 = (int)(y + r);
            for (int i = num; i < num3 + 1; i++) {
                for (int j = num2; j < num4 + 1; j++) {
                    Tile tile = Main.tile[i, j];
                    if (Vector2.Distance(new(i, j), new(x, y)) <= r && tile.TileType != TileID.ShadowOrbs) {
                        if (type == 65535) {
                            tile.HasTile = false;
                            tile.WallType = (ushort)ModContent.WallType<StarvingStoneWall>();
                        } else {
                            tile.HasTile = true;
                            tile.TileType = type;
                            tile.WallType = (ushort)ModContent.WallType<StarvingStoneWall>();
                            WorldGen.SquareTileFrame(i, j, true);
                        }
                    }
                    //else if (Vector2.Distance(new(i, j), new(x, y)) == r - 1)
                    //   tile.wall = 0;
                }
            }
        }
        /// <summary>
        /// Makes an ending circle for the Starved generation.
        /// </summary>
        /// <param name="x">The x coordinate of the center of the circle.</param>
        /// <param name="y">The y coordinate of the center of the circle.</param>
        /// <param name="r">The radius of the circle.</param>
        /// <param name="type">The type to generate - if ushort.MaxValue, will generate air.</param>
        public static void MakeEndingCircle(int x, int y, float r, ushort type) {
            int num = (int)(x - r);
            int num2 = (int)(y - r);
            int num3 = (int)(x + r);
            int num4 = (int)(y + r);
            for (int i = num; i < num3 + 1; i++) {
                for (int j = num2; j < num4 + 1; j++) {
                    Tile tile = Main.tile[i, j];
                    if (Vector2.Distance(new Vector2(i, j), new Vector2(x, y)) <= r && tile.TileType != TileID.ShadowOrbs) {
                        if (type == 65535) {
                            tile.HasTile = false;
                            tile.WallType = (ushort)ModContent.WallType<StarvingStoneWall>();
                        } else {
                            tile.HasTile = true;
                            tile.TileType = type;
                            WorldGen.SquareTileFrame(i, j, true);
                        }
                    } else if (Vector2.Distance(new Vector2(i, j), new Vector2(x, y)) == r - 1) {
                        tile.WallType = (ushort)ModContent.WallType<StarvingStoneWall>();
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
            topRight.TileFrameX = (short)(18 + num);
            topRight.TileFrameY = 18;
        }
    }
}
