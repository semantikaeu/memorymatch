namespace Assets.Scripts.Game
{
    using Assets.GameEngine;
    using Assets.GameEngine.Providers;
    using Assets.Scripts.Utils;

    using UnityEngine;

    public class GameOverScorePopup : MonoBehaviour
    {
        public GameObject ScorePosition;
        public GameObject HighScorePosition;

        [Header("Single player")]
        public GameObject SinglePlayerContainer;
        public GameObject SinglePlayerScore1;
        public GameObject SinglePlayerBestScore;

        [Header("Two players")]
        public GameObject TwoPlayerContainer;
        public GameObject TwoPlayerScore1;
        public GameObject TwoPlayerScore2;

        [Header("Additional settings")]
        public Font DefaultFont;
        public int Highscore;

        private int numberOfPlayers;

        private void Start()
        {
            MemoryGame.Current.GameCompleted();

            //MemoryGame.Current.Scores[0] = 777;
            //MemoryGame.Current.Scores[1] = 643;

            Awake();
        }

        private void Awake()
        {
            MemoryGame.Current.IsBusy = true;

            numberOfPlayers = MemoryGame.Current.Players;
            bool isSinglePlayer = numberOfPlayers == 1;

            SinglePlayerContainer.SetActive(isSinglePlayer);
            TwoPlayerContainer.SetActive(!isSinglePlayer);

            string gameMode = MemoryGame.Current.GetGameModeString();

            IHighscoreProvider highscoreProvider = MiscFactory.GetHighscoreProvider();
            Highscore = highscoreProvider.GetHighScore(gameMode);
            //Highscore = 777;
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.richText = true;
            style.wordWrap = true;
            style.fontSize = (int)(34 * Initialize.Scale);
            style.fontStyle = FontStyle.Bold;
            style.font = DefaultFont;
            style.padding = new RectOffset(10, 10, 10, 10);

            string scoreText;
            if (numberOfPlayers == 1)
            {
                scoreText = MemoryGame.Current.Scores[0].ToString();
                ShowScore(SinglePlayerScore1, ScorePosition.transform, style, scoreText);

                style.fontSize = (int)(16 * Initialize.Scale);

                scoreText = Highscore.ToString();
                ShowScore(SinglePlayerBestScore, HighScorePosition.transform, style, scoreText);
            }
            else
            {
                scoreText = MemoryGame.Current.Scores[0].ToString();
                ShowScore(TwoPlayerScore1, ScorePosition.transform, style, scoreText);

                scoreText = MemoryGame.Current.Scores[1].ToString();
                ShowScore(TwoPlayerScore2, ScorePosition.transform, style, scoreText);
            }
        }

        private void ShowScore(GameObject obj, Transform offsetX, GUIStyle style, string score)
        {
            Vector3 rawPosition = obj.transform.position;
            rawPosition.x = offsetX.position.x;

            Vector3 start = Camera.main.WorldToScreenPoint(rawPosition);
            start.y = Screen.height - start.y;

            float minWidth, maxWidth;
            string text = TextFormatting.AddSpacingBetweenCharacters(score);
            GUIContent content = new GUIContent(text);
            style.CalcMinMaxWidth(content, out minWidth, out maxWidth);
            float height = style.CalcHeight(content, maxWidth);
            height /= 2.0f;

            float halfWidth = maxWidth / 2.0f;

            GUI.Label(new Rect(start.x - halfWidth - 3, start.y + 2 - height, maxWidth, 120), FontColorUtils.Shadow(text), style);
            GUI.Label(new Rect(start.x - halfWidth, start.y - height, maxWidth, 120), FontColorUtils.MainGameScore(text), style);
        }
    }
}
