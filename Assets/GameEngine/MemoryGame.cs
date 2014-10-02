namespace Assets.GameEngine
{
    using System.Collections.Generic;
    using System.Globalization;

    using Assets.GameEngine.Data;
    using Assets.GameEngine.Providers;
    using Assets.Scripts.Utils;

    /// <summary>
    /// This is main class for memory logic.
    /// </summary>
    public class MemoryGame : ObservableObject
    {
        private static MemoryGame currentGame = new MemoryGame();

        private readonly List<CardData> selectedCards = new List<CardData>();

        private bool isBusy;
        private CardData firstCard;
        private CardData lastCard;
        private string gameMode;
        private QuestionData question;
        private int currentPlayer;
        private int players;
        private MemoryGameState gameState;
        private IScoreCalculation scoreCalculation;
        private IHighscoreProvider highscoreProvider;
        private int scoreChanged;

        /// <summary>
        /// Initializes static members of the <see cref="MemoryGame"/> class.
        /// </summary>
        static MemoryGame()
        {
            Current = new MemoryGame();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryGame"/> class.
        /// </summary>
        public MemoryGame()
            : this(MiscFactory.GetScoreCalculation(), MiscFactory.GetHighscoreProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryGame" /> class.
        /// </summary>
        /// <param name="scoreCalculation">Score calculation provider.</param>
        /// <param name="highscoreProvider">The high score provider.</param>
        public MemoryGame(IScoreCalculation scoreCalculation, IHighscoreProvider highscoreProvider)
        {
            this.scoreCalculation = scoreCalculation;
            this.highscoreProvider = highscoreProvider;

            StartNewGame();
        }

        /// <summary>
        /// Gets or sets current game instance.
        /// </summary>
        public static MemoryGame Current
        {
            get { return currentGame; }
            protected set { currentGame = value; }
        }

        public IScoreCalculation ScoreCalculation
        {
            get { return scoreCalculation; }
            set { scoreCalculation = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether game logic is busy.
        /// </summary>
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value, "IsBusy"); }
        }

        /// <summary>
        /// Gets or sets first selected card.
        /// </summary>
        public CardData FirstCard
        {
            get { return firstCard; }
            set { SetProperty(ref firstCard, value, "FirstCard"); }
        }

        /// <summary>
        /// Gets or sets second selected card.
        /// </summary>
        public CardData LastCard
        {
            get { return lastCard; }
            set { SetProperty(ref lastCard, value, "LastCard"); }
        }

        /// <summary>
        /// Gets or sets game rules.
        /// </summary>
        public GameRules Rules { get; set; }

        public string GameMode
        {
            get { return gameMode; }
            set { SetProperty(ref gameMode, value, "GameMode"); }
        }

        public QuestionData Question
        {
            get { return question; }
            set { SetProperty(ref question, value, "Question"); }
        }

        public int[] Scores { get; private set; }

        public int CurrentPlayer
        {
            get { return currentPlayer; }
            set { SetProperty(ref currentPlayer, value, "CurrentPlayer"); }
        }

        public int Players
        {
            get { return players; }
            set { SetProperty(ref players, value, "Players"); }
        }

        public MemoryGameState GameState
        {
            get { return gameState; }
            set { SetProperty(ref gameState, value, "GameState"); }
        }

        public int ScoreChanged
        {
            get { return scoreChanged; }
            set { SetProperty(ref scoreChanged, value, "ScoreChanged"); }
        }

        public void StartNewGame()
        {
            Rules = new GameRules();

            ScoreChanged = 0;
            Scores = new[] { 0, 0, 0, 0 };
            FirstCard = LastCard = null;
            selectedCards.Clear();
            IsBusy = false;

            GameState = MemoryGameState.Play;
        }

        /// <summary>
        /// Determines whether this card can flip.
        /// </summary>
        /// <param name="data">Selected card.</param>
        /// <returns>Returns if current card can flip and game logic is not busy.</returns>
        public bool CanFlipCard(CardData data)
        {
            if (GameState == MemoryGameState.Question || selectedCards.Count >= 2 || IsBusy)
            {
                return false;
            }

            return selectedCards.Count <= 0 || selectedCards[0].Instance != data.Instance;
        }

        public MemoryGameState SelectCard(CardData data)
        {
            if (!CanFlipCard(data))
            {
                return MemoryGameState.Play;
            }

            selectedCards.Add(data);

            if (FirstCard == null)
            {
                FirstCard = data;
                return MemoryGameState.Play;
            }

            LastCard = data;

            var state = MemoryGameState.WrongPair;
            if (IsPairFound(FirstCard, LastCard))
            {
                if (GameMode == "Q" && data.Question != null && data.Question.ContainsQuestion)
                {
                    state = MemoryGameState.Question;
                    Question = data.Question;
                }
                else
                {
                    state = MemoryGameState.PairFound;
                }
            }

            GameState = state;

            // State question requires one more step before scoring.
            if (state != MemoryGameState.Question)
            {
                int score = scoreCalculation.Calculate(GameState, gameMode, Rules, selectedCards, Scores[currentPlayer]);
                AddScoreToCurrentPlayer(score);

                if (state != MemoryGameState.PairFound && state != MemoryGameState.CorrectAnswer)
                {
                    NextPlayer();
                }
            }

            return state;
        }

        public void NextPlayer()
        {
            CurrentPlayer = (CurrentPlayer + 1) % Players;
        }

        public void QuestionAnswer(bool correctAnswer)
        {
            GameState = correctAnswer ? MemoryGameState.CorrectAnswer : MemoryGameState.WrongAnswer;

            int score = scoreCalculation.Calculate(GameState, gameMode, Rules, selectedCards, Scores[currentPlayer]);
            AddScoreToCurrentPlayer(score);

            ResetCurrentTurn();

            if (!correctAnswer)
            {
                NextPlayer();
            }
        }

        public void AddScoreToCurrentPlayer(int score)
        {
            ScoreChanged = score;

            Scores[currentPlayer] += score;
            OnPropertyChanged("Scores");
        }

        public void ResetCurrentTurn()
        {
            ScoreChanged = 0;
            selectedCards.Clear();
            FirstCard = null;
            LastCard = null;

            GameState = MemoryGameState.Play;
        }

        public int DifficultyToNumberOfCards(int difficulty)
        {
            switch (difficulty)
            {
                case 0:
                    return 12;
                case 1:
                    return 24;
                case 2:
                    return 40;
                case 3:
                    return 60;
                case 4:
                    return 84;
            }

            return 0;
        }

        public void GameCompleted()
        {
            string mode = GetGameModeString();
            int bestScore = highscoreProvider.GetHighScore(mode);

            foreach (var score in Scores)
            {
                if (score > bestScore)
                {
                    bestScore = score;
                    highscoreProvider.SaveHighScore(score, mode);
                }
            }
        }

        public string GetGameModeString()
        {
            string deckId = "x";
            if (DataMessenger.SelectedDeck != null)
            {
                deckId = DataMessenger.SelectedDeck.Id.ToString(CultureInfo.InvariantCulture);
            }

            return deckId + "-" + DataMessenger.Difficulty;
        }

        private bool IsPairFound(CardData card1, CardData card2)
        {
            return card1.Id == card2.Id;
        }
    }
}
