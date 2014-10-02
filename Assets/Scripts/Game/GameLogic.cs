using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Assets.GameEngine;
using Assets.GameEngine.Data;
using Assets.GameEngine.Providers;
using Assets.GameEngine.Utils;
using Assets.Scripts.Game;
using Assets.Scripts.Utils;

using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [Header("Game settings")]
    public GameObject CardObject;
    public GameObject SpawnLocation;
    public GameObject SpawnEndLocation;
    public GameObject TitleLocation;
    public GameObject ScoreDisplay;
    public Font DefaultFont;
    public string GameMode = "C";
    public int[] Score;
    public int CurrentPlayer;

    [Range(1, 4)]
    public int PlayersPlaying = 1;

    [Header("Additional settings")]
    public float DelayBeforeProcessingCurrentPairInSeconds = 0.45f;
    public float DelayBeforeHidingCardsInSeconds = 0.65f;

    [Header("Debug settings")]
    public int DebugQuestionId = 0;
    public bool IsDebuggingQuestionsEnabled = true;
    
    private MemoryGame game;

    private string scoreFormat = "PLAYER {1}: {0}";
    private string currentScoreText = string.Empty;
    private string currentScoreText2 = string.Empty;
    private string gameTitle;

    /// <summary>
    /// Gets or sets the numbers of cards left.
    /// </summary>
    public static int CardsLeft { get; set; }

    public static bool ShouldFlip(CardData newCard)
    {
        return !MemoryGame.Current.CanFlipCard(newCard);
    }

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    private void Start()
    {
        currentScoreText = TextFormatting.AddSpacingBetweenCharacters(string.Format(scoreFormat, 0, 1));
        currentScoreText2 = TextFormatting.AddSpacingBetweenCharacters(string.Format(scoreFormat, 0, 2));

        game = MemoryGame.Current;
        game.GameMode = GameMode;
        game.StartNewGame();

        PlayersPlaying = DataMessenger.NumberOfPlayers;
        game.Players = PlayersPlaying;

        Score = game.Scores;
        CurrentPlayer = game.CurrentPlayer;

        CardsLeft = 0;

        int numberOfCards = game.DifficultyToNumberOfCards(DataMessenger.Difficulty) / 2;
        var fieldSize = DeckLayoutProvider.GetGridSize(numberOfCards * 2);

        if (IsDebuggingQuestionsEnabled)
        {
            DebugQuestionPopup();
        }

        List<CardData> cards;

        if (DataMessenger.SelectedDeck != null)
        {
            cards = DataMessenger.SelectedDeck.Cards.OrderBy(a => Guid.NewGuid()).Take(numberOfCards).ToList();
            gameTitle = DataMessenger.SelectedDeck.ResourceName;
        }
        else
        {
            cards = CardFactory.GetRandomDeck(numberOfCards);
            gameTitle = "Fossils";
        }

        gameTitle = TextFormatting.AddSpacingBetweenCharactersWithSmallSpace(gameTitle, (int)(Initialize.Scale * 14));

        while (cards.Count < numberOfCards)
        {
            cards.AddRange(cards);
        }

        if (cards.Count > numberOfCards)
        {
            cards = cards.Take(numberOfCards).ToList();
        }

        cards.AddRange(cards);
        cards = cards.OrderBy(c => Guid.NewGuid()).ToList();

        var field = WorldToRect.GetRect(SpawnLocation, SpawnEndLocation);
        field.x = 20;
        field.width = Screen.width - field.x - field.x;
        var list = CalculateLayout.Calculate(
            (int)fieldSize.y,
            (int)fieldSize.x,
            field.width,
            field.height,
            field.x,
            field.y,
            10 * Initialize.Scale);
        float scale = 1f;
        float offsetY = 0f;

        switch (DataMessenger.Difficulty)
        {
            case 0:
                scale = 1f;
                break;
            case 1:
                scale = 0.8f;
                offsetY = 8f;
                break;
            case 2:
                scale = 0.6f;
                offsetY = 12f;
                break;
            case 3:
                scale = 0.5f;
                offsetY = 14f;
                break;
            case 4:
                scale = 0.4f;
                offsetY = 20f;
                break;
        }

        if (CardObject != null && SpawnLocation != null)
        {
            int i = 0;
            for (int y = 0; y < fieldSize.y; ++y)
            {
                for (int x = 0; x < fieldSize.x; ++x)
                {
                    ++CardsLeft;
                    var card = (GameObject)Instantiate(CardObject);
                    card.SendMessage("SetCard", cards[i]);

                    Vector3 newPosition = Camera.main.ScreenToWorldPoint(
                        new Vector3(list[i].x + (list[i].width / 2.0f), list[i].y + (list[i].height / 3.0f) - offsetY));
                    newPosition.z = -2;

                    GameObject obj = new GameObject();
                    obj.transform.position = newPosition;
                    obj.transform.parent = SpawnLocation.transform;
                    obj.transform.localScale = new Vector3(scale, scale, 1);

                    card.transform.parent = obj.transform;
                    card.transform.localPosition = new Vector3();

                    ++i;
                }
            }
        }
    }

    private void DebugQuestionPopup()
    {
        var popups = GameObject.Find("Popups");

        var cards = CardFactory.GetAllOfflineDecks()[0].Cards;
        if (popups != null)
        {
            DataMessenger.LastQuestion = cards[DebugQuestionId].Question;

            var animator = popups.GetComponent<Animator>();
            animator.SetBool("IsQuestionVisible", true);
        }
    }

    private void OnGUI()
    {
        if (game == null)
        {
            return;
        }

        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.wordWrap = true;
        style.fontSize = (int)(18 * Initialize.Scale);
        style.fontStyle = FontStyle.Bold;
        style.font = DefaultFont;

        //GUIContent content = new GUIContent(currentScoreText);

        //float min;
        //float max;
        //style.CalcMinMaxWidth(content, out min, out max);
        //float height = style.CalcHeight(content, max);

        //max += 50;
        //GUI.BeginGroup(new Rect(Screen.width - max - 20, 20, max + 2, 100));
        //FontColorUtils.DrawTextWithShadow(currentScoreText, max, height, style);
        //GUI.EndGroup();

        ShowScore(currentScoreText, 0, style);

        if (PlayersPlaying > 1)
        {
            ShowScore(currentScoreText2, 1, style);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.LoadLevel("Home");
        }

        //style = new GUIStyle();
        //style.richText = true;
        //style.fontSize = (int)(32 * Initialize.Scale);
        //style.fontStyle = FontStyle.Bold;
        //style.font = DefaultFont;

        //Vector3 p = Camera.main.WorldToScreenPoint(TitleLocation.transform.position);
        //p.y = Screen.height - p.y;

        //content = new GUIContent(gameTitle);
        //height = style.CalcHeight(content, max);
        //max = (Screen.width - max - 40) - p.x;

        //GUI.BeginGroup(new Rect(p.x, p.y, max, height + 22));
        //FontColorUtils.DrawTextWithShadow(gameTitle, max, height, style);
        //GUI.EndGroup();
    }

    private void ShowScore(string scoreText, int player, GUIStyle style)
    {
        Vector3 p = Camera.main.WorldToScreenPoint(ScoreDisplay.transform.position);
        p.y = Screen.height - p.y;

        GUIContent content = new GUIContent(scoreText);

        float min;
        float max;
        style.CalcMinMaxWidth(content, out min, out max);
        float height = style.CalcHeight(content, max);
        max += 50;

        if (PlayersPlaying > 1)
        {
            if (player == 0)
            {
                p.x -= max;
            }
            else
            {
                p.x += max * 0.15f;
            }
        }
        else
        {
            p.x -= max / 2.0f;
        }

        GUI.BeginGroup(new Rect(p.x, p.y - (height / 2.0f), max, 100));

        if (player == CurrentPlayer)
        {
            // #FFAD14
            FontColorUtils.DrawTextWithShadow(scoreText, "#ffad14", max, height, style, 6, 6);
        }
        else
        {
            FontColorUtils.DrawText(scoreText, max, height, style);
        }

        GUI.EndGroup();
    }

    /// <summary>
    /// Process questions answer.
    /// </summary>
    /// <param name="isCorrect">Is answer correct.</param>
    private void QuestionIsAnswered(bool isCorrect)
    {
        FlipOrRemoveCard(game.FirstCard, !isCorrect);
        FlipOrRemoveCard(game.LastCard, !isCorrect);

        game.QuestionAnswer(isCorrect);

        // TODO: This should not be anymore necessery.
        //game.ResetCurrentTurn();

        UpdateScores();

        game.IsBusy = false;

        if (isCorrect)
        {
            StartCoroutine(CompletePairFound());
        }
    }

    private IEnumerator FlipCard(CardData data)
    {
        var state = game.SelectCard(data);
        if (state == MemoryGameState.Play || state == MemoryGameState.Busy)
        {
            yield break;
        }

        game.IsBusy = true;

        yield return new WaitForSeconds(0.45f);

        bool coverCard = false;
        if (state == MemoryGameState.Question)
        {
            var popups = GameObject.Find("Popups");
            if (popups != null)
            {
                DataMessenger.LastQuestion = game.Question;
                DataMessenger.LastQuestionImageTexture = data.Texture;

                var animator = popups.GetComponent<Animator>();
                animator.SetBool("IsQuestionVisible", true);
            }
        }
        else
        {
            if (state != MemoryGameState.PairFound)
            {
                coverCard = true;
            }

            yield return new WaitForSeconds(DelayBeforeHidingCardsInSeconds);

            FlipOrRemoveCard(game.FirstCard, coverCard);
            FlipOrRemoveCard(game.LastCard, coverCard);

            game.ResetCurrentTurn();

            UpdateScores();

            yield return new WaitForSeconds(0.15f);

            if (state == MemoryGameState.PairFound)
            {
                yield return StartCoroutine(CompletePairFound());
            }

            game.IsBusy = false;
        }
    }

    private IEnumerator CompletePairFound()
    {
        Debug.Log("Complete pair found...");

        CardsLeft -= 2;
        Debug.Log(CardsLeft);

        if (CardsLeft <= 0)
        {
            Debug.Log("Start new");

            game.GameCompleted();

            GameObject gObject = GameObject.Find("BackgroundMusic");
            yield return StartCoroutine(MusicUtils.ToggleBackgroundMusic(gObject, true));

            var sound = GetComponent<AudioSource>();
            if (sound != null)
            {
                sound.Play();
            }

            var popups = GameObject.Find("Popups");
            if (popups != null)
            {
                var animator = popups.GetComponent<Animator>();
                animator.SetBool("ShowGameOver", true);
            }

            yield return new WaitForSeconds(4.25f);

            yield return StartCoroutine(MusicUtils.ToggleBackgroundMusic(gObject, false));
        }

        game.IsBusy = false;
    }

    private void FlipOrRemoveCard(CardData data, bool flip)
    {
        var animator = data.Instance.GetComponent<Animator>();
        animator.SetBool("PairFound", !flip);
        animator.SetBool("IsCovering", flip);
        animator.SetBool("IsUncovering", false);
    }

    private void UpdateScores(bool showScoreAnimation = true)
    {
        CurrentPlayer = game.CurrentPlayer;

        Score[game.CurrentPlayer] = game.Scores[game.CurrentPlayer];
        currentScoreText = TextFormatting.AddSpacingBetweenCharacters(string.Format(scoreFormat, Score[0], 1));

        if (PlayersPlaying > 1)
        {
            currentScoreText2 = TextFormatting.AddSpacingBetweenCharacters(string.Format(scoreFormat, Score[1], 2));
        }

        if (showScoreAnimation)
        {
            var scorePopup = GameObject.Find("Score");
            if (scorePopup != null)
            {
                scorePopup.SendMessage("ShowScore");
            }
        }
    }
}
