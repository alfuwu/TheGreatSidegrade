using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Common.Abstract;

public abstract class ModGod : ModType, ILocalizedModType {
    private static readonly Dictionary<string, ModGod> gods = [];
    public string LocalizationCategory => "Gods";

    /// <summary>
    /// Allows retrieving a God by using their name
    /// </summary>
    /// <param name="name">The name of the desired God (case sensitive)</param>
    /// <returns>The ModGod instance of the requested God</returns>
    public static ModGod GetGod(string name) => gods.TryGetValue(name, out ModGod god) ? god : null;

    internal static float FaithModification(float faith) { // magic numbers go brrrr
        return (float)Math.Pow((Math.Log10(Math.Pow(faith, 20)) - 5) / 25, 3) + 1.008f;
    }

    public sealed override void SetupContent() => SetStaticDefaults();

    protected sealed override void Register() => gods.Add(Name, this); // ModTypeLookup<ModGod>.Register(this);

    /// <summary>
	/// Allows you to modify the power of the player's natural life regeneration. This can be done by multiplying the regen parameter by any number. For example, campfires multiply it by 1.1, while walking multiplies it by 0.5.
	/// </summary>
	/// <param name="regen"></param>
	public virtual void ModifyLifeRegen(GreatlySidegradedPlayer player, ref float regen) { }

    public virtual void ModifyHurt(GreatlySidegradedPlayer player, ref Player.HurtModifiers info) { }

    /// <summary>
    /// Allows you to prevent a faithful's death.
    /// </summary>
    /// <param name="player">The player that is about to die</param>
    /// <returns></returns>
    public virtual bool PreKill(GreatlySidegradedPlayer player) => true;
}
