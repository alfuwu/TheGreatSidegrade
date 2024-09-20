using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheGreatSidegrade.Content.Walls.Starved;

public class StarvedGrassWall : ModWall {
    public override void SetStaticDefaults() {
        AddMapEntry(new Color(106, 116, 59));
        HitSound = SoundID.Grass;
        WallID.Sets.Conversion.Grass[Type] = true;
        //DustType = ModContent.DustType<StarvedDust>();
    }
}
