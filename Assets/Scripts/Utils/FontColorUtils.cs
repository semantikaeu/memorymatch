namespace Assets.Scripts.Utils
{
    using UnityEngine;

    public class FontColorUtils
    {
        public static string Default(string text)
        {
            return string.Format("<color=#efdbb1>{0}</color>", text);
        }

        public static string DeckTitle(string text)
        {
            return string.Format("<color=#f8c015>{0}</color>", text);
        }

        public static string MainGameScore(string text)
        {
            return string.Format("<color=#ffb920>{0}</color>", text);
        }

        public static string BestScore(string text)
        {
            return string.Format("<color=#ffa007>{0}</color>", text);     
        }

        public static string BestScoreShadow(string text)
        {
            return string.Format("<color=#70472ecc>{0}</color>", text);
        }

        public static string Shadow(string text)
        {
            return string.Format("<color=#70472ecc>{0}</color>", text);            
        }

        public static string GetColoredText(string text, string color)
        {
            return string.Format("<color={1}>{0}</color>", text, color);
        }

        public static void DrawTextWithShadow(string text, string color, float width, float height, GUIStyle style, float shadowX = 2, float shadowY = 2)
        {
            GUI.Label(new Rect(shadowX, shadowY, width, height), Shadow(text), style);
            GUI.Label(new Rect(0, 0, width, height), GetColoredText(text, color), style);
        }

        public static void DrawTextWithShadow(string text, float width, float height, GUIStyle style, float shadowX = 2, float shadowY = 2)
        {
            GUI.Label(new Rect(shadowX, shadowY, width, height), Shadow(text), style);
            GUI.Label(new Rect(0, 0, width, height), Default(text), style);
        }

        public static void DrawText(string text, float width, float height, GUIStyle style)
        {
            GUI.Label(new Rect(0, 0, width, height), Default(text), style);
        }

        public static string DefaultFontColor
        {
            get { return "#f8c015"; }
        }

        public static string ShadowFontColor
        {
            get { return "#0000007f"; }
        }
    }
}
