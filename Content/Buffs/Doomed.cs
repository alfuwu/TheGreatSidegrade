using Terraria;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Buffs;

public class Doomed : ModBuff { // the Nothing main debuff
    public override void SetStaticDefaults() {
        Main.debuff[Type] = true;
    }
}
