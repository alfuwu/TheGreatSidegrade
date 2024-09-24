using Terraria.ModLoader;
using TheGreatSidegrade.Content.Tiles.Starved;
using TheGreatSidegrade.Common.Abstract;
using Terraria.ID;

namespace TheGreatSidegrade.Content.Items.Starved.Placeable;

public class Starvewood : BlockItem {
    public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
        itemGroup = ContentSamples.CreativeHelper.ItemGroup.Wood;
    }

    public override int TileID => ModContent.TileType<StarvingStone>();
}