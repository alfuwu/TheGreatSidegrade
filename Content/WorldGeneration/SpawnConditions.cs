using TheGreatSidegrade.Common;

namespace TheGreatSidegrade.Content.WorldGeneration;

public static class SpawnConditions {
    public static ModSpawnConditionBestiaryInfoElement TheFractured { get; } = new($"{TheGreatSidegrade.Localization}.Bestiary.Biomes.TheFractured", "UI/WorldCreation/IconEvilFractured", "UI/Bestiary/FracturedBG", new(200, 200, 200));
    public static ModSpawnConditionBestiaryInfoElement TheNothing { get; } = new($"{TheGreatSidegrade.Localization}.Bestiary.Biomes.TheNothing", "UI/WorldCreation/IconEvilNothing", "UI/Bestiary/NothingBG", new(200, 200, 200));
    public static ModSpawnConditionBestiaryInfoElement TheRotten { get; } = new($"{TheGreatSidegrade.Localization}.Bestiary.Biomes.TheRotten", "UI/WorldCreation/IconEvilRotten", "UI/Bestiary/RottenBG", new(200, 200, 200));
    public static ModSpawnConditionBestiaryInfoElement TheSpiral { get; } = new($"{TheGreatSidegrade.Localization}.Bestiary.Biomes.TheSpiral", "UI/WorldCreation/IconEvilSpiral", "UI/Bestiary/SpiralBG", new(200, 200, 200));
    public static ModSpawnConditionBestiaryInfoElement TheStarved { get; } = new($"{TheGreatSidegrade.Localization}.Bestiary.Biomes.TheStarved", "UI/WorldCreation/IconEvilStarved", "UI/Bestiary/StarvedBG", new(200, 200, 200));
}
