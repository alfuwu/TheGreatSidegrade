using Terraria.ModLoader;
using TheGreatSidegrade.Assets;
using TheGreatSidegrade.Common.Hooks;

namespace TheGreatSidegrade {
	public class TheGreatSidegrade : Mod {
		public static Mod Avalon;
        public static Mod Confection;
        public static TheGreatSidegrade Mod;
        public const string AssetPath = $"{nameof(TheGreatSidegrade)}/Assets/";

        public override void Load() {
            Mod = this;
            ModLoader.TryGetMod("Avalon", out Avalon);
            ModLoader.TryGetMod("Confection", out Confection);

            Hooks.RegisterHooks();
        }

        public override void Unload() {
            Hooks.UnregisterHooks();
        }

        public static bool IsContagion {
            get => Avalon != null && Avalon.TryFind("AvalonWorld", out ModSystem world) && (int) world.GetType().GetField("WorldEvil").GetValue(world) == 2;
            set {
                if (Avalon != null && Avalon.TryFind("AvalonWorld", out ModSystem world))
                    world.GetType().GetField("WorldEvil").SetValue(world, value ? 2 : 0);
            }
        }
    }
}
