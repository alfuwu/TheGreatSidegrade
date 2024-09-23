using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;

namespace TheGreatSidegrade.Common.UI;

public class CustomGroupOptionButton<T> : GroupOptionButton<T> {
    public CustomGroupOptionButton(T option, LocalizedText title, LocalizedText description, Color textColor, Asset<Texture2D> icon, float textSize = 1f, float titleAlignmentX = 0.5f, float titleWidthReduction = 10f) : base(option, title, description, textColor, null, textSize, titleAlignmentX, titleWidthReduction) {
        typeof(GroupOptionButton<T>).GetField("_iconTexture", BindingFlags.Instance | BindingFlags.NonPublic)?.SetValue(this, icon);
    }
}
