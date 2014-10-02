namespace Assets.Scripts.Controls
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public class GroupControl : IUnityControl
    {
        public int LeftMargin = 0;
        public int TopMargin = 0;
        public int RightMargin = 0;
        public int BottomMargin = 0;

        public int Width = 120;
        public int Height = 120;

        public Vector2 Offset = Vector2.zero;

        public bool IsDirty = true;

        public GroupControl()
        {
            Items = new List<IUnityControl>();
        }

        public Rect Rect { get; set; }

        public object Tag { get; set; }

        public List<IUnityControl> Items { get; set; }

        public void MeasureSize()
        {
        }

        public void Render()
        {
            foreach (var item in Items)
            {
                item.Render();
            }
        }

        public bool HitTest(Vector2 hitPosition)
        {
            return Items.Any(i => i.HitTest(hitPosition));
        }
    }
}
