using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using TheGreatSidegrade.Assets;

namespace TheGreatSidegrade.Common;

public class ModSpawnConditionBestiaryInfoElement : IBestiaryInfoElement, IBestiaryBackgroundImagePathAndColorProvider, IBestiaryPrioritizedElement {
    private readonly string _key;
    private readonly string _iconPath;
    private readonly string _backgroundImagePath;
    private Color? _backgroundColor;
    private readonly bool _hidePortrait;

    public float OrderPriority { get; set; }

    public ModSpawnConditionBestiaryInfoElement(string nameLanguageKey, string iconPath, string backgroundImagePath = null, Color? backgroundColor = null, bool hidePortraitInfo = false) {
        _key = nameLanguageKey;
        _iconPath = iconPath;
        _backgroundImagePath = backgroundImagePath;
        _backgroundColor = backgroundColor;
        _hidePortrait = hidePortraitInfo;
    }

    public Asset<Texture2D> GetBackgroundImage() {
        if (_backgroundImagePath == null)
            return null;

        return GameAssets.GetAsset($"Assets/Textures/{_backgroundImagePath}", AssetRequestMode.ImmediateLoad);
    }

    public Color? GetBackgroundColor() => _backgroundColor;

    public UIElement GetFilterImage() {
        return new UIImage(GameAssets.GetAsset($"Assets/Textures/{_iconPath}", AssetRequestMode.ImmediateLoad)) {
            HAlign = 0.5f,
            VAlign = 0.5f
        };
    }

    public string GetSearchString(ref BestiaryUICollectionInfo info) {
        if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
            return null;

        return Language.GetText(_key).Value;
    }

    public string GetDisplayNameKey() => _key;

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) {
        if (_hidePortrait)
            return null;

        if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
            return null;

        UIElement uIElement = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel"), null, 12, 7) {
            Width = new StyleDimension(-14f, 1f),
            Height = new StyleDimension(34f, 0f),
            BackgroundColor = new Color(43, 56, 101),
            BorderColor = Color.Transparent,
            Left = new StyleDimension(5f, 0f)
        };

        uIElement.SetPadding(0f);
        uIElement.PaddingRight = 5f;
        UIElement filterImage = GetFilterImage();
        filterImage.HAlign = 0f;
        filterImage.Left = new StyleDimension(5f, 0f);
        UIText element = new(Language.GetText(GetDisplayNameKey()), 0.8f) {
            HAlign = 0f,
            Left = new StyleDimension(38f, 0f),
            TextOriginX = 0f,
            TextOriginY = 0f,
            VAlign = 0.5f,
            DynamicallyScaleDownToWidth = true
        };

        if (filterImage != null)
            uIElement.Append(filterImage);

        uIElement.Append(element);
        AddOnHover(uIElement);
        return uIElement;
    }

    private void AddOnHover(UIElement button) {
        button.OnUpdate += e => ShowButtonName(e);
    }

    private void ShowButtonName(UIElement element) {
        if (element.IsMouseHovering) {
            string textValue = Language.GetTextValue(GetDisplayNameKey());
            Main.instance.MouseText(textValue, 0, 0);
        }
    }
}
