using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace TheGreatSidegrade;

public static class Extensions {
    public static Asset<Texture2D> GetTexture(this ModItem item) => ModContent.Request<Texture2D>(item.Texture);

    public static Rectangle GetDims(this ModItem item) {
        if (Main.netMode != NetmodeID.Server)
            return item.GetTexture().Frame();

        return Rectangle.Empty;
    }
}
