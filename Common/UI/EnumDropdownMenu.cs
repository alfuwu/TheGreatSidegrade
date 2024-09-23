using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace TheGreatSidegrade.Common.UI;

public class EnumDropdownMenu<T> : UIElement {
    public struct Item(string texture, LocalizedText title, LocalizedText desc) {
        public string TexturePath = texture;
        public LocalizedText title = title;
        public LocalizedText description = desc;
    }

    T currentOption;
    private GroupOptionButton<T>[] options = new GroupOptionButton<T>[Enum.GetValues(typeof(T)).Length];
    private bool showing = false;

    public GroupOptionButton<T>[] Options { get => options; }

    public EnumDropdownMenu(Item[] items, Color textColor, float textSize = 1f, float titleAlignmentX = 0.5f, float titleWidthReduction = 10f) {
        for (int i = 0; i < items.Length; i++)
            options[i] = new((T) Enum.GetValues(typeof(T)).GetValue(i), items[i].title, items[i].description, textColor, items[i].TexturePath, textSize, titleAlignmentX, titleWidthReduction);
        SetCurrentOption((T) Enum.GetValues(typeof(T)).GetValue(0));
        foreach (GroupOptionButton<T> i in options)
            if (i.IsSelected)
                Append(i);
    }

    public void SetText(int i, LocalizedText text, float textSize, Color color) {
        options[i].SetText(text, textSize, color);
    }

    public void SetCurrentOption(T option) {
        currentOption = option;
        RemoveAllChildren();
        foreach (GroupOptionButton<T> i in options) {
            i.SetCurrentOption(option);
            if (i.IsSelected)
                Append(i);
        }
    }

    protected override void DrawSelf(SpriteBatch spriteBatch) {
        if (showing) {
            return; // unimplemented
        } else {
            
        }
    }

    public override void LeftMouseDown(UIMouseEvent evt) {
        SoundEngine.PlaySound(SoundID.MenuTick);
        showing = !showing;
        //base.LeftMouseDown(evt);
    }

    public override void MouseOver(UIMouseEvent evt) {
        base.MouseOver(evt);
        //_hovered = true;
    }

    public override void MouseOut(UIMouseEvent evt) {
        base.MouseOut(evt);
        //_hovered = false;
    }

    public void SetColor(int i, Color color, float opacity) {
        options[i].SetColor(color, opacity);
    }

    public void SetColorsBasedOnSelectionState(int i, Color pickedColor, Color unpickedColor, float opacityPicked, float opacityNotPicked) {
        options[i].SetColorsBasedOnSelectionState(pickedColor, unpickedColor, opacityPicked, opacityNotPicked);
    }

    public void SetBorderColor(int i, Color color) {
        options[i].SetBorderColor(color);
    }
}
