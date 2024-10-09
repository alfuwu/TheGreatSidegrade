using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;
using Terraria;
using ReLogic.Content;
using Terraria.Graphics.Effects;

namespace TheGreatSidegrade.Assets;

public static class GameEffects {
    private static Asset<Effect> ChronovoreDoubleTapShader;
    private static Asset<Effect> DreamingColossusShader;
    private static Asset<Effect> OblivionShader;
    private static Asset<Effect> VeilkeeperRealityPhaseShader;

    private static void RegisterShader(Asset<Effect> shader, string name, string pass) => GameShaders.Misc[$"{nameof(TheGreatSidegrade)}/{name}"] = new(shader, pass);
    private static void RegisterScreenShader(Asset<Effect> shader, string name, string pass, EffectPriority priority = EffectPriority.Medium) => Filters.Scene[$"{nameof(TheGreatSidegrade)}/{name}"] = new(new(shader, pass), priority);

    public static Asset<Effect> GetEffect(string path, AssetRequestMode requestMode = AssetRequestMode.AsyncLoad) => TheGreatSidegrade.Mod.Assets.Request<Effect>($"Assets/Effects/{path}", requestMode);

    public static void LoadShaders() {
        if (Main.dedServ)
            return;

        ChronovoreDoubleTapShader = GetEffect("DoubleTap");
        RegisterScreenShader(ChronovoreDoubleTapShader, "ChronovoreDoubleTap/BluePass", "BluePass");
        RegisterScreenShader(ChronovoreDoubleTapShader, "ChronovoreDoubleTap/RipplePass", "ScreenPass");
        //DreamingColossusShader = GetEffect("DreamingColossus");
        //RegisterShader(DreamingColossusShader, "DreamingColossus", "ScreenPass");
        OblivionShader = GetEffect("Oblivion");
        RegisterShader(OblivionShader, "Oblivion", "ScreenPass");
        //VeilkeeperRealityPhaseShader = GetEffect("Veilkeeper");
        //RegisterShader(VeilkeeperRealityPhaseShader, "Veilkeeper", "ScreenPass");
    }
}
