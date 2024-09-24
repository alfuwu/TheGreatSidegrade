using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using TheGreatSidegrade.Content.Buffs;

namespace TheGreatSidegrade.Common;

public class GreatlySidegradedPlayer : ModPlayer {
    public int faith;
    private bool isEternalized;

    public override void ModifyManaCost(Item item, ref float reduce, ref float mult) {
        if (isEternalized)
            mult *= 99999999f; // just give it some real high value or smthn to prevent anything from working
    }

    public override void ModifyHurt(ref Player.HurtModifiers info) {
        if (isEternalized) {
            info.ModifyHurtInfo += (ref Player.HurtInfo info) => {
                info.Damage = 0;
                info.SourceDamage = 0;
            };
            Player.statLife += 1; // heal the 1 minimum damage you'd take
        }
    }

    public override void GetHealLife(Item item, bool quickHeal, ref int healValue) {
        if (isEternalized)
            healValue = 0;
    }

    public override void GetHealMana(Item item, bool quickHeal, ref int healValue) {
        if (isEternalized)
            healValue = 0;
    }

    public override bool ModifyNurseHeal(NPC nurse, ref int health, ref bool removeDebuffs, ref string chatText) {
        if (isEternalized) {
            chatText = Language.GetTextValue($"Mods.{nameof(TheGreatSidegrade)}.Dialogue.Nurse.CantHeal");
            return false;
        }
        return base.ModifyNurseHeal(nurse, ref health, ref removeDebuffs, ref chatText);
    }

    public override void PreUpdate() {
        if (Player.HasBuff<Eternalized>())
            isEternalized = true;
    }

    public override void PostUpdate() {
        if (isEternalized && !Player.HasBuff<Eternalized>()) {
            isEternalized = false;
            Player.AddBuff(ModContent.BuffType<CosmicRetribution>(), 3600);
        }
    }
}
