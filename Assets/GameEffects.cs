using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria;
using ReLogic.Content;

namespace TheGreatSidegrade.Assets;

public class GameEffects {
    public static Asset<Effect> ChronovoreDoubleTapShader;

    private static void RegisterMiscShader(Asset<Effect> shader, string name, string pass) => GameShaders.Misc[$"TheGreatSidegrade/{name}"] = new(shader, pass);

    public static Asset<Effect> GetEffect(string path, AssetRequestMode requestMode = AssetRequestMode.AsyncLoad) => TheGreatSidegrade.Mod.Assets.Request<Effect>(path, requestMode);

    public static void LoadShaders() {
        if (Main.dedServ)
            return;

        // temp, remove calamity shader and replace with actual effect shader when done learning how to shadfer
        ChronovoreDoubleTapShader = GetEffect("DoGPortalShader");
        RegisterMiscShader(ChronovoreDoubleTapShader, "ChronovoreDoubleTap", "ScreenPass");
    }
}
