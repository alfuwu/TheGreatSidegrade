using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using TheGreatSidegrade.Common;
using Terraria.ID;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvedGrass : ModTile {
    public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
        sightColor = GreatlySidegradedWorld.StarvedBiomeColor;
        return true;
    }

    public override void SetStaticDefaults() {
        Main.tileSolid[Type] = true;
        Main.tileBrick[Type] = true;
        Main.tileBlockLight[Type] = true;
        TileID.Sets.Conversion.Grass[Type] = true;
        TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
        TileID.Sets.SpreadOverground[Type] = true;
        TileID.Sets.SpreadUnderground[Type] = true;
        TileID.Sets.CanBeDugByShovel[Type] = true;
        TileID.Sets.NeedsGrassFraming[Type] = true;
        TileID.Sets.NeedsGrassFramingDirt[Type] = TileID.Dirt;
        TileID.Sets.Grass[Type] = true;
        GreatlySidegradedIDs.Sets.StarvedTileCollection.Add(Type);

        RegisterItemDrop(ItemID.DirtBlock);

        AddMapEntry(new Color(221, 131, 59));
    }

    public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
        if (fail && !effectOnly) {
            //if (Main.tile[i, j - 1].TileType == ModContent.TileType<ContagionShortGrass>())
            //    WorldGen.KillTile(i, j - 1);
            //if (Main.tile[i, j + 1].TileType == ModContent.TileType<ContagionVines>())
            //    WorldGen.KillTile(i, j + 1);
            noItem = true;
            Main.tile[i, j].TileType = TileID.Dirt;
            WorldGen.SquareTileFrame(i, j);
        }
    }

    //public override void NumDust(int i, int j, bool fail, ref int num) {
    //    num = fail ? 1 : 3;
    //}

    //public override void ChangeWaterfallStyle(ref int style) {
    //    style = ModContent.GetInstance<ExampleWaterfallStyle>().Slot;
    //}
}