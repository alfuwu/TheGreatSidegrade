using System;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheGreatSidegrade.Common.Abstract;

public abstract class ModEvent : ILoadable, ILocalizedModType {
    public abstract Func<float> StartChance { get; }
    public abstract Func<bool> CanStart { get; }
    public abstract Func<bool> CanEnd { get; }
    public virtual bool CanHappenDuringOtherEvents { get => false; }
    public string LocalizationCategory => "Events";
    public virtual string Name { get => GetType().Name; }
    public virtual string FullName => $"{Mod.Name}/{Name}";
    public virtual string GetStartText => this.GetLocalization("Start").Value;
    public virtual string GetEndText => Language.Exists(this.GetLocalizationKey("End")) ? this.GetLocalization("End").Value : null;
    public Mod Mod { get; private set; }

    /// <summary>
    /// This function gets called when the event begins.
    /// </summary>
    public virtual void OnEventStart() {

    }

    /// <summary>
    /// This function gets called when the event ends. Can be used for granting the player achievements, etc.
    /// </summary>
    public virtual void OnEventEnd() {

    }

    /// <summary>
    /// Gets called when the world is unloaded while the event is active.
    /// </summary>
    public virtual void Reset() {

    }

    /// <summary>
    /// This function gets called before every world update.
    /// </summary>
    public virtual void EventPreUpdate() {

    }

    /// <summary>
    /// This function gets called after every world update.
    /// </summary>
    public virtual void EventPostUpdate() {

    }

    /// <summary>
    /// Allows saving custom event data to the world's NBT tag.
    /// </summary>
    /// <param name="tag">A tag compound</param>
    public virtual void SaveEventData(TagCompound tag) {

    }

    /// <summary>
    /// Allows loading custom event data from the world's NBT tag.
    /// </summary>
    /// <param name="tag">A tag compound</param>
    public virtual void LoadEventData(TagCompound tag) {

    }

    public void Load(Mod mod) {
        Mod = mod;
    }

    public void Unload() {
        
    }
}
