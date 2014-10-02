namespace Assets.Scripts.Controls
{
    using UnityEngine;

    public class ImageViewWithFrame : IUnityControl
    {
        public float LeftMargin = 0;
        public float TopMargin = 0;
        public float RightMargin = 0;
        public float BottomMargin = 0;

        public int Spacing = 30;
        public int BigSize = 120;

        public Texture ImageTexture;
        public Rect ImageRect;
        public ScaleMode ImageScaleMode = ScaleMode.ScaleAndCrop;

        public Texture FrameTexture;
        public Rect FrameRect;
        public ScaleMode FrameScaleMode = ScaleMode.StretchToFill;
        
        public ImageView ImageView;
        public ImageView FrameView;

        public bool IsDirty = true;

        public Rect Rect { get; set; }

        public object Tag { get; set; }

        public void MeasureSize()
        {
            var bigScale = BigSize / (float)Scale(120);

            float scaledBigFrameOffsetX = Scale(13) * bigScale;
            float scaledBigFrameOffsetY = Scale(12) * bigScale;
            float scaledBigFrameWidth = BigSize + (Scale(25) * bigScale);
            float scaledBigFrameHeight = BigSize + (Scale(25) * bigScale);

            float scaledRightMargin = Scale(RightMargin);
            float scaledBottomMargin = Scale(BottomMargin);

            ImageRect = new Rect(scaledBigFrameOffsetX, scaledBigFrameOffsetY, BigSize, BigSize);
            FrameRect = new Rect(0, 0, scaledBigFrameWidth, scaledBigFrameHeight);

            float fullItemWidth = scaledBigFrameWidth + scaledRightMargin;
            float fullHeight = scaledBigFrameHeight + scaledBottomMargin;
            Rect = new Rect(LeftMargin, TopMargin, fullItemWidth, fullHeight);

            IsDirty = false;
        }

        public void Render()
        {
            if (IsDirty)
            {
                MeasureSize();
            }

            // This is used for moving both images.
            GUI.BeginGroup(Rect);

            GUI.DrawTexture(ImageRect, ImageTexture, ImageScaleMode);
            GUI.DrawTexture(FrameRect, FrameTexture, FrameScaleMode);

            GUI.EndGroup();

            //GUI.TextArea(Rect, "a");
        }

        public bool HitTest(Vector2 hitPosition)
        {
            return Rect.Contains(hitPosition);
        }

        private int Scale(int number)
        {
            return (int)(number * Initialize.Scale);
        }

        private int Scale(float number)
        {
            return (int)(number * Initialize.Scale);
        }
    }
}
