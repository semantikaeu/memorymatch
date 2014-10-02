namespace Assets.GameEngine.Utils
{
    using System;

    using UnityEngine;

    public static class MathHelper
    {
        public const float FLOAT_TOLERANCE = 0.002f;
        public static Rect ScaleAndCrop(float width, float height, float targetWidth, float targetHeight, out float scale)
        {
            float scaleX = targetWidth / width;
            float scaleY = targetHeight / height;

            scale = Math.Max(scaleX, scaleY);

            // original equation: (targetWidth - (width * scale)) / scale
            float deltaWidth = (targetWidth / scale) - width;

            // original equation: (targetHeight - (height * scale)) / scale
            float deltaHeight = (targetHeight / scale) - height;

            float offsetX = -(int)deltaWidth >> 1;
            float offsetY = -(int)deltaHeight >> 1;

            return new Rect(offsetX, offsetY, width + deltaWidth, height + deltaHeight);
        }

        public static Rect ScaleAndCrop2(float width, float height, float targetWidth, float targetHeight, out float scale)
        {
            if (Math.Abs(width - targetWidth) > FLOAT_TOLERANCE)
            {
                height = (height * targetWidth) / width;
            }

            if (height < targetHeight)
            {
                width = (width * targetHeight) / height;
            }

            scale = targetHeight / height;

            float deltaHeight = targetHeight - height;
            float deltaWidth = targetWidth - width;
            float offsetX = deltaWidth / 2.0f;
            float offsetY = deltaHeight / 2.0f;

            return new Rect(offsetX, offsetY, targetWidth, targetHeight);
        }
    }
}
