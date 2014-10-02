namespace Assets.Scripts.Controls
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    public class ItemsControl : IUnityControl
    {
        public int LeftMargin = 0;
        public int TopMargin = 0;
        public int RightMargin = 0;
        public int BottomMargin = 0;

        public ItemsControl()
        {
            Items = new List<IUnityControl>();
        }

        public Rect Rect { get; set; }

        public object Tag { get; set; }

        public List<IUnityControl> Items { get; set; }

        private bool isDirty = true;

        public virtual void MeasureSize()
        {
            Rect size = new Rect();

            foreach (var item in Items)
            {
                item.MeasureSize();

                size.x = Math.Min(size.x, item.Rect.x);
                size.width = Math.Max(size.width, item.Rect.width);
                size.y = 0;
                size.height += item.Rect.height + item.Rect.y;
            }

            size.x += LeftMargin;
            size.y += TopMargin;
            size.width += RightMargin;
            size.height += BottomMargin;

            Rect = size;
            isDirty = false;
        }

        public virtual void Render()
        {
            if (isDirty)
            {
                MeasureSize();
            }

            Rect position = Rect;
            foreach (var item in Items)
            {
                GUI.BeginGroup(position);
                item.Render();
                GUI.EndGroup();

                position.y += item.Rect.y + item.Rect.height;
            }
        }

        public bool HitTest(Vector2 hitPosition)
        {
            foreach (var item in Items)
            {
                if (item.HitTest(hitPosition))
                {
                    return true;
                }

                hitPosition.y += item.Rect.y + item.Rect.height;
            }

            return false;
        }

        public IUnityControl GetHitTestItem(Vector2 hitPosition)
        {
            foreach (var item in Items)
            {
                if (item.HitTest(hitPosition))
                {
                    return item;
                }

                hitPosition.y -= item.Rect.y + item.Rect.height;
            }

            return null;
        }
    }
}
