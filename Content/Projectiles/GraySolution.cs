using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheGreatSidegrade.Content.Projectiles;

public class GraySolution : ModProjectile {
    public ref float Progress => ref Projectile.ai[0];

    public override void SetDefaults() {
        Projectile.DefaultToSpray();
        Projectile.aiStyle = 0;
    }

    public override void AI() {
        int dustType = ModContent.DustType<Dusts.Fractured.GraySolution>();

        if (Projectile.owner == Main.myPlayer)
            Convert((int)(Projectile.position.X + Projectile.width * 0.5f) / 16, (int)(Projectile.position.Y + Projectile.height * 0.5f) / 16, 2);

        if (Projectile.timeLeft > 133)
            Projectile.timeLeft = 133;

        if (Progress > 7f) {
            float dustScale = 1f;

            if (Progress == 8f)
                dustScale = 0.2f;
            else if (Progress == 9f)
                dustScale = 0.4f;
            else if (Progress == 10f)
                dustScale = 0.6f;
            else if (Progress == 11f)
                dustScale = 0.8f;

            Progress += 1f;

            Dust dust = Dust.NewDustDirect(new(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100);

            dust.noGravity = true;
            dust.scale *= 1.75f;
            dust.velocity.X *= 2f;
            dust.velocity.Y *= 2f;
            dust.scale *= dustScale;
        } else {
            Progress += 1f;
        }

        Projectile.rotation += 0.3f * Projectile.direction;
    }

    private static void Convert(int i, int j, int size = 4) {
        for (int k = i - size; k <= i + size; k++) {
            for (int l = j - size; l <= j + size; l++) {
                if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
                    int type = Main.tile[k, l].TileType;
                    int wall = Main.tile[k, l].WallType;

                    if (wall == WallID.Stone) {
                        //Main.tile[k, l].WallType = (ushort) ModContent.WallType<Walls.Fractured.Fracstone>();
                        WorldGen.SquareWallFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    }

                    if (TileID.Sets.Conversion.Stone[type]) {
                        //Main.tile[k, l].TileType = (ushort) ModContent.TileType<Fracstone>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    } else if (TileID.Sets.Conversion.Sand[type]) {
                        //Main.tile[k, l].TileType = (ushort) ModContent.TileType<Shattersand>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    } else if (TileID.Sets.Conversion.HardenedSand[type]) {
                        //Main.tile[k, l].TileType = (ushort) ModContent.TileType<HardenedShattersand>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    } else if (TileID.Sets.Conversion.Sandstone[type]) {
                        //Main.tile[k, l].TileType = (ushort) ModContent.TileType<Shattersandstone>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    } else if (TileID.Sets.Conversion.Ice[type]) {
                        //Main.tile[k, l].TileType = (ushort) ModContent.TileType<ShatteredIce>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    } else if (TileID.Sets.Conversion.Grass[type]) {
                        //Main.tile[k, l].TileType = (ushort) ModContent.TileType<Fracgrass>();
                        WorldGen.SquareTileFrame(k, l);
                        NetMessage.SendTileSquare(-1, k, l, 1);
                    } else if (TileID.Sets.Conversion.JungleGrass[type]) {
                        //Main.tile[k, l].TileType = (ushort) ModContent.TileType<JungleFracgrass>();
                    }
                }
            }
        }
    }
}
