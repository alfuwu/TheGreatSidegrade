using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TheGreatSidegrade.Content.Tiles.Rotten;

public class Rust : ModTile {
    public override void SetStaticDefaults() {
        TileID.Sets.Ore[Type] = true;
        TileID.Sets.OreMergesWithMud[Type] = true;
        Main.tileSpelunker[Type] = true;
        Main.tileOreFinderPriority[Type] = 330;
        Main.tileShine2[Type] = true;
        Main.tileShine[Type] = 975;
        Main.tileMergeDirt[Type] = true;
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;
        
        TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Demonite, 0));
        TileObjectData.addTile(Type);
    }
}
