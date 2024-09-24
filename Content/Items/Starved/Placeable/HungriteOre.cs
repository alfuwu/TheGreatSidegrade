using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheGreatSidegrade.Common.Abstract;
using TheGreatSidegrade.Content.Tiles.Starved;

namespace TheGreatSidegrade.Content.Items.Starved.Placeable;

public class HungriteOre : BlockItem {
    public override int TileID => ModContent.TileType<Hungrite>();

    public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
        itemGroup = ContentSamples.CreativeHelper.ItemGroup.Material;
    }

    public override void SetDefaults() {
        base.SetDefaults();
        Item.value = Item.sellPrice(0, 0, 7, 0);
    }
}
