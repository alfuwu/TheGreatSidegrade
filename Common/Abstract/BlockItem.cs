using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheGreatSidegrade.Common.Abstract;

public abstract class BlockItem : ModItem {
    public abstract int TileID { get; }

    public override void SetStaticDefaults() {
        Item.ResearchUnlockCount = 100;
    }

    public override void SetDefaults() {
        Rectangle dims = this.GetDims();
        Item.autoReuse = true;
        Item.consumable = true;
        Item.createTile = TileID;
        Item.rare = ItemRarityID.White;
        Item.width = dims.Width;
        Item.height = dims.Height;
        Item.useTime = 10;
        Item.useTurn = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.maxStack = 9999;
        Item.value = 0;
        Item.useAnimation = 15;
    }
}
