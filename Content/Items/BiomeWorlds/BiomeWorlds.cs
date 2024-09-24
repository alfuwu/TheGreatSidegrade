using SubworldLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.IO;
using Terraria.WorldBuilding;
using TheGreatSidegrade.Common;

namespace TheGreatSidegrade.Content.Items.BiomeWorlds;

[ExtendsFromMod("BiomeWorlds", "SubworldLibrary")]
public class BiomeWorlds {
    public static bool EnterWorld<T>(Player player) where T : Subworld {
        if (player.whoAmI == Main.myPlayer) {
            if (SubworldSystem.Current == null || (SubworldSystem.Current != null && SubworldSystem.Current.GetType() != typeof(T))) {
                bool bl = SubworldSystem.Enter<T>();
                if (!bl) {
                    string message;
                    if (!SubworldSystem.IsActive<T>()) {
                        message = "SubworldLibrary Mod is required to be enabled for this item to work!";
                    } else {
                        DefaultInterpolatedStringHandler val = default;
                        val.AppendLiteral("Unable to enter ");
                        val.AppendFormatted(SubworldSystem.GetIndex<T>());
                        val.AppendLiteral("!");
                        message = val.ToStringAndClear();
                    }
                    if (Main.netMode == NetmodeID.Server) {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), Color.Orange, -1);
                        return bl;
                    } else {
                        Main.NewText(message, Color.Orange);
                    }
                }
                return bl;
            }
            SoundEngine.PlaySound(SoundID.Item6, player.position, null);
            player.Spawn(PlayerSpawnContext.RecallFromItem);
        }
        return true;
    }
}

[ExtendsFromMod("BiomeWorlds", "SubworldLibrary")]
public abstract class BaseWorldGenPass : GenPass {
    internal Dictionary<ushort, ushort> replacementTiles = [];
    internal Dictionary<ushort, ushort> replacementWalls = [];
    internal abstract GreatlySidegradedWorld.WorldEvil WorldEvilType { get; }

    public BaseWorldGenPass() : base("Standard World", 100.0) { }

    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration) {
        GenerationProgress currentGenerationProgress = WorldGenerator.CurrentGenerationProgress;
        WorldGen.WorldGenParam_Evil = (int) WorldEvilType;
        WorldGen.GenerateWorld(Main.ActiveWorldFileData.Seed, null);
        WorldGenerator.CurrentGenerationProgress = currentGenerationProgress;
        for (int x = 0; x < Main.maxTilesX; x++) {
            for (int y = 0; y < Main.maxTilesY; y++) {
                Tile tile = Main.tile[x, y];
                if (!tile.HasTile && tile.WallType == 0)
                    continue;
                for (int i = 0; i < replacementTiles.Count; i++) {
                    if (tile.TileType == replacementTiles.Keys.ToList()[i]) {
                        tile.TileType = replacementTiles.Values.ToList()[i];
                        break;
                    }
                }
                for (int i = 0; i < replacementWalls.Count; i++) {
                    if (tile.WallType == replacementWalls.Keys.ToList()[i]) {
                        tile.WallType = replacementWalls.Values.ToList()[i];
                        break;
                    }
                }
            }
        }
    }
}
