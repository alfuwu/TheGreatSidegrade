using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.Items.BiomeWorlds.Worlds;

namespace TheGreatSidegrade.Content.Items.BiomeWorlds;

[ExtendsFromMod("BiomeWorlds", "SubworldLibrary")]
public class FracturedMirror : ModItem {
    public override void SetDefaults() {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 1;
        Item.value = 100;
        Item.rare = ItemRarityID.Blue;
        Item.useAnimation = 30;
        Item.useTime = 30;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.consumable = false;
    }

    public override bool? UseItem(Player player) {
        return BiomeWorlds.EnterWorld<FracturedWorld>(player);
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 10)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
