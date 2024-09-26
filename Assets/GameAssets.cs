using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace TheGreatSidegrade.Assets;

public class GameAssets {
    public static Texture2D GetTexture(string path, AssetRequestMode requestMode = AssetRequestMode.AsyncLoad) => TheGreatSidegrade.Mod.Assets.Request<Texture2D>(path, requestMode).Value;

    public static Asset<Texture2D> GetAsset(string path, AssetRequestMode requestMode = AssetRequestMode.AsyncLoad) => TheGreatSidegrade.Mod.Assets.Request<Texture2D>(path, requestMode);
}
