using Terraria;
using TheGreatSidegrade.Content.Buffs;

namespace TheGreatSidegrade.Common.Hooks;

public class PlayerHooks {
    public static void OnUpdateLifeRegen(On_Player.orig_UpdateLifeRegen orig, Player self) {
        if (!self.HasBuff<Eternalized>())
            orig(self);
    }
    
    public static void OnUpdateManaRegen(On_Player.orig_UpdateManaRegen orig, Player self) {
        if (!self.HasBuff<Eternalized>())
            orig(self);
    }
}
