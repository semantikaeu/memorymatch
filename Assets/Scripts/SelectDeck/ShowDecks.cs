using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Assets.GameEngine.Data;
using Assets.GameEngine.Providers;
using Assets.Scripts.Controls;
using Assets.Scripts.Utils;

using UnityEngine;

public class ShowDecks : MonoBehaviour
{
    public GameObject LoadingObject;
    public GameObject SpawnObject;
    public GameObject SpawnEndObject;
    public Texture SelectedTexture;
    public Texture FrameObject;
    public Font DefaultFont;
    public GameObject DifficultyMenu;

    public int Spacing = 30;
    public int BigSize = 150;
    public int SmallSize = 55;

    public float BigScale = 1.0f;
    public float SmallScale = 1.0f;

    private const int LoadingState = 0;
    private const int ShowDeckState = 1;

    private List<DeckData> decks;
    private int state = LoadingState;
    private int nextState = ShowDeckState;
    private int selectedDeckId = 0;

    private List<GroupControl> deckControls = new List<GroupControl>();
    private Vector2 scrollViewVector;
    private Rect scrollViewRect;
    private Rect scrollViewContentRect;

    private int size = 14;
    private bool isMousePressed;
    private int mouseTimeout = 30;

    // Use this for initialization
    private void Start()
    {
        Spacing = Scale(Spacing);
        BigSize = Scale(BigSize);
        SmallSize = Scale(SmallSize);

        BigScale = BigSize / (float)Scale(120);
        SmallScale = SmallSize / (float)Scale(40);

        decks = CardFactory.GetAllOfflineDecks();

        StartCoroutine(LoadDecks());
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case LoadingState:
                var x = LoadingObject.transform.rotation.x;
                var y = LoadingObject.transform.rotation.y;
                var z = LoadingObject.transform.rotation.z;

                LoadingObject.transform.Rotate(x, y + 2, z + 1);
                break;

            case ShowDeckState:
                
                break;
        }
    }

    private void OnGUI()
    {
        Vector3 p = Camera.main.WorldToScreenPoint(SpawnObject.transform.position);
        p.y = 10;

        if (state == ShowDeckState)
        {
            int fullItemWidth = BigSize + Spacing;

            float scaledBigFrameOffsetX = Scale(14) * BigScale;
            float scaledBigFrameWidth = BigSize + (Scale(30) * BigScale);

            float scaledSmallFrameOffsetX = Scale(5) * SmallScale;
            scrollViewVector = GUI.BeginScrollView(scrollViewRect, scrollViewVector, scrollViewContentRect);

            p.x -= scaledBigFrameOffsetX - scaledSmallFrameOffsetX;

            for (int i = 0; i < size; ++i)
            {
                GUI.BeginGroup(CalculatePosition(p, p.y, scaledBigFrameWidth));
                deckControls[i].Render();
                GUI.EndGroup();

                p.x += fullItemWidth;
            }

            GUI.EndScrollView(true);
        }

        if (Input.GetMouseButtonDown(0) && !isMousePressed && mouseTimeout <= 0)
        {
            mouseTimeout = 30;

            isMousePressed = true;

            MouseDown();
        }
        else
        {
            isMousePressed = false;

            if (mouseTimeout > 0)
            {
                --mouseTimeout;
            }
        }
    }

    private Rect CalculatePosition(Vector3 p, float offsetY, float size)
    {
        return new Rect(p.x, offsetY, size, size);
    }

    private void MouseDown()
    {
        Vector2 position = Input.mousePosition;
        position.x += scrollViewVector.x;
        position.y = Screen.height - position.y + scrollViewVector.y;

        Vector3 p = Camera.main.WorldToScreenPoint(SpawnObject.transform.position);
        p.y = Screen.height - p.y + 10;

        Debug.Log(position);
        int fullItemWidth = BigSize + Spacing;
        for (int i = 0; i < size; ++i)
        {
            Rect imageRect = CalculatePosition(p, p.y, BigSize);
            imageRect.x -= 10;
            imageRect.y -= 10;
            imageRect.width += 20;
            imageRect.height += 20;
            if (imageRect.Contains(position))
            {
                Debug.Log("Hit item " + i + "!");

                ChangeDeckSelection(selectedDeckId, false);

                DataMessenger.SelectedDeck = decks[i];
                selectedDeckId = i;
                ChangeDeckSelection(selectedDeckId, true);
            }

            p.x += fullItemWidth;
        }
    }

    private void ChangeDeckSelection(int i, bool isSelected)
    {
        var group = deckControls[i];
        ImageView selected = group.Items.FirstOrDefault(s => s.Tag is string && ReferenceEquals(s.Tag, "selected")) as ImageView;
        if (selected != null)
        {
            selected.Scale = isSelected ? 0.3f : 0f;
            selected.MeasureSize();

            if (isSelected)
            {
                UpdateDifficultyAvailability();
            }
        }
    }

    private IEnumerator LoadDecks()
    {
        deckControls.Clear();
        scrollViewVector = Vector2.zero;

        Vector3 p = Camera.main.WorldToScreenPoint(SpawnObject.transform.position);
        p.y = Screen.height - p.y;
        
        Vector3 e = Camera.main.WorldToScreenPoint(SpawnEndObject.transform.position);
        e.y = Screen.height - e.y;

        BigSize = (int)(((e.y - p.y) * 0.8f) - Scale(14));
        BigScale = BigSize / (float)Scale(120);

        //Debug.Log(e.y + "-" + p.y + "-" + Scale(14) + "=" + BigSize + "  =>  BigScale = " + BigScale);

        SmallSize = BigSize / 3;
        SmallScale = SmallSize / (float)Scale(40);

        p.y = 10;

        float scaledBigFrameOffsetX = Scale(14) * BigScale;

        float scaledSmallFrameOffsetX = Scale(5) * SmallScale;
        float scaledSmallFrameOffsetY = Scale(5) * SmallScale;

        float firstImageOffsetY = p.y + (Scale(7) * BigScale);
        float secondImageOffsetY = p.y + SmallSize + (Scale(13 + 7) * BigScale);

        p.x -= scaledBigFrameOffsetX - scaledSmallFrameOffsetX;

        if (size > 0)
        {
            DataMessenger.SelectedDeck = decks[0];
        }

        size = decks.Count;
        for (int i = 0; i < size; ++i)
        {
            bool isInResources = decks[i].ImageLocation == "Resources";
            List<CardData> cards = new List<CardData>(decks[i].Cards).OrderBy(o => Guid.NewGuid()).ToList();

            string path = isInResources
                              ? "Decks/" + decks[i].ResourceName + "/" + Path.GetFileNameWithoutExtension(cards[0].Image)
                              : string.Empty;

            var groupControl = new GroupControl();
            var frame = new ImageViewWithFrame();

            yield return StartCoroutine(ImageViewWithFrame(cards[0], frame, BigSize, path, rightMargin: 30));
            groupControl.Items.Add(frame);

            path = isInResources
                       ? "Decks/" + decks[i].ResourceName + "/" + Path.GetFileNameWithoutExtension(cards[1].Image)
                       : string.Empty;
            float leftMargin = scaledBigFrameOffsetX - Scale(9) - scaledSmallFrameOffsetX;
            float topMargin = firstImageOffsetY - scaledSmallFrameOffsetY;

            frame = new ImageViewWithFrame();
            yield return StartCoroutine(ImageViewWithFrame(cards[1], frame, SmallSize, path, leftMargin, topMargin));
            groupControl.Items.Add(frame);

            path = isInResources
                       ? "Decks/" + decks[i].ResourceName + "/" + Path.GetFileNameWithoutExtension(cards[2].Image)
                       : string.Empty;
            topMargin = secondImageOffsetY - scaledSmallFrameOffsetY;

            frame = new ImageViewWithFrame();
            yield return StartCoroutine(ImageViewWithFrame(cards[2], frame, SmallSize, path, leftMargin, topMargin));
            groupControl.Items.Add(frame);

            leftMargin = scaledBigFrameOffsetX + Scale(8);
            topMargin = BigSize - Scale(10);

            GUIStyle style = new GUIStyle
                                 {
                                     fontSize = (int)(24 * Initialize.Scale),
                                     font = DefaultFont,
                                     clipping = TextClipping.Clip,
                                 };

            int textWidth = 200;

            TextView textView = new TextView();
            textView.Content = new GUIContent(FontColorUtils.Shadow(decks[i].Title));
            textView.Style = style;
            textView.Rect = new Rect(leftMargin + 2, topMargin + 2, Scale(textWidth), 100);
            groupControl.Items.Add(textView);

            textView = new TextView();
            textView.Content = new GUIContent(FontColorUtils.DeckTitle(decks[i].Title));
            textView.Style = style;
            textView.Rect = new Rect(leftMargin, topMargin, Scale(textWidth), 100);
            groupControl.Items.Add(textView);

            ImageView imageView = new ImageView();
            imageView.LeftMargin = BigSize - Scale(25);
            imageView.TopMargin = (int)scaledSmallFrameOffsetY + Scale(15);
            imageView.ImageTexture = SelectedTexture;
            imageView.ImageRect = new Rect(0, 0, SelectedTexture.width, SelectedTexture.height);
            imageView.Scale = i == 0 ? 0.3f : 0;
            imageView.Tag = "selected";
            imageView.MeasureSize();
            groupControl.Items.Add(imageView);

            groupControl.Tag = decks[i];
            deckControls.Add(groupControl);

            if (i == 0)
            {
                UpdateDifficultyAvailability();
            }
        }

        p = Camera.main.WorldToScreenPoint(SpawnObject.transform.position);
        p.y = Screen.height - p.y;

        int fullItemWidth = BigSize + Spacing;
        float scaledBigFrameHeight = BigSize + (Scale(27) * BigScale);
        float width = (fullItemWidth * size) + p.x + (Screen.width - e.x - Spacing);

        scrollViewRect = new Rect(0, p.y - 20, Screen.width, scaledBigFrameHeight + Scale(25));
        scrollViewContentRect = new Rect(0, 0, width, scaledBigFrameHeight);

        NextState();
    }

    private IEnumerator ImageViewWithFrame(
        CardData card,
        ImageViewWithFrame frame,
        int imageSize,
        string resourcePath,
        float leftMargin = 0,
        float topMargin = 0,
        float rightMargin = 0,
        float bottomMargin = 0)
    {
        Texture texture;

        if (!string.IsNullOrEmpty(resourcePath))
        {
            texture = Resources.Load<Texture>(resourcePath);
        }
        else
        {
            string path = WebCache.GetCachedPath(card);
            WWW www = new WWW(path);
            yield return www;

            texture = www.texture;
        }

        frame.BigSize = imageSize;
        frame.FrameTexture = FrameObject;
        frame.ImageTexture = texture;
        frame.LeftMargin = leftMargin;
        frame.TopMargin = topMargin;
        frame.RightMargin = rightMargin;
        frame.BottomMargin = bottomMargin;

        frame.MeasureSize();
    }

    private void NextState()
    {
        if (state == LoadingState)
        {
            LoadingObject.SetActive(false);
        }

        state = nextState;

        if (state == LoadingState)
        {
            LoadingObject.SetActive(true);
        }
    }

    private int Scale(int number)
    {
        return (int)(number * Initialize.Scale);
    }

    private void UpdateDifficultyAvailability()
    {
        var deck = decks[selectedDeckId];

        int maxDifficulty = 5;
        int cards = deck.Cards.Count;

        if (cards < 12)
        {
            maxDifficulty = 1;
        }
        else if (cards < 20)
        {
            maxDifficulty = 2;
        }
        else if (cards < 30)
        {
            maxDifficulty = 3;
        }
        else if (cards < 42)
        {
            maxDifficulty = 4;
        }

        if (DifficultyMenu != null)
        {
            DifficultyMenu.SendMessage("UpdateDifficulty", maxDifficulty);
        }
    }
}
