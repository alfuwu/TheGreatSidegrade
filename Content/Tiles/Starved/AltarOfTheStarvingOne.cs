using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheGreatSidegrade.Content.Tiles.Starved {
    public class AltarOfTheStarvingOne : ModTile {
        public override void SetStaticDefaults() {
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.CoordinateHeights = [16, 18];
            TileObjectData.addTile(Type);
            TileID.Sets.PreventsTileRemovalIfOnTopOfIt[Type] = true;
            TileID.Sets.PreventsSandfall[Type] = true;
            TileID.Sets.InteractibleByNPCs[Type] = true;
            Main.tileHammer[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            AdjTiles = [TileID.DemonAltar];
            HitSound = SoundID.NPCHit1;

            AddMapEntry(new Color(200, 180, 30), this.GetLocalization("MapEntry"));
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            WorldGen.SmashAltar(i, j);
        }

        public override bool CanExplode(int i, int j) {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged) {
            if (!Main.hardMode)
                blockDamaged = false;

            return Main.hardMode;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b) {
            float f = Main.rand.Next(-5, 6) * 0.004f;
            r = 0.632f + (float)Math.Pow(f, 1.4);
            g = 0.444f + f * 1.2f;
            b = 0.131f;
        }
    }
}
