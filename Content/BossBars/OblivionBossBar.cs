using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ReLogic.Graphics;

namespace TheGreatSidegrade.Content.BossBars;

public class OblivionBossBar : ModBossBar {
    private int bossHeadIndex = -1;

    public override Asset<Texture2D> GetIconTexture(ref Rectangle? iconFrame) {
        if (bossHeadIndex != -1)
            return TextureAssets.NpcHeadBoss[bossHeadIndex];
        return null;
    }

    public override bool? ModifyInfo(ref BigProgressBarInfo info, ref float life, ref float lifeMax, ref float shield, ref float shieldMax) {
        NPC npc = Main.npc[info.npcIndexToAimAt];
        if (!npc.active)
            return false;

        bossHeadIndex = npc.GetBossHeadTextureIndex();
        return true;
    }

	public static void DrawHealthText(SpriteBatch spriteBatch, Rectangle area) {
        DynamicSpriteFont value = FontAssets.ItemStack.Value;
        Vector2 vector = area.Center.ToVector2();
        vector.Y += 1f;
        string text = "/";
        Vector2 vector2 = value.MeasureString(text);
        Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X, vector.Y, Color.White, Color.Black, vector2 * 0.5f);
        text = "Infinty";
        vector2 = value.MeasureString(text);
        Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X - 5f, vector.Y, Color.White, Color.Black, vector2 * new Vector2(1f, 0.5f));
        Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X + 5f, vector.Y, Color.White, Color.Black, vector2 * new Vector2(0f, 0.5f));
    }

    public override bool PreDraw(SpriteBatch spriteBatch, NPC npc, ref BossBarDrawParams drawParams) {
        Rectangle? iconFrame = null;
        Texture2D iconTexture = (GetIconTexture(ref iconFrame) ?? TextureAssets.NpcHead[0]).Value;
        iconFrame ??= iconTexture.Frame();

        // aaaa duplication of vanilla method
        // rip compat with any mod that modifies every boss bar for some reason
        // anyways
        Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar").Value;
		Point p = new(456, 22);
		Point p2 = new(32, 24);
		int verticalFrames = 6;
		Rectangle value2 = value.Frame(1, verticalFrames, 0, 3);
		Color color = Color.White * 0.2f;
		int num2 = p.X;
		num2 -= num2 % 2;
		Rectangle value3 = value.Frame(1, verticalFrames, 0, 2);
		value3.X += p2.X;
		value3.Y += p2.Y;
		value3.Width = 2;
		value3.Height = p.Y;
		Rectangle value4 = value.Frame(1, verticalFrames, 0, 1);
		value4.X += p2.X;
		value4.Y += p2.Y;
		value4.Width = 2;
		value4.Height = p.Y;
		Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), p.ToVector2());
		Vector2 vector = rectangle.TopLeft() - p2.ToVector2();
		spriteBatch.Draw(value, vector, value2, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		spriteBatch.Draw(value, rectangle.TopLeft(), value3, Color.White, 0f, Vector2.Zero, new Vector2(num2 / value3.Width, 1f), SpriteEffects.None, 0f);
		spriteBatch.Draw(value, rectangle.TopLeft() + new Vector2(num2 - 2, 0f), value4, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		Rectangle value5 = value.Frame(1, verticalFrames);
		spriteBatch.Draw(value, vector, value5, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		Vector2 vector2 = new Vector2(4f, 20f) + new Vector2(26f, 28f) / 2f;
		spriteBatch.Draw(iconTexture, vector + vector2, iconFrame.Value, Color.White, 0f, iconFrame.Value.Size() / 2f, 1f, SpriteEffects.None, 0f);
		if (BigProgressBarSystem.ShowText)
			DrawHealthText(spriteBatch, rectangle);
        return false;
    }
}