using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using System;
using Terraria;
using TheGreatSidegrade.Assets;

namespace TheGreatSidegrade.Common.Hooks {
    public class WorldList {
        public static void DrawSelf(ILContext il) {
            ILCursor c = new(il);
            // Move il cursor into positon
            if (!c.TryGotoNext(i => i.MatchLdsfld<WorldGen>(nameof(WorldGen.crimson))))
                return;
            if (!c.TryGotoNext(i => i.MatchLdloc(5)))
                return;
            if (!c.TryGotoPrev(i => i.MatchLdarg(1)))
                return;
            if (!c.TryGotoNext(i => i.MatchLdfld(out _)))
                return;
            c.Index++;
            c.Index += 0;
            c.EmitDelegate<Func<Texture2D, Texture2D>>((corruptTexture) => {
                return GreatlySidegradedWorld.worldEvil switch {
                    GreatlySidegradedWorld.WorldEvil.Fractured => GameAssets.FracturedProgressBar,
                    GreatlySidegradedWorld.WorldEvil.Nothing => GameAssets.NothingProgressBar,
                    GreatlySidegradedWorld.WorldEvil.Rotten => GameAssets.RottenProgressBar,
                    GreatlySidegradedWorld.WorldEvil.Spiral => GameAssets.SpiralProgressBar,
                    GreatlySidegradedWorld.WorldEvil.Starved=> GameAssets.StarvedProgressBar,
                    _ => corruptTexture,
                };
            });
        }
    }
}
