namespace Assets.GameEngine.Providers
{
    public interface IHighscoreProvider
    {
        void SaveHighScore(int score, string gameMode);

        int GetHighScore(string gameMode);
    }
}
