using Terraria.GameContent.UI.Elements;

namespace TheGreatSidegrade.Common.Hooks {
    public class Hooks {
        public static void RegisterHooks() {
            IL_UIGenProgressBar.DrawSelf += WorldList.DrawSelf;
        }

        public static void UnregisterHooks() {
            IL_UIGenProgressBar.DrawSelf -= WorldList.DrawSelf;
        }
    }
}
