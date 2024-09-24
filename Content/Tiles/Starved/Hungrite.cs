using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheGreatSidegrade.Content.Items.Starved.Placeable;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class Hungrite : ModTile {
    public override void SetStaticDefaults() {
        TileID.Sets.Ore[Type] = true;
        TileID.Sets.OreMergesWithMud[Type] = true;
        Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
        Main.tileOreFinderPriority[Type] = 320; // Metal Detector value, see https://terraria.wiki.gg/wiki/Metal_Detector
        Main.tileShine2[Type] = true; // Modifies the draw color slightly.
        Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
        Main.tileMergeDirt[Type] = true;
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;
        
        TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Demonite, 0));

        RegisterItemDrop(ModContent.ItemType<HungriteOre>());
    }
}
