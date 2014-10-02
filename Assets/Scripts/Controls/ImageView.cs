namespace Assets.Scripts.Controls
{
    using UnityEngine;

    public class ImageView : IUnityControl
    {
        public int LeftMargin = 0;
        public int TopMargin = 0;
        public int RightMargin = 0;
        public int BottomMargin = 0;

        public int Width = 120;
        public int Height = 120;
        public Vector3 Position;
        public float Scale = 1.0f;
        public Vector2 Offset = Vector2.zero;

        public Texture ImageTexture;
        public Rect ImageRect;
        public ScaleMode ImageScaleMode = ScaleMode.ScaleAndCrop;

        public bool IsDirty = true;

        public ImageView() { }

        public ImageView(ImageView imageView)
        {
            LeftMargin = imageView.LeftMargin;
            TopMargin = imageView.TopMargin;
            RightMargin = imageView.RightMargin;
            BottomMargin = imageView.BottomMargin;

            Width = imageView.Width;
            Height = imageView.Height;
            Position = imageView.Position;
            Scale = imageView.Scale;
            Offset = imageView.Offset;

            ImageTexture = imageView.ImageTexture;
            ImageRect = imageView.ImageRect;
            ImageScaleMode = imageView.ImageScaleMode;
            IsDirty = imageView.IsDirty;

            Rect = imageView.Rect;
        }

        public Rect Rect { get; set; }

        public object Tag { get; set; }

        public void MeasureSize()
        {
            int scaledWidth = ScaleValue(Width);
            int scaledHeight = ScaleValue(Height);
            int fullItemWidth = ScaleValue(LeftMargin + RightMargin) + scaledWidth;
            int fullItemHeight = ScaleValue(TopMargin + BottomMargin) + scaledHeight;

            ImageRect = new Rect(Position.x + LeftMargin + ScaleValue(Offset.x), Position.y + TopMargin + ScaleValue(Offset.y), scaledWidth, scaledHeight);
            Rect = new Rect(Position.x + ScaleValue(Offset.x), Position.y + ScaleValue(Offset.y), fullItemWidth, fullItemHeight);

            IsDirty = false;
        }

        public void Render()
        {
            if (IsDirty)
            {
                MeasureSize();
            }

            GUI.DrawTexture(ImageRect, ImageTexture, ImageScaleMode);
        }

        public bool HitTest(Vector2 hitPosition)
        {
            return Rect.Contains(hitPosition);
        }

        private int ScaleValue(int number)
        {
            return (int)(number * Initialize.Scale * Scale);
        }

        private int ScaleValue(float number)
        {
            return (int)(number * Initialize.Scale * Scale);
        }
    }
}
