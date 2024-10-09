using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using TheGreatSidegrade.Content.Gores;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvingSapling : ModTile {
    public override void SetStaticDefaults() {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.Width = 1;
        TileObjectData.newTile.Height = 2;
        TileObjectData.newTile.Origin = new(0, 1);
        TileObjectData.newTile.AnchorBottom = new(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.UsesCustomCanPlace = true;
        TileObjectData.newTile.CoordinateHeights = [16, 18];
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.AnchorValidTiles = [ModContent.TileType<StarvedGrass>()];
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.DrawFlipHorizontal = true;
        TileObjectData.newTile.WaterPlacement = LiquidPlacement.NotAllowed;
        TileObjectData.newTile.LavaDeath = true;
        TileObjectData.newTile.RandomStyleRange = 3;
        TileObjectData.newTile.StyleMultiplier = 3;

        TileObjectData.newSubTile.CopyFrom(TileObjectData.newTile);
        TileObjectData.newSubTile.AnchorValidTiles = [ModContent.TileType<StarvedSand>()];
        TileObjectData.addSubTile(1);

        TileObjectData.addTile(Type);

        LocalizedText name = CreateMapEntryName();
        AddMapEntry(new(151, 107, 75), name);

        TileID.Sets.TreeSapling[Type] = true;
        TileID.Sets.CommonSapling[Type] = true;
        TileID.Sets.SwaysInWindBasic[Type] = true;
        TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]);

        //DustType = ModContent.DustType<StarvewoodDust>();

        AdjTiles = [TileID.Saplings];
    }

    public override void NumDust(int i, int j, bool fail, ref int num) {
        num = fail ? 1 : 3;
    }

    public override void RandomUpdate(int i, int j) {
        if (!WorldGen.genRand.NextBool(20))
            return;

        Tile tile = Framing.GetTileSafely(i, j);
        bool growSucess;

        if (tile.TileFrameX < 54) {
            tile = Main.tile[i, j];
            if (tile.HasUnactuatedTile) {
                for (int k = 0; k < (Main.maxTilesX * Main.maxTilesY); k++) {
                    if (j > Main.rockLayer) {
                        if (WorldGen.genRand.NextBool(5))
                            AttemptToGrowStarvingTreeFromSapling(i, j);
                    } else {
                        if (WorldGen.genRand.NextBool(20))
                            AttemptToGrowStarvingTreeFromSapling(i, j);
                    }
                }
            }
            growSucess = false;
        } else {
            growSucess = WorldGen.GrowPalmTree(i, j);
        }

        bool isPlayerNear = WorldGen.PlayerLOS(i, j);

        if (growSucess && isPlayerNear)
            WorldGen.TreeGrowFXCheck(i, j);
    }

    public override void SetSpriteEffects(int i, int j, ref SpriteEffects effects) {
        if (i % 2 == 1)
            effects = SpriteEffects.FlipHorizontally;
    }

    public static bool AttemptToGrowStarvingTreeFromSapling(int x, int y) {
        if (Main.netMode == NetmodeID.MultiplayerClient)
            return false;
        if (!WorldGen.InWorld(x, y, 2))
            return false;
        Tile tile = Main.tile[x, y];
        if (!tile.HasTile)
            return false;
        bool flag = StarvingTree.GrowModdedTreeWithSettings(x, y, StarvingTree.Tree_Starved);
        if (flag && WorldGen.PlayerLOS(x, y))
            GrowStarvingTreeFXCheck(x, y);
        return flag;
    }

    public static void GrowStarvingTreeFXCheck(int x, int y) {
        int treeHeight = 1;
        for (int num = -1; num > -100; num--) {
            Tile tile = Main.tile[x, y + num];
            if (!tile.HasTile || !TileID.Sets.GetsCheckedForLeaves[tile.TileType])
                break;
            treeHeight++;
        }
        for (int i = 1; i < 5; i++) {
            Tile tile2 = Main.tile[x, y + i];
            if (tile2.HasTile && TileID.Sets.GetsCheckedForLeaves[tile2.TileType]) {
                treeHeight++;
                continue;
            }
            break;
        }
        if (treeHeight > 0) {
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.SpecialFX, -1, -1, null, 1, x, y, treeHeight, ModContent.GoreType<StarvingTreeLeaf>());
            else if (Main.netMode == NetmodeID.SinglePlayer)
                WorldGen.TreeGrowFX(x, y, treeHeight, ModContent.GoreType<StarvingTreeLeaf>());
        }
    }
}