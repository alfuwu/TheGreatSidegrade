using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.Tiles.Fractured;
using TheGreatSidegrade.Content.Tiles.Nothing;
using TheGreatSidegrade.Content.Tiles.Rotten;
using TheGreatSidegrade.Content.Tiles.Spiral;
using TheGreatSidegrade.Content.Tiles.Starved;

namespace TheGreatSidegrade.Common.Hooks;

public class WorldGenHooks {
    public static void TileRunner(On_WorldGen.orig_TileRunner orig, int i, int j, double strength, int steps, int type, bool addTile, double speedX, double speedY, bool noYChange, bool overRide, int ignoreTileType) {
        if ((type == TileID.Demonite || type == TileID.Crimtane /* nvm it does im just stupid */ || (TheGreatSidegrade.HasAvalon && TheGreatSidegrade.Avalon.TryFind("BacciliteOre", out ModTile tile) && type == tile.Type)) && !GreatlySidegradedWorld.IsVanillaEvil())
            type = GreatlySidegradedWorld.worldEvil switch {
                GreatlySidegradedWorld.WorldEvil.Fractured => ModContent.TileType<Fractite>(),
                GreatlySidegradedWorld.WorldEvil.Nothing => ModContent.TileType<Oblivium>(),
                GreatlySidegradedWorld.WorldEvil.Rotten => ModContent.TileType<Rust>(),
                GreatlySidegradedWorld.WorldEvil.Spiral => ModContent.TileType<Spite>(),
                GreatlySidegradedWorld.WorldEvil.Starved => ModContent.TileType<Hungrite>(),
                _ => type
            };
        orig(i, j, strength, steps, type, addTile, speedX, speedY, noYChange, overRide, ignoreTileType);
    }
}
