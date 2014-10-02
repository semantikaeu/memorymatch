namespace Assets.Scripts.Controls
{
    using UnityEngine;

    public interface IUnityControl
    {
        Rect Rect { get; set; }

        object Tag { get; set; }

        void MeasureSize();

        void Render();

        bool HitTest(Vector2 hitPosition);
    }
}
