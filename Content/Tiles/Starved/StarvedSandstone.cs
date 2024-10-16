﻿using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using TheGreatSidegrade.Common;
using Terraria.ID;
using TheGreatSidegrade.Content.Items.Starved.Placeable;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvedSandstone : ModTile {
    public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
        sightColor = GreatlySidegradedWorld.StarvedBiomeColor;
        return true;
    }

    public override void SetStaticDefaults() {
        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
        Main.tileBlockLight[Type] = true;
        TileID.Sets.Conversion.Sandstone[Type] = true;
        GreatlySidegradedIDs.Sets.StarvedTileCollection.Add(Type);

        AddMapEntry(new Color(221, 131, 59));

        RegisterItemDrop(ModContent.ItemType<StarvedSandstoneBlock>());
    }

    //public override void NumDust(int i, int j, bool fail, ref int num) {
    //    num = fail ? 1 : 3;
    //}

    //public override void ChangeWaterfallStyle(ref int style) {
    //    style = ModContent.GetInstance<ExampleWaterfallStyle>().Slot;
    //}
}