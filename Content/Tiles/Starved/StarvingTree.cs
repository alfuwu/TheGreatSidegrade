﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.GameContent;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria;
using static Terraria.WorldGen;
using static Terraria.GameContent.TilePaintSystemV2;
using TheGreatSidegrade.Common;
using Microsoft.Xna.Framework;
using TheGreatSidegrade.Content.Gores;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvingTree : ModTile {
    public static readonly GrowTreeSettings Tree_Starved = new() {
        GroundTest = StarvewoodTreeGroundTest,
        WallTest = DefaultTreeWallTest,
        TreeHeightMax = 12,
        TreeHeightMin = 7,
        TreeTileType = (ushort)ModContent.TileType<StarvingTree>(),
        TreeTopPaddingNeeded = 4,
        SaplingTileType = (ushort)ModContent.TileType<StarvingSapling>()
    };

    public static readonly TreePaintingSettings TreeStarved = new() {
        UseSpecialGroups = true,
        SpecialGroupMinimalHueValue = 11f / 72f,
        SpecialGroupMaximumHueValue = 0.25f,
        SpecialGroupMinimumSaturationValue = 0.88f,
        SpecialGroupMaximumSaturationValue = 1f
    };

    public override void SetStaticDefaults() {
        Main.tileAxe[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = false;
        Main.tileLavaDeath[Type] = false;
        TileID.Sets.IsATreeTrunk[Type] = true;
        TileID.Sets.IsShakeable[Type] = true;
        TileID.Sets.GetsDestroyedForMeteors[Type] = true;
        TileID.Sets.GetsCheckedForLeaves[Type] = true;
        TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
        TileID.Sets.PreventsTileReplaceIfOnTopOfIt[Type] = true;
        LocalizedText name = CreateMapEntryName();
        AddMapEntry(new(151, 107, 75), name);
        //DustType = ModContent.DustType<StarvewoodDust>();
    }

    public static void KillTile_GetTreeDrops(int i, int j, Tile tileCache, ref bool bonusWood, ref int dropItem, ref int secondaryItem) {
        if (tileCache.TileFrameX >= 22 && tileCache.TileFrameY >= 198) {
            if (Main.netMode != NetmodeID.MultiplayerClient) {
                if (genRand.NextBool(2)) {
                    for (int k = j; !Main.tile[i, k].HasTile || !Main.tileSolid[Main.tile[i, k].TileType] || Main.tileSolidTop[Main.tile[i, k].TileType]; k++);
                    dropItem = 9;
                    secondaryItem = 27;
                } else {
                    dropItem = 9;
                }
            }
        } else {
            dropItem = 9;
        }
        if (dropItem != 9)
            return;
        GetTreeBottom(i, j, out var x, out var y);
        if (Main.tile[x, y].HasTile)
            dropItem = ModContent.ItemType<Items.Starved.Placeable.Starvewood>();
        int num = Player.FindClosest(new(x * 16, y * 16), 16, 16);
        int axe = Main.player[num].inventory[Main.player[num].selectedItem].axe;
        if (genRand.Next(100) < axe || Main.rand.NextBool(3))
            bonusWood = true;
    }

    public static bool GrowModdedTreeWithSettings(int checkedX, int checkedY, GrowTreeSettings settings) {
        int i;
        for (i = checkedY; Main.tile[checkedX, i].TileType == settings.SaplingTileType; i++);
        if (Main.tile[checkedX - 1, i - 1].LiquidAmount != 0 || Main.tile[checkedX, i - 1].LiquidAmount != 0 || Main.tile[checkedX + 1, i - 1].LiquidAmount != 0)
            return false;
        Tile tile = Main.tile[checkedX, i];
        if (!tile.HasUnactuatedTile || tile.IsHalfBlock || tile.Slope != 0) {
            return false;
        }
        bool flag = settings.WallTest(Main.tile[checkedX, i - 1].WallType);
        if (!settings.GroundTest(tile.TileType) || !flag)
            return false;
        if ((!Main.tile[checkedX - 1, i].HasTile || !settings.GroundTest(Main.tile[checkedX - 1, i].TileType)) && (!Main.tile[checkedX + 1, i].HasTile || !settings.GroundTest(Main.tile[checkedX + 1, i].TileType)))
            return false;
        TileColorCache cache = Main.tile[checkedX, i].BlockColorAndCoating();
        if (Main.tenthAnniversaryWorld && !gen && (settings.TreeTileType == 596 || settings.TreeTileType == 616))
            cache.Color = (byte)genRand.Next(1, 13);
        int num = 2;
        int num2 = genRand.Next(settings.TreeHeightMin, settings.TreeHeightMax + 1);
        int num3 = num2 + settings.TreeTopPaddingNeeded;
        if (!EmptyTileCheck(checkedX - num, checkedX + num, i - num3, i - 1, 20))
            return false;
        bool flag2 = false;
        bool flag3 = false;
        int num4;
        for (int j = i - num2; j < i; j++) {
            Tile tile2 = Main.tile[checkedX, j];
            tile2.TileFrameNumber = (byte)genRand.Next(3);
            tile2.HasTile = true;
            tile2.TileType = settings.TreeTileType;
            tile2.UseBlockColors(cache);
            num4 = genRand.Next(3);
            int num5 = genRand.Next(10);
            if (j == i - 1 || j == i - num2)
                num5 = 0;
            while (((num5 == 5 || num5 == 7) && flag2) || ((num5 == 6 || num5 == 7) && flag3))
                num5 = genRand.Next(10);
            flag2 = num5 == 5 || num5 == 7;
            flag3 = num5 == 6 || num5 == 7;
            switch (num5) {
                case 1:
                    if (num4 == 0) {
                        tile2.TileFrameX = 0;
                        tile2.TileFrameY = 66;
                    } else if (num4 == 1) {
                        tile2.TileFrameX = 0;
                        tile2.TileFrameY = 88;
                    } else if (num4 == 2) {
                        tile2.TileFrameX = 0;
                        tile2.TileFrameY = 110;
                    }
                    break;
                case 2:
                    if (num4 == 0) {
                        tile2.TileFrameX = 22;
                        tile2.TileFrameY = 0;
                    } else if (num4 == 1) {
                        tile2.TileFrameX = 22;
                        tile2.TileFrameY = 22;
                    } else if (num4 == 2) {
                        tile2.TileFrameX = 22;
                        tile2.TileFrameY = 44;
                    }
                    break;
                case 3:
                    if (num4 == 0) {
                        tile2.TileFrameX = 44;
                        tile2.TileFrameY = 66;
                    } else if (num4 == 1) {
                        tile2.TileFrameX = 44;
                        tile2.TileFrameY = 88;
                    } else if (num4 == 2) {
                        tile2.TileFrameX = 44;
                        tile2.TileFrameY = 110;
                    }
                    break;
                case 4:
                    if (num4 == 0) {
                        tile2.TileFrameX = 22;
                        tile2.TileFrameY = 66;
                    } else if (num4 == 1) {
                        tile2.TileFrameX = 22;
                        tile2.TileFrameY = 88;
                    } else if (num4 == 2) {
                        tile2.TileFrameX = 22;
                        tile2.TileFrameY = 110;
                    }
                    break;
                case 5:
                    if (num4 == 0) {
                        tile2.TileFrameX = 88;
                        tile2.TileFrameY = 0;
                    } else if (num4 == 1) {
                        tile2.TileFrameX = 88;
                        tile2.TileFrameY = 22;
                    } else if (num4 == 2) {
                        tile2.TileFrameX = 88;
                        tile2.TileFrameY = 44;
                    }
                    break;
                case 6:
                    if (num4 == 0) {
                        tile2.TileFrameX = 66;
                        tile2.TileFrameY = 66;
                    } else if (num4 == 1) {
                        tile2.TileFrameX = 66;
                        tile2.TileFrameY = 88;
                    } else if (num4 == 2) {
                        tile2.TileFrameX = 66;
                        tile2.TileFrameY = 110;
                    }
                    break;
                case 7:
                    if (num4 == 0) {
                        tile2.TileFrameX = 110;
                        tile2.TileFrameY = 66;
                    } else if (num4 == 1) {
                        tile2.TileFrameX = 110;
                        tile2.TileFrameY = 88;
                    } else if (num4 == 2) {
                        tile2.TileFrameX = 110;
                        tile2.TileFrameY = 110;
                    }
                    break;
                default:
                    if (num4 == 0) {
                        tile2.TileFrameX = 0;
                        tile2.TileFrameY = 0;
                    }
                    if (num4 == 1) {
                        tile2.TileFrameX = 0;
                        tile2.TileFrameY = 22;
                    }
                    if (num4 == 2) {
                        tile2.TileFrameX = 0;
                        tile2.TileFrameY = 44;
                    }
                    break;
            }
            if (num5 == 5 || num5 == 7) {
                Tile tile3 = Main.tile[checkedX - 1, j];
                tile3.HasTile = true;
                tile3.TileType = settings.TreeTileType;
                tile3.UseBlockColors(cache);
                num4 = genRand.Next(3);
                if (genRand.Next(3) < 2) {
                    if (num4 == 0) {
                        tile3.TileFrameX = 44;
                        tile3.TileFrameY = 198;
                    } else if (num4 == 1) {
                        tile3.TileFrameX = 44;
                        tile3.TileFrameY = 220;
                    } else if (num4 == 2) {
                        tile3.TileFrameX = 44;
                        tile3.TileFrameY = 242;
                    }
                } else {
                    if (num4 == 0) {
                        tile3.TileFrameX = 66;
                        tile3.TileFrameY = 0;
                    } else if (num4 == 1) {
                        tile3.TileFrameX = 66;
                        tile3.TileFrameY = 22;
                    } else if (num4 == 2) {
                        tile3.TileFrameX = 66;
                        tile3.TileFrameY = 44;
                    }
                }
            }
            if (num5 != 6 && num5 != 7)
                continue;
            Tile tile4 = Main.tile[checkedX + 1, j];
            tile4.HasTile = true;
            tile4.TileType = settings.TreeTileType;
            tile4.UseBlockColors(cache);
            num4 = genRand.Next(3);
            if (genRand.Next(3) < 2) {
                if (num4 == 0) {
                    tile4.TileFrameX = 66;
                    tile4.TileFrameY = 198;
                } else if (num4 == 1) {
                    tile4.TileFrameX = 66;
                    tile4.TileFrameY = 220;
                } else if (num4 == 2) {
                    tile4.TileFrameX = 66;
                    tile4.TileFrameY = 242;
                }
            } else {
                if (num4 == 0) {
                    tile4.TileFrameX = 88;
                    tile4.TileFrameY = 66;
                } else if (num4 == 1) {
                    tile4.TileFrameX = 88;
                    tile4.TileFrameY = 88;
                }else if (num4 == 2) {
                    tile4.TileFrameX = 88;
                    tile4.TileFrameY = 110;
                }
            }
        }
        bool flag4 = Main.tile[checkedX - 1, i].HasUnactuatedTile && !Main.tile[checkedX - 1, i].IsHalfBlock && Main.tile[checkedX - 1, i].Slope == 0 && IsTileTypeFitForTree(Main.tile[checkedX - 1, i].TileType) && !genRand.NextBool(3);
        bool flag5 = Main.tile[checkedX + 1, i].HasUnactuatedTile && !Main.tile[checkedX + 1, i].IsHalfBlock && Main.tile[checkedX + 1, i].Slope == 0 && IsTileTypeFitForTree(Main.tile[checkedX + 1, i].TileType) && !genRand.NextBool(3);
        if (flag5) {
            Tile HasTile1 = Main.tile[checkedX + 1, i - 1];
            HasTile1.HasTile = true;
            Main.tile[checkedX + 1, i - 1].TileType = settings.TreeTileType;
            Main.tile[checkedX + 1, i - 1].UseBlockColors(cache);
            num4 = genRand.Next(3);
            if (num4 == 0) {
                Main.tile[checkedX + 1, i - 1].TileFrameX = 22;
                Main.tile[checkedX + 1, i - 1].TileFrameY = 132;
            } else if (num4 == 1) {
                Main.tile[checkedX + 1, i - 1].TileFrameX = 22;
                Main.tile[checkedX + 1, i - 1].TileFrameY = 154;
            } else if (num4 == 2) {
                Main.tile[checkedX + 1, i - 1].TileFrameX = 22;
                Main.tile[checkedX + 1, i - 1].TileFrameY = 176;
            }
        }
        if (flag4) {
            Tile HasTile2 = Main.tile[checkedX - 1, i - 1];
            HasTile2.HasTile = true;
            Main.tile[checkedX - 1, i - 1].TileType = settings.TreeTileType;
            Main.tile[checkedX - 1, i - 1].UseBlockColors(cache);
            num4 = genRand.Next(3);
            if (num4 == 0) {
                Main.tile[checkedX - 1, i - 1].TileFrameX = 44;
                Main.tile[checkedX - 1, i - 1].TileFrameY = 132;
            } else if (num4 == 1) {
                Main.tile[checkedX - 1, i - 1].TileFrameX = 44;
                Main.tile[checkedX - 1, i - 1].TileFrameY = 154;
            } else if (num4 == 2) {
                Main.tile[checkedX - 1, i - 1].TileFrameX = 44;
                Main.tile[checkedX - 1, i - 1].TileFrameY = 176;
            }
        }
        num4 = genRand.Next(3);
        if (flag4 && flag5) {
            if (num4 == 0) {
                Main.tile[checkedX, i - 1].TileFrameX = 88;
                Main.tile[checkedX, i - 1].TileFrameY = 132;
            } else if (num4 == 1) {
                Main.tile[checkedX, i - 1].TileFrameX = 88;
                Main.tile[checkedX, i - 1].TileFrameY = 154;
            } else if (num4 == 2) {
                Main.tile[checkedX, i - 1].TileFrameX = 88;
                Main.tile[checkedX, i - 1].TileFrameY = 176;
            }
        } else if (flag4) {
            if (num4 == 0) {
                Main.tile[checkedX, i - 1].TileFrameX = 0;
                Main.tile[checkedX, i - 1].TileFrameY = 132;
            } else if (num4 == 1) {
                Main.tile[checkedX, i - 1].TileFrameX = 0;
                Main.tile[checkedX, i - 1].TileFrameY = 154;
            } else if (num4 == 2) {
                Main.tile[checkedX, i - 1].TileFrameX = 0;
                Main.tile[checkedX, i - 1].TileFrameY = 176;
            }
        } else if (flag5) {
            if (num4 == 0) {
                Main.tile[checkedX, i - 1].TileFrameX = 66;
                Main.tile[checkedX, i - 1].TileFrameY = 132;
            } else if (num4 == 1) {
                Main.tile[checkedX, i - 1].TileFrameX = 66;
                Main.tile[checkedX, i - 1].TileFrameY = 154;
            } else if (num4 == 2) {
                Main.tile[checkedX, i - 1].TileFrameX = 66;
                Main.tile[checkedX, i - 1].TileFrameY = 176;
            }
        }
        if (genRand.NextBool(13)) {
            num4 = genRand.Next(3);
            if (num4 == 0) {
                Main.tile[checkedX, i - num2].TileFrameX = 22;
                Main.tile[checkedX, i - num2].TileFrameY = 198;
            } else if (num4 == 1) {
                Main.tile[checkedX, i - num2].TileFrameX = 22;
                Main.tile[checkedX, i - num2].TileFrameY = 220;
            } else if (num4 == 2) {
                Main.tile[checkedX, i - num2].TileFrameX = 22;
                Main.tile[checkedX, i - num2].TileFrameY = 242;
            }
        } else {
            num4 = genRand.Next(3);
            if (num4 == 0) {
                Main.tile[checkedX, i - num2].TileFrameX = 0;
                Main.tile[checkedX, i - num2].TileFrameY = 198;
            } else if (num4 == 1) {
                Main.tile[checkedX, i - num2].TileFrameX = 0;
                Main.tile[checkedX, i - num2].TileFrameY = 220;
            } else if (num4 == 2) {
                Main.tile[checkedX, i - num2].TileFrameX = 0;
                Main.tile[checkedX, i - num2].TileFrameY = 242;
            }
        }
        RangeFrame(checkedX - 2, i - num2 - 1, checkedX + 2, i + 1);
        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendTileSquare(-1, checkedX - 1, i - num2, 3, num2);
        return true;
    }

    public Texture2D GetTreeTopTexture(int tileType, int treeTextureStyle, byte tileColor) {
        Texture2D texture2D = TryGetTreeTopAndRequestIfNotReady(GreatlySidegradedWorld.StarvingTree, treeTextureStyle, tileColor);
        texture2D ??= (Texture2D)ModContent.Request<Texture2D>(Texture + "_Tops"); // unsure if we need to add the variant code here or not
        return texture2D;
    }

    public Texture2D GetTreeBranchTexture(int tileType, int treeTextureStyle, byte tileColor) {
        Texture2D texture2D = TryGetTreeBranchAndRequestIfNotReady(GreatlySidegradedWorld.StarvingTree, treeTextureStyle, tileColor);
        texture2D ??= (Texture2D)ModContent.Request<Texture2D>(Texture + "_Branches");
        return texture2D;
    }

    public class TreeTopRenderTargetHolder : ARenderTargetHolder {
        public TreeFoliageVariantKey Key;

        public override void Prepare() {
            Asset<Texture2D> asset = Key.TextureIndex switch {
                0 => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree_Tops"),
                1 => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree_Tops"),
                2 => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree_Tops"),
                _ => TextureAssets.TreeTop[0],
            };
            asset.Wait?.Invoke();
            PrepareTextureIfNecessary(asset.Value);
        }

        public override void PrepareShader() => PrepareShader(Key.PaintColor, TreeStarved);
    }

    public class TreeBranchTargetHolder : ARenderTargetHolder {
        public TreeFoliageVariantKey Key;

        public override void Prepare() {
            Asset<Texture2D> asset = Key.TextureIndex switch {
                0 => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree_Branches"),
                1 => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree_Branches"),
                2 => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree_Branches"),
                _ => TextureAssets.TreeBranch[0]
            };
            asset.Wait?.Invoke();
            PrepareTextureIfNecessary(asset.Value);
        }

        public override void PrepareShader() => PrepareShader(Key.PaintColor, TreeStarved);
    }

    private readonly Dictionary<TreeFoliageVariantKey, TreeBranchTargetHolder> _treeBranchRenders = [];
    private readonly Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder> _treeTopRenders = [];

    public void RequestTreeTop(ref TreeFoliageVariantKey lookupKey) {
        List<ARenderTargetHolder> _requests = (List<ARenderTargetHolder>)typeof(TilePaintSystemV2).GetField("_requests", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilePaintSystem);
        if (!_treeTopRenders.TryGetValue(lookupKey, out var value)) {
            value = new TreeTopRenderTargetHolder {
                Key = lookupKey
            };
            _treeTopRenders.Add(lookupKey, value);
        }
        if (!value.IsReady)
            _requests.Add(value);
    }

    public void RequestTreeBranch(ref TreeFoliageVariantKey lookupKey) {
        List<ARenderTargetHolder> _requests = (List<ARenderTargetHolder>)typeof(TilePaintSystemV2).GetField("_requests", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilePaintSystem);
        if (!_treeBranchRenders.TryGetValue(lookupKey, out var value)) {
            value = new TreeBranchTargetHolder {
                Key = lookupKey
            };
            _treeBranchRenders.Add(lookupKey, value);
        }
        if (!value.IsReady)
            _requests.Add(value);
    }

    public Texture2D TryGetTreeTopAndRequestIfNotReady(int confectionTreeVariation, int treeTopStyle, int paintColor) {
        TreeFoliageVariantKey treeFoliageVariantKey = default;
        treeFoliageVariantKey.TextureStyle = treeTopStyle;
        treeFoliageVariantKey.PaintColor = paintColor;
        treeFoliageVariantKey.TextureIndex = confectionTreeVariation;
        TreeFoliageVariantKey lookupKey = treeFoliageVariantKey;
        if (_treeTopRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
            return value.Target;
        RequestTreeTop(ref lookupKey);
        return null;
    }

    public Texture2D TryGetTreeBranchAndRequestIfNotReady(int confectionTreeVariation, int treeTopStyle, int paintColor) {
        TreeFoliageVariantKey treeFoliageVariantKey = default;
        treeFoliageVariantKey.TextureStyle = treeTopStyle;
        treeFoliageVariantKey.PaintColor = paintColor;
        treeFoliageVariantKey.TextureIndex = confectionTreeVariation;
        TreeFoliageVariantKey lookupKey = treeFoliageVariantKey;
        if (_treeBranchRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
            return value.Target;
        RequestTreeBranch(ref lookupKey);
        return null;
    }

    public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
        width = 20;
        height = 20;
        if (GreatlySidegradedWorld.StarvingTree > 0 && tileFrameX < 176)
            tileFrameX = (short)(tileFrameX + 176);
        else if (tileFrameX >= 176)
            tileFrameX = (short)(tileFrameX - 176);
    }

    public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) {
        Tile tile = Main.tile[i, j];
        if (i > 5 && j > 5 && i < Main.maxTilesX - 5 && j < Main.maxTilesY - 5)
            if (tile.HasTile)
                if (Main.tileFrameImportant[Type])
                    CheckTree(i, j);
        return false;
    }

    public override IEnumerable<Item> GetItemDrops(int i, int j) {
        int dropItem = ItemID.None;
        int dropItemStack = 1;
        int secondaryItem = ItemID.None;
        Tile tileCache = Main.tile[i, j];
        bool bonusWood = false;
        KillTile_GetTreeDrops(i, j, tileCache, ref bonusWood, ref dropItem, ref secondaryItem);
        if (bonusWood)
            dropItemStack++;
        yield return new Item(dropItem, dropItemStack);
        yield return new Item(secondaryItem);
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
        Tile tile = Main.tile[i, j];
        if (fail && Main.netMode != NetmodeID.MultiplayerClient && TileID.Sets.IsShakeable[tile.TileType])
            ShakeTree(i, j);
    }

    public override void NearbyEffects(int i, int j, bool closer) {
        GetTreeBottom(i, j, out var x, out var y);
        Tile tilebelow = Main.tile[x, y + 1];
        Tile tilecurrent = Main.tile[x, y];
        if (tilebelow.TileType != ModContent.TileType<StarvedGrass>() && tilecurrent.TileType != ModContent.TileType<StarvedGrass>())
            Main.tile[i, j].TileType = TileID.Trees;
    }

    public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
        spriteBatch.End();
        spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);
        DrawTrees(i, j, spriteBatch);
        spriteBatch.End();
        spriteBatch.Begin();
    }

    private static void ShakeTree(int i, int j) {
        FieldInfo numTreeShakesReflect = typeof(WorldGen).GetField("numTreeShakes", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
        int numTreeShakes = (int)numTreeShakesReflect.GetValue(null);
        int maxTreeShakes = (int)typeof(WorldGen).GetField("maxTreeShakes", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
        int[] treeShakeX = (int[])typeof(WorldGen).GetField("treeShakeX", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
        int[] treeShakeY = (int[])typeof(WorldGen).GetField("treeShakeY", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(null);
        if (numTreeShakes == maxTreeShakes)
            return;
        GetTreeBottom(i, j, out var x, out var y);
        for (int k = 0; k < numTreeShakes; k++)
            if (treeShakeX[k] == x && treeShakeY[k] == y)
                return;
        treeShakeX[numTreeShakes] = x;
        treeShakeY[numTreeShakes] = y;
        numTreeShakesReflect.SetValue(null, ++numTreeShakes);
        y--;
        while (y > 10 && Main.tile[x, y].HasTile && TileID.Sets.IsShakeable[Main.tile[x, y].TileType])
            y--;
        y++;
        if (!IsTileALeafyTreeTop(x, y) || Collision.SolidTiles(x - 2, x + 2, y - 2, y + 2)) 
            return;
        if (Main.getGoodWorld && genRand.NextBool(17)) {
            Projectile.NewProjectile(new EntitySource_ShakeTree(x, y), x * 16, y * 16, Main.rand.Next(-100, 101) * 0.002f, 0f, ProjectileID.Bomb, 0, 0f, Main.myPlayer, 16f, 16f);
        } else if (genRand.NextBool(7)) {
            Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.Acorn, genRand.Next(1, 3));
        } else if (genRand.NextBool(35) && Main.halloween) {
            Item.NewItem(new EntitySource_ShakeTree(x, y), x * 16, y * 16, 16, 16, ItemID.RottenEgg, genRand.Next(1, 3));
        } else if (genRand.NextBool(12)) {
            Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, ModContent.ItemType<Items.Starved.Placeable.Starvewood>(), genRand.Next(1, 4));
        } else if (genRand.NextBool(20)) {
            int type = ItemID.CopperCoin;
            int num2 = genRand.Next(50, 100);
            if (genRand.NextBool(30)) {
                type = ItemID.GoldCoin;
                num2 = 1;
                if (genRand.NextBool(5))
                    num2++;
                if (genRand.NextBool(10))
                    num2++;
            } else if (genRand.NextBool(10)) {
                type = ItemID.SilverCoin;
                num2 = genRand.Next(1, 21);
                if (genRand.NextBool(3))
                    num2 += genRand.Next(1, 21);
                if (genRand.NextBool(4))
                    num2 += genRand.Next(1, 21);
            }
            Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, type, num2);
        } else if (genRand.NextBool(12)) {
            int secondaryItemStack = ItemID.Starfruit;
            Item.NewItem(GetItemSource_FromTreeShake(x, y), x * 16, y * 16, 16, 16, secondaryItemStack);
        }
        if (Main.netMode == NetmodeID.Server)
            NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, 1f, ModContent.GoreType<StarvingTreeLeaf>());
        else if (Main.netMode == NetmodeID.SinglePlayer)
            TreeGrowFX(x, y, 1, ModContent.GoreType<StarvingTreeLeaf>(), hitTree: true);
    }

    private static void EmitStarvingLeaves(int tilePosX, int tilePosY, int grassPosX, int grassPosY) {
        bool _isActiveAndNotPaused = (bool)typeof(TileDrawing).GetField("_isActiveAndNotPaused", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
        int _leafFrequency = (int)typeof(TileDrawing).GetField("_leafFrequency", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
        UnifiedRandom _rand = (UnifiedRandom)typeof(TileDrawing).GetField("_rand", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
        if (!_isActiveAndNotPaused)
            return;
        Tile tile = Main.tile[tilePosX, tilePosY];
        if (tile.LiquidAmount > 0)
            return;
        int num = 0;
        bool flag = (byte)num != 0;
        int num2 = _leafFrequency;
        bool flag2 = tilePosX - grassPosX != 0;
        if (flag)
            num2 /= 2;
        if (!DoesWindBlowAtThisHeight(tilePosY))
            num2 = 10000;
        if (flag2)
            num2 *= 3;
        if (!_rand.NextBool(num2))
            return;
        int num3 = 2;
        Vector2 vector = new(tilePosX * 16 + 8, tilePosY * 16 + 8);
        if (flag2) {
            int num4 = tilePosX - grassPosX;
            vector.X += num4 * 12;
            int num5 = 0;
            if (tile.TileFrameY == 220)
                num5 = 1;
            else if (tile.TileFrameY == 242)
                num5 = 2;
            if (tile.TileFrameX == 66) {
                vector += num5 switch {
                    0 => new Vector2(0, -6),
                    1 => new Vector2(0, -6),
                    2 => new Vector2(0, 8),
                    _ => Vector2.Zero
                };
            } else {
                vector += num5 switch {
                    0 => new Vector2(0, 4),
                    1 => new Vector2(2, -6),
                    2 => new Vector2(6, -6),
                    _ => Vector2.Zero
                };
            }
        } else {
            vector += new Vector2(-16f, -16f);
            if (flag)
                vector.Y -= Main.rand.Next(0, 28) * 4;
        }
        if (!SolidTile(vector.ToTileCoordinates()))
            Gore.NewGoreDirect(new EntitySource_Misc(""), vector, Utils.RandomVector2(Main.rand, -num3, num3), ModContent.GoreType<StarvingTreeLeaf>(), 0.7f + Main.rand.NextFloat() * 0.6f).Frame.CurrentColumn = Main.tile[tilePosX, tilePosY].TileColor;
    }

    public static bool StarvewoodTreeGroundTest(int tileType) => tileType == ModContent.TileType<StarvedGrass>();

    public static bool GetStarvewoodTreeFoliageData(int i, int j, int xoffset, ref int treeFrame, out int floorY, out int topTextureFrameWidth, out int topTextureFrameHeight) {
        int num = i + xoffset;
        StarvingTreeTextureFrame(i, ref treeFrame, out topTextureFrameWidth, out topTextureFrameHeight);
        floorY = j;
        for (int k = 0; k < 100; k++) {
            floorY = j + k;
            Tile tile2 = Main.tile[num, floorY];
            if (tile2 == null)
                return false;
        }
        return true;
    }

    private static void StarvingTreeTextureFrame(int i, ref int treeFrame, out int topTextureFrameWidth, out int topTextureFrameHeight) {
        int variant = GreatlySidegradedWorld.StarvingTree;
        if (variant == 0) {
            topTextureFrameWidth = 80;
            topTextureFrameHeight = 80;
        } else if (variant == 1) {
            topTextureFrameWidth = 102;
            topTextureFrameHeight = 118;
            if (i % 3 == 1)
                treeFrame += 3;
            else if (i % 3 == 2)
                treeFrame += 6;
        } else {
            topTextureFrameWidth = 100;
            topTextureFrameHeight = 110;
        }
    }

    private void DrawTrees(int k, int l, SpriteBatch spriteBatch) {
        double _treeWindCounter = (double)typeof(TileDrawing).GetField("_treeWindCounter", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance).GetValue(Main.instance.TilesRenderer);
        float num15 = 0.08f;
        float num16 = 0.06f;
        int PositioningFix = CaptureManager.Instance.IsCapturing ? 0 : 192; // Fix to the positioning to the Branches and Tops being 192 pixels to the top and left
        int x = k;
        int y = l;
        Tile tile = Main.tile[x, y];
        if (!tile.HasTile)
            return;
        short frameX = tile.TileFrameX;
        short frameY = tile.TileFrameY;
        bool flag = tile.WallType > 0;
        if (frameY >= 198 && frameX >= 22) {
            int treeFrame = GetTreeFrame(tile);
            switch (frameX) {
                case 22:
                    int num5 = 0;
                    int grassPosX = x + num5;
                    if (!GetStarvewoodTreeFoliageData(x, y, num5, ref treeFrame, out int floorY3, out int topTextureFrameWidth3, out int topTextureFrameHeight3))
                        return;
                    EmitStarvingLeaves(x, y, grassPosX, floorY3);
                    byte tileColor3 = tile.TileColor;
                    Texture2D treeTopTexture = GetTreeTopTexture(Type, 0, tileColor3);
                    Vector2 vector = new(x * 16 - (int)Main.Camera.UnscaledPosition.X + 8 + PositioningFix, y * 16 - (int)Main.Camera.UnscaledPosition.Y + 16 + PositioningFix);
                    float num7 = 0f;
                    if (!flag)
                        num7 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
                    vector.X += num7 * 2f;
                    vector.Y += Math.Abs(num7) * 2f;
                    Color color6 = Lighting.GetColor(x, y);
                    if (tile.IsTileFullbright)
                        color6 = Color.White;
                    spriteBatch.Draw(treeTopTexture, vector, new(treeFrame * (topTextureFrameWidth3 + 2), 0, topTextureFrameWidth3, topTextureFrameHeight3), color6, num7 * num15, new(topTextureFrameWidth3 / 2, topTextureFrameHeight3), 1f, 0, 0f);
                    break;
                case 44:
                    int num21 = x;
                    int num2 = 1;
                    if (!GetStarvewoodTreeFoliageData(x, y, num2, ref treeFrame, out int floorY2, out _, out _))
                        return;
                    EmitStarvingLeaves(x, y, num21 + num2, floorY2);
                    byte tileColor2 = tile.TileColor;
                    Texture2D treeBranchTexture2 = GetTreeBranchTexture(Type, 0, tileColor2);
                    Vector2 position2 = new Vector2((x * 16) + PositioningFix, (y * 16) + PositioningFix) - Main.Camera.UnscaledPosition.Floor() + new Vector2(16f, 12f);
                    float num4 = 0f;
                    if (!flag)
                        num4 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
                    if (num4 > 0f)
                        position2.X += num4;
                    position2.X += Math.Abs(num4) * 2f;
                    Color color4 = Lighting.GetColor(x, y);
                    if (tile.IsTileFullbright)
                        color4 = Color.White;
                    spriteBatch.Draw(treeBranchTexture2, position2, new(0, treeFrame * 42, 40, 40), color4, num4 * num16, new(40f, 24f), 1f, 0, 0f);
                    break;
                case 66:
                    int num17 = x;
                    int num18 = -1;
                    if (!GetStarvewoodTreeFoliageData(x, y, num18, ref treeFrame, out int floorY, out _, out _))
                        return;
                    EmitStarvingLeaves(x, y, num17 + num18, floorY);
                    byte tileColor = tile.TileColor;
                    Texture2D treeBranchTexture = GetTreeBranchTexture(Type, 0, tileColor);
                    Vector2 position = new Vector2((x * 16) + PositioningFix, (y * 16) + PositioningFix) - Main.Camera.UnscaledPosition.Floor() + new Vector2(0f, 18f);
                    float num20 = 0f;
                    if (!flag)
                        num20 = Main.instance.TilesRenderer.GetWindCycle(x, y, _treeWindCounter);
                    if (num20 < 0f)
                        position.X += num20;
                    position.X -= Math.Abs(num20) * 2f;
                    Color color2 = Lighting.GetColor(x, y);
                    if (tile.IsTileFullbright)
                        color2 = Color.White;
                    spriteBatch.Draw(treeBranchTexture, position, new(42, treeFrame * 42, 40, 40), color2, num20 * num16, new(0f, 30f), 1f, 0, 0f);
                    break;
            }
        }
    }
}
