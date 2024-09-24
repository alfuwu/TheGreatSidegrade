using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.Dusts.Starved;

namespace TheGreatSidegrade.Content.WorldGeneration.Biomes;

public class StarvedWaterStyle : ModWaterStyle {
    private Asset<Texture2D> rainTexture;

    public override void Load() {
        rainTexture = Mod.Assets.Request<Texture2D>("Content/WorldGeneration/Biomes/StarvedRain");
    }

    public override int ChooseWaterfallStyle() {
        return ModContent.GetInstance<StarvedWaterfallStyle>().Slot;
    }

    public override int GetSplashDust() {
        return ModContent.DustType<OrangeSolution>();
    }

    public override int GetDropletGore() {
        return ModContent.GoreType<StarvedDroplet>();
    }

    public override void LightColorMultiplier(ref float r, ref float g, ref float b) {
        r = 1f;
        g = 1f;
        b = 1f;
    }

    public override Color BiomeHairColor() {
        return Color.Yellow;
    }

    public override byte GetRainVariant() {
        return (byte)Main.rand.Next(3);
    }

    public override Asset<Texture2D> GetRainTexture() => rainTexture;
}
