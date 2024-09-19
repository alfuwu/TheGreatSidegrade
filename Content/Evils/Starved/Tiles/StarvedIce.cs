﻿using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace TheGreatSidegrade.Content.Evils.Starved.Tiles {
    public class StarvedIce : ModTile {
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            TileID.Sets.Conversion.Ice[Type] = true;
            //Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;

            AddMapEntry(new Color(221, 131, 59));
        }

        //public override void NumDust(int i, int j, bool fail, ref int num) {
        //    num = fail ? 1 : 3;
        //}

        //public override void ChangeWaterfallStyle(ref int style) {
        //    style = ModContent.GetInstance<ExampleWaterfallStyle>().Slot;
        //}
    }
}
