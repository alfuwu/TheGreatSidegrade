using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Buffs;

public class Doomed : ModBuff { // the Nothing main debuff
    public override void SetStaticDefaults() {
        Main.debuff[Type] = true;
        BuffID.Sets.LongerExpertDebuff[Type] = true;
    }
}
