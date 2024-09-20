using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheGreatSidegrade.Content.Items.Starved.Placeable;

public class StarvingCampfire : ModItem {
    public override void SetDefaults() {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Starved.StarvingCampfire>(), 0);
    }

    public override void AddRecipes() {
        CreateRecipe()
            .AddRecipeGroup(RecipeGroupID.Wood, 10)
            .AddIngredient<StarvingTorch>(5)
            .Register();
    }
}