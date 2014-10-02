namespace Assets.GameEngine.Data
{
    using System;

    /// <summary>
    /// The game rules.
    /// </summary>
    public class GameRules
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameRules"/> class.
        /// </summary>
        public GameRules()
        {
            Players = 1;
            CardsForPair = 2;
            ConcludePoints = 15;
            LucyPoints = 20;
            PairFoundPoints = 10;
            MissPoints = -1;
            MaxCardsOnField = 4;
            ShowCardInMiliSeconds = 750;
        }

        public int CardsForPair { get; set; }

        public string Difficulty { get; set; }

        public int MaxCardsOnField { get; set; }

        public int Players { get; set; }

        public bool IsTimeAttackMode { get; set; }

        public TimeSpan TimeAttackDuration { get; set; }

        public TimeSpan TimeAttackAddedDurationForFoundPair { get; set; }

        public bool IsFlashMode { get; set; }

        public TimeSpan FlashDuration { get; set; }

        public bool IsLimitedTriesMode { get; set; }

        public int LimitedTries { get; set; }

        public int AddTriesForFoundPair { get; set; }

        public int LucyPoints { get; set; }

        public int ConcludePoints { get; set; }

        public int PairFoundPoints { get; set; }

        public int MissPoints { get; set; }

        public int ShowCardInMiliSeconds { get; set; }

        public bool IsArcade { get; set; }
    }
}
