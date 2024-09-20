using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.WorldGeneration.Biomes;

public class StarvedUndergroundBackgroundStyle : ModUndergroundBackgroundStyle {
    public override void FillTextureArray(int[] textureSlots) {
        textureSlots[0] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/StarvedUnderground0");
        textureSlots[1] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/StarvedUnderground1");
        textureSlots[2] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/StarvedUnderground2");
        textureSlots[3] = BackgroundTextureLoader.GetBackgroundSlot(Mod, "Assets/Textures/Backgrounds/StarvedUnderground3");
    }
}
