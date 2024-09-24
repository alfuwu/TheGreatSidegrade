using Terraria.ModLoader;
using TheGreatSidegrade.Content.Tiles.Starved;
using TheGreatSidegrade.Common.Abstract;

namespace TheGreatSidegrade.Content.Items.Starved.Placeable;

public class StarvedSandBlock : BlockItem {
    public override int TileID => ModContent.TileType<StarvedSand>();
}