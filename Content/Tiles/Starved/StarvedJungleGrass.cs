using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheGreatSidegrade.Common;

namespace TheGreatSidegrade.Content.Tiles.Starved;

public class StarvedJungleGrass : ModTile {
    /*
     * public static List<int> CorruptCountCollection = new List<int> {
			23,
			661,
			25,
			112,
			163,
			398,
			400,
			636,
			24,
			32
		};
		public static bool[] CorruptBiomeSight = Factory.CreateBoolSet(23, 661, 25, 112, 163, 398, 400, 636, 24, 32);
    */
    public override bool IsTileBiomeSightable(int i, int j, ref Color sightColor) {
        sightColor = GreatlySidegradedWorld.StarvedBiomeColor;
        return true;
    }

    public override void SetStaticDefaults() {
        Main.tileSolid[Type] = true;
        Main.tileBrick[Type] = true;
        Main.tileBlockLight[Type] = true;
        TileID.Sets.Conversion.JungleGrass[Type] = true;
        TileID.Sets.Conversion.MergesWithDirtInASpecialWay[Type] = true;
        TileID.Sets.SpreadOverground[Type] = true;
        TileID.Sets.SpreadUnderground[Type] = true;
        TileID.Sets.CanBeDugByShovel[Type] = true;
        TileID.Sets.NeedsGrassFraming[Type] = true;
        TileID.Sets.NeedsGrassFramingDirt[Type] = TileID.Mud;
        TileID.Sets.Grass[Type] = true;
        TileID.Sets.GrassSpecial[Type] = true;
        TileID.Sets.DoesntPlaceWithTileReplacement[Type] = true;
        GreatlySidegradedIDs.Sets.StarvedTileCollection.Add(Type);

        RegisterItemDrop(ItemID.MudBlock);
    }
}