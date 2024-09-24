using Terraria;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.DamageTypes;

public class HolyDamageClass : DamageClass {
    public override StatInheritanceData GetModifierInheritance(DamageClass damageClass) {
        if (damageClass == Generic)
            return StatInheritanceData.Full;

        return new StatInheritanceData(
            damageInheritance: 1f,
            critChanceInheritance: 1f,
            attackSpeedInheritance: 0f,
            armorPenInheritance: 2f,
            knockbackInheritance: 0f
        );
    }

    public override bool GetEffectInheritance(DamageClass damageClass) => damageClass == Magic;

    public override void SetDefaultStats(Player player) {
        // faith scaling?
        // yes
    }

    public override bool UseStandardCritCalcs => true;

    public override bool ShowStatTooltipLine(Player player, string lineName) => lineName != "Speed";
}
