using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheGreatSidegrade.Common;

namespace TheGreatSidegrade.Content.Tiles.Starved {
    public class StarvedJungleGrass : ModTile {
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
            TileID.Sets.NeedsGrassFramingDirt[Type] = TileID.Mud;
            TileID.Sets.Grass[Type] = true;

            RegisterItemDrop(ItemID.MudBlock);
        }
    }
}
