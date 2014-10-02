namespace Assets.GameEngine.Providers
{
    using System;
    using System.IO;

    using UnityEngine;

    public class HighscoreProvider : IHighscoreProvider
    {
        public void SaveHighScore(int score, string gameMode)
        {
#if !UNITY_WP8 && !UNITY_WINRT
            try
            {
                File.WriteAllText(Application.persistentDataPath + "/.score-" + gameMode, score.ToString());
            }
            catch (Exception ex)
            {
                Debug.Log("Serialization failed: " + ex.Message);
            }
#endif
        }

        public int GetHighScore(string gameMode)
        {
#if !UNITY_WP8 && !UNITY_WINRT
            try
            {
                string text = File.ReadAllText(Application.persistentDataPath + "/.score-" + gameMode);
                return int.Parse(text);
            }
            catch (Exception ex)
            {
                Debug.Log("Serialization failed: " + ex.Message);
            }
#endif

            return -1;
        }
    }
}
