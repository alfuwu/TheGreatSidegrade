using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using TheGreatSidegrade.Common;
using TheGreatSidegrade.Content.Items.Starved.Placeable;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvedIce : ModTile {
    public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
        sightColor = GreatlySidegradedWorld.StarvedBiomeColor;
        return true;
    }

    public override void SetStaticDefaults() {
        Main.tileSolid[Type] = true;
        TileID.Sets.Conversion.Ice[Type] = true;
        //Main.tileMergeDirt[Type] = true;
        Main.tileBlockLight[Type] = true;
        GreatlySidegradedIDs.Sets.StarvedTileCollection.Add(Type);

        AddMapEntry(new Color(221, 131, 59));

        RegisterItemDrop(ModContent.ItemType<StarvedIceBlock>());
    }

    //public override void NumDust(int i, int j, bool fail, ref int num) {
    //    num = fail ? 1 : 3;
    //}

    //public override void ChangeWaterfallStyle(ref int style) {
    //    style = ModContent.GetInstance<ExampleWaterfallStyle>().Slot;
    //}
}