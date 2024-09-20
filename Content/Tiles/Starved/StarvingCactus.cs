using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using TheGreatSidegrade.Assets;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvingCactus : ModCactus {
    private Asset<Texture2D> texture;
    private Asset<Texture2D> fruitTexture;
    public override void SetStaticDefaults() {
        GrowsOnTileId = [ModContent.TileType<StarvedSand>()];
        texture = GameAssets.GetAsset("Content/Tiles/Starved/StarvingCactus");
        //fruitTexture = GameAssets.GetAsset("Content/Tiles/Starved/StarvingCactus_Fruit");
    }

    public override Asset<Texture2D> GetTexture() => texture;

    public override Asset<Texture2D> GetFruitTexture() => fruitTexture;
}