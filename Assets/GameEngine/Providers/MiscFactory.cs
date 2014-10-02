namespace Assets.GameEngine.Providers
{
    public static class MiscFactory
    {
        public static IHighscoreProvider GetHighscoreProvider()
        {
            return new HighscoreProvider();
        }

        public static IScoreCalculation GetScoreCalculation()
        {
            return new DailyMemoryScoreCalculation();
        }
    }
}
