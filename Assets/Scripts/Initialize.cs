using System;

using Assets.Scripts.Utils;

using UnityEngine;

public class Initialize : MonoBehaviour
{
    public GameObject WideUi;
    public GameObject SmallUi;
    public Font DefaultFont;
    public GUIText AboutText;
    
    public GameObject AboutTextStart;
    public GameObject AboutTextEnd;
    
    private static float scale;
    private static float scaleHeight;

    public static float Scale
    {
        get
        {
            if (scale <= 0.01)
            {
                UpdateScale();
            }

            return scale;
        }
        set { scale = value; }
    }

    public static float ScaleHeight
    {
        get
        {
            if (scaleHeight <= 0.1)
            {
                UpdateScale();
            }

            return scaleHeight;
        }
        set { scaleHeight = value; }
    }

    public static string FontColor { get { return ""; } }

    public static string ShadowColor { get { return ""; } }

    public static string QuestionsFontColor { get { return "#503a28"; } }

    public static bool IsWideScreen
    {
        get { return Screen.width / (float)Screen.height > 4.0f / 3.0f; }
    }

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    private void Start()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        //if (IsWideScreen)
        //{
        //    WideUi.SetActive(true);
        //    SmallUi.SetActive(false);
        //}
    }

    private void OnGUI()
    {
        //float scale = Screen.height / 343.0f;

        //GUIStyle style = new GUIStyle();
        //style.richText = true;
        //style.wordWrap = true;
        //style.fontStyle = FontStyle.Bold;
        //style.font = DefaultFont;
        
        //var rect = WorldToRect.GetRect(AboutTextStart, AboutTextEnd);

        //GUI.BeginGroup(rect);


        //GUI.BeginGroup(new Rect(20, 10, 300, 100));
        //GUI.Label(new Rect(2, 2, 300, 50), "<color=#0000007f>Memory Match</color>", style);
        //GUI.Label(new Rect(0, 0, 300, 50), "<color=#f8c015>Memory Match</color>", style);

        //style.fontSize = (int)(18 * Scale);
        //GUI.Label(new Rect(2, 30 * Scale, 300, 50), "<color=#0000007f>Natural History Edition</color>", style);
        //GUI.Label(new Rect(0, 28 * Scale, 300, 50), "<color=#f8c015>Natural History Edition</color>", style);
        //GUI.EndGroup();
    }

    private static void UpdateScale()
    {
        float x = Screen.width;
        float y = Screen.height;
        scale = x / 676.0f;
        scaleHeight = y / 326.0f;
    }
}
