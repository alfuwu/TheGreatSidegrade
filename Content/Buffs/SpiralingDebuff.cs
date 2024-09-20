using Terraria;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Buffs {
    public class SpiralingDebuff : ModBuff {
        public override void SetStaticDefaults() {
            Main.debuff[Type] = true;
        }
    }
}
