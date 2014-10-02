using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Assets.GameEngine.Data;
using Assets.GameEngine.Providers;
using Assets.Scripts.Controls;

using UnityEngine;

public class ShowDecks2 : MonoBehaviour
{
    public GameObject LoadingObject;
    public GameObject SpawnObject;
    public GameObject SpawnEndObject;
    public Texture FrameObject;

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

    private List<Texture> textures = new List<Texture>();
    private Vector2 scrollViewVector;

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
        Vector3 e = Camera.main.WorldToScreenPoint(SpawnEndObject.transform.position);
        p.y = Screen.height - p.y;
        e.y = Screen.height - e.y;

        if (state == ShowDeckState)
        {
            int fullItemWidth = BigSize + Spacing;

            float scaledBigFrameOffsetX = Scale(14) * BigScale;
            float scaledBigFrameOffsetY = Scale(13) * BigScale;
            float scaledBigFrameWidth = BigSize + (Scale(30) * BigScale);
            float scaledBigFrameHeight = BigSize + (Scale(27) * BigScale);

            float scaledSmallFrameOffsetX = Scale(5) * SmallScale;
            float scaledSmallFrameOffsetY = Scale(5) * SmallScale;
            float scaledSmallFrameWidth = SmallSize + (Scale(10) * SmallScale);
            float scaledSmallFrameHeight = SmallSize + (Scale(9) * SmallScale);

            float width = (fullItemWidth * size) + p.x + (Screen.width - e.x - Spacing);
            scrollViewVector = GUI.BeginScrollView(new Rect(0, p.y - 20, Screen.width, scaledBigFrameHeight + Scale(25)), scrollViewVector, new Rect(0, 0, width, scaledBigFrameHeight));

            p.y = 10;

            float firstImageOffsetY = p.y + (Scale(7) * BigScale);
            float secondImageOffsetY = p.y + SmallSize + (Scale(13 + 7) * BigScale);

            p.x -= scaledBigFrameOffsetX - scaledSmallFrameOffsetX;

            int imageIndex = 0;
            for (int i = 0; i < size; ++i)
            {
                ScaleMode scaleMode = ScaleMode.ScaleAndCrop;

                Rect imageRect = CalculatePosition(p, p.y, scaledBigFrameWidth + 200);

                GUI.BeginGroup(imageRect);

                GUI.DrawTexture(new Rect(scaledBigFrameOffsetX, scaledBigFrameOffsetY, BigSize, BigSize), textures[imageIndex], scaleMode);
                GUI.DrawTexture(new Rect(0, 0, scaledBigFrameWidth, scaledBigFrameHeight), FrameObject, ScaleMode.StretchToFill);

                ++imageIndex;
                float x = scaledBigFrameOffsetX - Scale(9);
                GUI.DrawTexture(new Rect(x, firstImageOffsetY, SmallSize, SmallSize), textures[imageIndex], scaleMode);
                GUI.DrawTexture(new Rect(x - scaledSmallFrameOffsetX, firstImageOffsetY - scaledSmallFrameOffsetY, scaledSmallFrameWidth, scaledSmallFrameHeight), FrameObject, ScaleMode.StretchToFill);

                ++imageIndex;
                GUI.DrawTexture(new Rect(x, secondImageOffsetY, SmallSize, SmallSize), textures[imageIndex], scaleMode);
                GUI.DrawTexture(new Rect(x - scaledSmallFrameOffsetX, secondImageOffsetY - scaledSmallFrameOffsetY, scaledSmallFrameWidth, scaledSmallFrameHeight), FrameObject, ScaleMode.StretchToFill);
                ++imageIndex;

                GroupControl groupControl = new GroupControl();
                ImageViewWithFrame frame = new ImageViewWithFrame();
                frame.BigSize = BigSize;
                frame.FrameTexture = FrameObject;
                frame.ImageTexture = textures[imageIndex];
                frame.RightMargin = 30;

                frame.MeasureSize();
                //frame.Render();
                groupControl.Items.Add(frame);

                frame = new ImageViewWithFrame();
                frame.BigSize = SmallSize;
                frame.FrameTexture = FrameObject;
                frame.ImageTexture = textures[imageIndex];
                frame.LeftMargin = x - scaledSmallFrameOffsetX;
                frame.TopMargin = firstImageOffsetY - scaledSmallFrameOffsetY;

                frame.MeasureSize();
                //frame.Render();
                groupControl.Items.Add(frame);

                frame = new ImageViewWithFrame();
                frame.BigSize = SmallSize;
                frame.FrameTexture = FrameObject;
                frame.ImageTexture = textures[imageIndex];
                frame.LeftMargin = x - scaledSmallFrameOffsetX;
                frame.TopMargin = secondImageOffsetY - scaledSmallFrameOffsetY;

                frame.MeasureSize();
                //frame.Render();
                groupControl.Items.Add(frame);

                groupControl.Render();

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

                Application.LoadLevel("Game");
            }

            p.x += fullItemWidth;
        }
    }

    private IEnumerator LoadDecks()
    {
        for (int i = 0; i < size; ++i)
        {
            List<CardData> cards = new List<CardData>(decks[0].Cards).OrderBy(o => Guid.NewGuid()).ToList();

            string path = "Decks/" + decks[0].ResourceName + "/" + Path.GetFileNameWithoutExtension(cards[0].Image);
            Texture texture = Resources.Load<Texture>(path);
            textures.Add(texture);

            path = "Decks/" + decks[0].ResourceName + "/" + Path.GetFileNameWithoutExtension(cards[1].Image);
            texture = Resources.Load<Texture>(path);
            textures.Add(texture);

            path = "Decks/" + decks[0].ResourceName + "/" + Path.GetFileNameWithoutExtension(cards[2].Image);
            texture = Resources.Load<Texture>(path);
            textures.Add(texture);
        }

        NextState();
        yield break;
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
}
