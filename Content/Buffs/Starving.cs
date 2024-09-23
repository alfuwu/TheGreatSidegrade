using Terraria;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Buffs;

public class Starving : ModBuff { // the Starved main debuff
    public override void SetStaticDefaults() {
        Main.debuff[Type] = true;
    }
}
