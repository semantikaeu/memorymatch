using Assets.Scripts.Utils;

using UnityEngine;

public class AboutScript : MonoBehaviour
{
    public GUIText AboutText;

    // Use this for initialization
    private void Start()
    {
        float scale = Screen.height / 343.0f;
        Debug.Log(Screen.height);
        Debug.Log(scale);
        AboutText.fontSize = (int)(AboutText.fontSize * scale);
    }
}
