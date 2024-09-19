using Terraria.ModLoader;
using TheGreatSidegrade.Common.Hooks;

namespace TheGreatSidegrade {
	public class TheGreatSidegrade : Mod {
		public static Mod ExxoAvalonOrigins;
        public static TheGreatSidegrade Mod;
        public const string AssetPath = $"{nameof(TheGreatSidegrade)}/Assets/";

        public override void Load() {
            Mod = this;
            ModLoader.TryGetMod("ExxoAvalonOrigins", out ExxoAvalonOrigins);

            Hooks.RegisterHooks();
        }

        public override void Unload() {
            Hooks.UnregisterHooks();
        }
    }
}
