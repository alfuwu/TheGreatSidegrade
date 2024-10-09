using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using TheGreatSidegrade.Common.Abstract;
using TheGreatSidegrade.Common.Hooks;
using TheGreatSidegrade.Common.Hooks.Compatibility;
using TheGreatSidegrade.Content.WorldGeneration.Passes;

namespace TheGreatSidegrade.Common;

public class GreatlySidegradedWorld : ModSystem {
    public static readonly Color FracturedBiomeColor = new(111, 110, 110);
    public static readonly Color NothingBiomeColor = new(10, 10, 10);
    public static readonly Color RottenBiomeColor = new(142, 76, 59);
    public static readonly Color SpiralBiomeColor = new(56, 76, 59);
    public static readonly Color StarvedBiomeColor = new(199, 173, 90);

    public static int FracTree = 0, EldTree = 0, RotTree = 0, HelixTree = 0, StarvingTree = 0;

    public static WorldEvil worldEvil;

    public static readonly List<Type> ActiveEvents = [];

    public static int totalFract = 0, totalVoid = 0, totalRot = 0, totalTwisted = 0, totalStarved = 0;
    public static int totalFract2 = 0, totalVoid2 = 0, totalRot2 = 0, totalTwisted2 = 0, totalStarved2 = 0;
    public static byte tFract = 0, tVoid = 0, tRot = 0, tTwisted = 0, tStarved = 0;

    public enum WorldEvil {
        Corruption,
        Crimson,
        Contagion, // avalon
        Fractured,
        Nothing,
        Rotten,
        Spiral,
        Starved
    }

    public override void PreWorldGen() {
        WorldGen.WorldGenParam_Evil = (int)WorldUIHooks.SelectedWorldEvil - 1;
        if (WorldGen.WorldGenParam_Evil == 0) {
            worldEvil = WorldEvil.Corruption;
        } else if (WorldGen.WorldGenParam_Evil == 1) {
            worldEvil = WorldEvil.Crimson;
        } else if (WorldGen.WorldGenParam_Evil == -1) {
            Mod.Logger.Info("RANDOM WORLD EVIL");
            byte avalonModifier = (byte)(TheGreatSidegrade.HasAvalon ? 0 : 1);
            if (WorldGen.genRand.NextBool(Enum.GetValues<WorldEvil>().Length - 3, Enum.GetValues<WorldEvil>().Length - avalonModifier))
                worldEvil = PickRandomEvil();
            else if (TheGreatSidegrade.IsContagion)
                worldEvil = WorldEvil.Contagion;
            else if (WorldGen.genRand.NextBool(2))
                worldEvil = WorldEvil.Crimson;
            else
                worldEvil = WorldEvil.Corruption;
            Mod.Logger.Info("CRIMSN: " + WorldGen.crimson);
            Mod.Logger.Info("WORLDE EVL: " + Enum.GetName(worldEvil));
        } else if (WorldGen.WorldGenParam_Evil > 1 && WorldGen.WorldGenParam_Evil < Enum.GetValues<WorldEvil>().Length) {
            worldEvil = (WorldEvil)WorldGen.WorldGenParam_Evil;
        }

        // scuffed, but works
        WorldGen.WorldGenParam_Evil = worldEvil == WorldEvil.Crimson ? 1 : 0;
        WorldGen.crimson = WorldGen.WorldGenParam_Evil == 1;
        if (TheGreatSidegrade.HasAvalon)
            ContagionSelectionMenuHook.SetContagion(worldEvil == WorldEvil.Contagion);
    }

    public static WorldEvil PickRandomEvil() {
        byte avalonModifier = TheGreatSidegrade.HasAvalon ? (byte) 0 : (byte) 1;
        return (WorldEvil)Main.rand.Next(Enum.GetValues<WorldEvil>().Length - 2 - avalonModifier) + 2 + avalonModifier;
    }

    public override void ModifyWorldGenTasks(List<GenPass> list, ref double totalWeight) {
        int index = list.FindIndex(genpass => genpass.Name.Equals("Corruption") || (genpass.Name.Equals("Contagion") && worldEvil != WorldEvil.Contagion));
        int index2 = list.FindIndex(genpass => genpass.Name.Equals("Evil Altars") || (genpass.Name.Equals("Icky Altars") && worldEvil != WorldEvil.Contagion));
        if (!IsVanillaEvil() && index > -1) {
            if (WorldGen.drunkWorldGen)
                list.Insert(0, new PassLegacy(Enum.GetName(worldEvil), GetWorldGenPass()));
            else
                list[index] = new PassLegacy("Corruption", GetWorldGenPass());
        }
        if (!IsVanillaEvil() && index2 > -1)
            list[index2] = new EvilAltars();
    }

    public override void ModifyHardmodeTasks(List<GenPass> list) {
        //int index2 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));
        int index = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Evil"));
        if (!IsVanillaEvil() && index > -1) {
            if (WorldGen.drunkWorldGen)
                list.Insert(index + 1, new PassLegacy($"Hardmode {Enum.GetName(worldEvil)}", GetHardModeWorldGenPass()));
            else
                list[index] = new PassLegacy("Hardmode Evil", GetHardModeWorldGenPass());
        }
    }

    public static WorldGenLegacyMethod GetWorldGenPass() {
        return worldEvil switch {
            WorldEvil.Fractured => new WorldGenLegacyMethod(Fractured.Method),
            WorldEvil.Nothing => new WorldGenLegacyMethod(Nothing.Method),
            WorldEvil.Rotten => new WorldGenLegacyMethod(Rotten.Method),
            WorldEvil.Spiral => new WorldGenLegacyMethod(Spiral.Method),
            WorldEvil.Starved => new WorldGenLegacyMethod(Starved.Method),
            _ => null
        };
    }

    public static WorldGenLegacyMethod GetHardModeWorldGenPass() {
        return worldEvil switch {
            WorldEvil.Fractured => new WorldGenLegacyMethod(StarvedHardMode.Method),
            WorldEvil.Nothing => new WorldGenLegacyMethod(StarvedHardMode.Method),
            WorldEvil.Rotten => new WorldGenLegacyMethod(StarvedHardMode.Method),
            WorldEvil.Spiral => new WorldGenLegacyMethod(StarvedHardMode.Method),
            WorldEvil.Starved => new WorldGenLegacyMethod(StarvedHardMode.Method),
            _ => null
        };
    }

    public override void NetSend(BinaryWriter bw) {
        bw.Write((byte) worldEvil);
        bw.Write(tFract);
        bw.Write(tVoid);
        bw.Write(tRot);
        bw.Write(tTwisted);
        bw.Write(tStarved);

        // tSick isn't syncing and i dont know why
        // but the dryad doesn't think the world is sick at all ever
        //if (TheGreatSidegrade.HasAvalon && TheGreatSidegrade.Avalon.TryFind("AvalonWorld", out ModSystem world))
        //    bw.Write((byte) world.GetType().GetField("tSick", BindingFlags.Public | BindingFlags.Static).GetValue(null));
    }

    public override void NetReceive(BinaryReader br) {
        worldEvil = (WorldEvil) br.ReadByte();
        tFract = br.ReadByte();
        tVoid = br.ReadByte();
        tRot = br.ReadByte();
        tTwisted = br.ReadByte();
        tStarved = br.ReadByte();
        TheGreatSidegrade.Mod.Logger.Info("YES YES YES LOADING");

        //if (TheGreatSidegrade.HasAvalon && TheGreatSidegrade.Avalon.TryFind("AvalonWorld", out ModSystem world))
        //    world.GetType().GetField("tSick", BindingFlags.Public | BindingFlags.Static).SetValue(null, br.ReadByte());
    }

    public override void SaveWorldData(TagCompound tag) {
        if (!IsVanillaEvil(worldEvil))
            tag["WorldEvil"] = (byte) worldEvil;
        foreach (ModEvent e in ModContent.GetContent<ModEvent>()) {
            if (ActiveEvents.Contains(e.GetType())) {
                TagCompound compound = [];
                e.SaveEventData(compound);
                tag[e.GetType().Name] = compound;
            }
        }
    }

    public override void SaveWorldHeader(TagCompound tag) {
        if (!IsVanillaEvil(worldEvil)) // we don't care about the icon if its vanilla/contagion
            tag["WorldEvil"] = (byte) worldEvil;
    }


    public override void LoadWorldData(TagCompound tag) {
        for (int i = 0; i < Main.tile.Width; ++i) {
            for (int j = 0; j < Main.tile.Height; ++j) { // expensiv
                if (GreatlySidegradedIDs.Sets.FracturedTileCollection.Contains(Main.tile[i, j].TileType))
                    totalFract2++;
                if (GreatlySidegradedIDs.Sets.VoidTileCollection.Contains(Main.tile[i, j].TileType))
                    totalVoid2++;
                if (GreatlySidegradedIDs.Sets.RottenTileCollection.Contains(Main.tile[i, j].TileType))
                    totalRot2++;
                if (GreatlySidegradedIDs.Sets.SpiralTileCollection.Contains(Main.tile[i, j].TileType))
                    totalTwisted2++;
                if (GreatlySidegradedIDs.Sets.StarvedTileCollection.Contains(Main.tile[i, j].TileType))
                    totalStarved2++;
            }
        }
        if (tag.TryGet("WorldEvil", out byte tmp))
            worldEvil = (WorldEvil) tmp;
        foreach (ModEvent e in ModContent.GetContent<ModEvent>())
            if (tag.ContainsKey(e.GetType().Name))
                StartEvent(e, tag.GetCompound(e.GetType().Name));
    }

    public override void OnWorldUnload() {
        foreach (ModEvent e in ModContent.GetContent<ModEvent>())
            if (ActiveEvents.Contains(e.GetType()))
                e.Reset();
        ActiveEvents.Clear();

        totalFract = 0;
        totalVoid = 0;
        totalRot = 0;
        totalTwisted = 0;
        totalStarved = 0;
        totalFract2 = 0;
        totalVoid2 = 0;
        totalRot2 = 0;
        totalTwisted2 = 0;
        totalStarved2 = 0;
        tFract = 0;
        tVoid = 0;
        tRot = 0;
        tTwisted = 0;
        tStarved = 0;
    }

    public override void PreUpdateWorld() {
        foreach (ModEvent e in ModContent.GetContent<ModEvent>())
            if (ActiveEvents.Contains(e.GetType()))
                e.EventPreUpdate();
    }

    public override void PostUpdateWorld() {
        if (TheGreatSidegrade.NightJustStarted)
            Mod.Logger.Info(TheGreatSidegrade.NightJustStarted);
        IEnumerable<ModEvent> events = ModContent.GetContent<ModEvent>();
        foreach (ModEvent e in events)
            if (ActiveEvents.Contains(e.GetType()))
                e.EventPostUpdate();
        foreach (ModEvent e in events)
            if (e.CanStart() && Main.rand.NextFloat() < e.StartChance() && !ActiveEvents.Contains(e.GetType()))
                StartEvent(e);
        TheGreatSidegrade.NightJustStarted = false;
        TheGreatSidegrade.DayJustStarted = false;
        foreach (ModEvent e in events)
            if (e.CanEnd() && ActiveEvents.Contains(e.GetType()))
                EndEvent(e);
    }

    public static void StartEvent(ModEvent e, TagCompound tag = null) {
        ActiveEvents.Add(e.GetType());
        if (tag != null)
            e.LoadEventData(tag);
        e.OnEventStart();
        if (e.GetStartText != null) {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(e.GetStartText, 50, 255, 130);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(e.GetStartText), new(50, 255, 130));
        }
    }

    public static void EndEvent(ModEvent e) {
        ActiveEvents.Remove(e.GetType());
        e.OnEventEnd();
        if (e.GetEndText != null) {
            if (Main.netMode == NetmodeID.SinglePlayer)
                Main.NewText(e.GetEndText, 50, 255, 130);
            else if (Main.netMode == NetmodeID.Server)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(e.GetEndText), new(50, 255, 130));
        }
    }

    public static bool IsVanillaEvil() => IsVanillaEvil(worldEvil);

    // contagion isn't vanilla, but for check purposes we don't want to do anything if the world is a contagion world
    public static bool IsVanillaEvil(WorldEvil evil) => evil == WorldEvil.Corruption || evil == WorldEvil.Crimson || evil == WorldEvil.Contagion;
}
