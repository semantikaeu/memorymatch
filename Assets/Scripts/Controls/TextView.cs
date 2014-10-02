namespace Assets.Scripts.Controls
{
    using UnityEngine;

    public class TextView : IUnityControl
    {
        public Rect Rect { get; set; }

        public object Tag { get; set; }

        public GUIContent Content { get; set; }

        public GUIStyle Style { get; set; }

        public void MeasureSize()
        {
        }

        public void Render()
        {
            GUI.Label(Rect, Content, Style);
        }

        public bool HitTest(Vector2 hitPosition)
        {
            return Rect.Contains(hitPosition);
        }
    }
}
