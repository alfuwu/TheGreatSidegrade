using Microsoft.Xna.Framework.Graphics;

namespace TheGreatSidegrade.Assets {
    public class GameAssets {
        public static Texture2D FracturedProgressBar = GetTexture("Assets/Textures/UI/WorldGen/Outer_Fractured");
        public static Texture2D NothingProgressBar = GetTexture("Assets/Textures/UI/WorldGen/Outer_Nothing");
        public static Texture2D RottenProgressBar = GetTexture("Assets/Textures/UI/WorldGen/Outer_Rotten");
        public static Texture2D SpiralProgressBar = GetTexture("Assets/Textures/UI/WorldGen/Outer_Spiral");
        public static Texture2D StarvedProgressBar = GetTexture("Assets/Textures/UI/WorldGen/Outer_Starved");

        public static Texture2D GetTexture(string path) {
            return TheGreatSidegrade.Mod.Assets.Request<Texture2D>(path).Value;
        }
    }
}
