using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using TheGreatSidegrade.Common;
using Terraria.ID;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvingStone : ModTile {
    public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
        sightColor = GreatlySidegradedWorld.StarvedBiomeColor;
        return true;
    }

    public override void SetStaticDefaults() {
        Main.tileSolid[Type] = true;
        Main.tileBrick[Type] = true;
        Main.tileMergeDirt[Type] = true;
        Main.tileBlockLight[Type] = true;
        TileID.Sets.Conversion.Stone[Type] = true;
        TileID.Sets.GeneralPlacementTiles[Type] = false;
        TileID.Sets.Stone[Type] = true;
        TileID.Sets.CanBeClearedDuringOreRunner[Type] = true;
        HitSound = SoundID.Tink;
        MinPick = 60;
        GreatlySidegradedIDs.Sets.StarvedTileCollection.Add(Type);

        AddMapEntry(new Color(164, 104, 91));
    }

    //public override void NumDust(int i, int j, bool fail, ref int num) {
    //    num = fail ? 1 : 3;
    //}

    //public override void ChangeWaterfallStyle(ref int style) {
    //    style = ModContent.GetInstance<ExampleWaterfallStyle>().Slot;
    //}
}