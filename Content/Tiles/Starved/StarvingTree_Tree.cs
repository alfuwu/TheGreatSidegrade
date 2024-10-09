using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using TheGreatSidegrade.Content.Gores;
using TheGreatSidegrade.Content.Items.Starved.Placeable;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvingTreeBase : ModTree {
    public override TreePaintingSettings TreeShaderSettings => new() {
        UseSpecialGroups = true,
        SpecialGroupMinimalHueValue = 11f / 72f,
        SpecialGroupMaximumHueValue = 0.25f,
        SpecialGroupMinimumSaturationValue = 0.88f,
        SpecialGroupMaximumSaturationValue = 1f
    };

    public override int CreateDust() => 0;// ModContent.DustType<StarvewoodDust>();

    public override void SetStaticDefaults() {
        GrowsOnTileId = [ModContent.TileType<StarvedGrass>(), ModContent.TileType<StarvingTree>()];
    }

    public override bool Shake(int x, int y, ref bool createLeaves) => false;

    public override Asset<Texture2D> GetTexture() => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree");

    public override int SaplingGrowthType(ref int style) {
        style = 0;
        return ModContent.TileType<StarvingSapling>();
    }

    public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight) { }

    public override int TreeLeaf() => ModContent.GoreType<StarvingTreeLeaf>();

    public override Asset<Texture2D> GetBranchTextures() => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree_Branches");

    public override Asset<Texture2D> GetTopTextures() => ModContent.Request<Texture2D>($"{nameof(TheGreatSidegrade)}/Content/Tiles/Starved/StarvingTree_Tops");

    public override int DropWood() => ModContent.ItemType<Starvewood>();
}