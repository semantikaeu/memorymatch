namespace Assets.Scripts.Controls
{
    using UnityEngine;

    public abstract class BaseControl
    {
        public int LeftMargin = 0;
        public int TopMargin = 0;
        public int RightMargin = 0;
        public int BottomMargin = 0;

        public Rect LayoutSize { get; set; }

        public Rect RenderSize { get; set; }

        public bool IsHitTestVisible { get; set; }

        public object Tag { get; set; }

        public abstract void MeasureSize();

        public abstract void Render();

        public bool HitTest(Vector2 hitPosition)
        {
            return IsHitTestVisible && LayoutSize.Contains(hitPosition);
        }
    }
}
