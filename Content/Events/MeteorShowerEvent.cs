using System;
using Terraria;
using Terraria.GameContent.Ambience;
using TheGreatSidegrade.Common.Abstract;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace TheGreatSidegrade.Content.Events;

public class MeteorShowerEvent : ModEvent {
    public override Func<float> StartChance => () => 0.2f;
    public override Func<bool> CanStart => () => TheGreatSidegrade.NightJustStarted || TheGreatSidegrade.DayJustStarted;
    public override Func<bool> CanEnd => () => spawnedMeteors > 3 && Math.Pow(Main.rand.NextFloat(), 30) * lastMeteor > 60;
    public override bool CanHappenDuringOtherEvents => false;
    public override string Name => "MeteorShower";
    private static int spawnedMeteors = 0;
    private static int lastMeteor = 0;
    private const int bigNumber = 50000000;

    public override void EventPostUpdate() {
        lastMeteor++;
        if (Math.Pow(Main.rand.NextFloat(), 3) < 1 - bigNumber / Math.Pow(lastMeteor, 2)) {
            // not sure why i need to set this boolean but okay
            WorldGen.spawnMeteor = true;
            WorldGen.dropMeteor();
            WorldGen.spawnMeteor = false;
            spawnedMeteors++;
            lastMeteor = 0;
        }
    }

    public override void OnEventEnd() {
        Mod.Logger.Info("TRYING TO STOP EVENT...");
        Reset();
    }

    public override void Reset() {
        spawnedMeteors = 0;
        lastMeteor = 0;
    }

    public override void SaveEventData(TagCompound tag) {
        tag["spawnedMeteors"] = spawnedMeteors;
        tag["lastMeteor"] = lastMeteor;
    }

    public override void LoadEventData(TagCompound tag) {
        _ = tag.TryGet("spawnedMeteors", out spawnedMeteors);
        _ = tag.TryGet("lastMeteor", out lastMeteor);
    }
}
