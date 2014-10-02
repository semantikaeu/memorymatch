namespace Assets.Scripts.CreateSet
{
    using Assets.Scripts.Utils;

    using UnityEngine;

    public class TitleLabel : MonoBehaviour
    {
        public GameObject SpawnLocation;
        public Font DefaultFont;

        public string Title = "Create set";

        private void Start()
        {
            Title = TextFormatting.AddSpacingBetweenCharactersWithSmallSpace(Title, (int)(8 * Initialize.Scale));
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.richText = true;
            style.wordWrap = true;
            style.fontSize = (int)(32 * Initialize.Scale);
            style.fontStyle = FontStyle.Bold;
            style.font = DefaultFont;

            Vector3 p = Camera.main.WorldToScreenPoint(SpawnLocation.transform.position);
            p.y = Screen.height - p.y;

            GUI.BeginGroup(new Rect(p.x, p.y, 500, 50));
            FontColorUtils.DrawTextWithShadow(Title, 500, 50, style);
            GUI.EndGroup();
        }
    }
}
