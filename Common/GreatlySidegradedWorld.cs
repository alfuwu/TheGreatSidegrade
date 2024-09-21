using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Chat;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using TheGreatSidegrade.Common.Abstract;
using TheGreatSidegrade.Content.WorldGeneration.Passes;

namespace TheGreatSidegrade.Common;

public class GreatlySidegradedWorld : ModSystem {
    public static Color FracturedBiomeColor = new(111, 110, 110);
    public static Color NothingBiomeColor = new(10, 10, 10);
    public static Color RottenBiomeColor = new(142, 76, 59);
    public static Color SpiralBiomeColor = new(56, 76, 59);
    public static Color StarvedBiomeColor = new(199, 173, 90);

    public static WorldEvil worldEvil;

    public static List<Type> ActiveEvents = [];

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
        if (WorldGen.WorldGenParam_Evil == 0 || TheGreatSidegrade.IsContagion) {
            worldEvil = WorldEvil.Corruption;
        } else if (WorldGen.WorldGenParam_Evil == 1) {
            worldEvil = WorldEvil.Crimson;
        } else if (WorldGen.WorldGenParam_Evil == -1) {
            Mod.Logger.Info("RANDOM WORLD EVIL");
            byte avalonModifier = TheGreatSidegrade.HasAvalon ? (byte) 0 : (byte) 1;
            if (WorldGen.genRand.NextBool(Enum.GetValues<WorldEvil>().Length - 3, Enum.GetValues<WorldEvil>().Length - avalonModifier))
                worldEvil = PickRandomEvil();
            else if (TheGreatSidegrade.HasAvalon && TheGreatSidegrade.IsContagion)
                worldEvil = WorldEvil.Contagion;
            else if (WorldGen.genRand.NextBool(2))
                worldEvil = WorldEvil.Crimson;
            else
                worldEvil = WorldEvil.Corruption;
            Mod.Logger.Info("CRIMSN: " + WorldGen.crimson);
            Mod.Logger.Info("WORLDE EVL: " + Enum.GetName(worldEvil));
        }
        if (WorldGen.WorldGenParam_Evil > 2)
            worldEvil = (WorldEvil) WorldGen.WorldGenParam_Evil;
        Mod.Logger.Info(TheGreatSidegrade.IsContagion);
        WorldGen.crimson = worldEvil == WorldEvil.Crimson;
        if (TheGreatSidegrade.IsContagion && worldEvil != WorldEvil.Contagion)
            TheGreatSidegrade.IsContagion = false;
    }

    public static WorldEvil PickRandomEvil() {
        byte avalonModifier = TheGreatSidegrade.HasAvalon ? (byte) 0 : (byte) 1;
        return (WorldEvil) Main.rand.Next(Enum.GetValues<WorldEvil>().Length - 2 - avalonModifier) + 2 + avalonModifier;
    }

    public override void ModifyWorldGenTasks(List<GenPass> list, ref double totalWeight) {
        int index = list.FindIndex(genpass => genpass.Name.Equals("Corruption") || (genpass.Name.Equals("Contagion") && worldEvil != WorldEvil.Contagion));
        int index2 = list.FindIndex(genpass => genpass.Name.Equals("Evil Altars") || (genpass.Name.Equals("Icky Altars") && worldEvil != WorldEvil.Contagion));
        if (!IsVanillaEvil() && index > -1)
            list[index] = new PassLegacy("Corruption", GetWorldGenPass());
        if (!IsVanillaEvil() && index2 > -1)
            list[index2] = new EvilAltars();
    }

    public override void ModifyHardmodeTasks(List<GenPass> list) {
        //int index2 = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Good"));
        int index = list.FindIndex(genpass => genpass.Name.Equals("Hardmode Evil"));
        if (!IsVanillaEvil() && index > -1)
            list[index] = new PassLegacy("Hardmode Evil", GetHardModeWorldGenPass());
    }

    public static WorldGenLegacyMethod GetWorldGenPass() {
        return worldEvil switch {
            WorldEvil.Fractured => new WorldGenLegacyMethod(Starved.Method),
            WorldEvil.Nothing => new WorldGenLegacyMethod(Starved.Method),
            WorldEvil.Rotten => new WorldGenLegacyMethod(Starved.Method),
            WorldEvil.Spiral => new WorldGenLegacyMethod(Starved.Method),
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
    }

    public override void NetReceive(BinaryReader br) {
        worldEvil = (WorldEvil) br.ReadByte();
    }

    public override void SaveWorldData(TagCompound tag) {
        tag["WorldEvil"] = (byte) worldEvil;
        foreach (ModEvent e in ModContent.GetContent<ModEvent>()) {
            if (ActiveEvents.Contains(e.GetType())) {
                TagCompound compound = [];
                e.SaveEventData(compound);
                tag[e.GetType().Name] = compound;
            }
        }
    }

    public override void LoadWorldData(TagCompound tag) {
        if (tag.ContainsKey("WorldEvil"))
            worldEvil = (WorldEvil) tag.GetByte("WorldEvil");
        foreach (ModEvent e in ModContent.GetContent<ModEvent>())
            if (tag.ContainsKey(e.GetType().Name))
                StartEvent(e, tag.GetCompound(e.GetType().Name));
    }

    public override void OnWorldUnload() {
        foreach (ModEvent e in ModContent.GetContent<ModEvent>())
            if (ActiveEvents.Contains(e.GetType()))
                e.Reset();
        ActiveEvents.Clear();
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
            if (e.CanStart.Invoke() && Main.rand.NextFloat() < e.StartChance.Invoke() && !ActiveEvents.Contains(e.GetType()))
                StartEvent(e);
        TheGreatSidegrade.NightJustStarted = false;
        TheGreatSidegrade.DayJustStarted = false;
        foreach (ModEvent e in events)
            if (e.CanEnd.Invoke() && ActiveEvents.Contains(e.GetType()))
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
    public static bool IsVanillaEvil(WorldEvil evil) => evil == WorldEvil.Corruption || worldEvil == WorldEvil.Crimson || worldEvil == WorldEvil.Contagion;
}
