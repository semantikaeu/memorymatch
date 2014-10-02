namespace Assets.Scripts.Utils
{
    using System.Collections;

    using Assets.GameEngine.Utils;
#if UNITY_WINRT && !UNITY_EDITOR
    using Path = System.IO.Path;
    using File = UnityEngine.Windows.File;
#else
    using System.IO;
#endif

    using Assets.GameEngine.Data;

    using UnityEngine;

    public class WebCache
    {
        private static string cachedImageNameFormat = "cached_{0}_{1}.jpg";

        public static IEnumerator CacheAndSet(SpriteRenderer renderer, CardData data)
        {
            var url = data.Image;

            if (string.IsNullOrEmpty(data.Title))
            {
                data.Title = "default";
            }

            bool saveToFile;
            string imageName = string.Format(cachedImageNameFormat, data.Title, data.Id);
            string fileName = Application.persistentDataPath + "/" + Path.GetFileName(imageName);
            if (File.Exists(fileName))
            {
                saveToFile = false;
                url = "file://" + fileName;
            }
            else
            {
                saveToFile = true;
                url = AzureMuseumImageFormatter.GetImageUri(url, 277, 274).ToString();
            }

            var www = new WWW(url);

            yield return www;

            if (saveToFile)
            {
                fileName = Application.persistentDataPath + "/" + Path.GetFileName(imageName);
                File.WriteAllBytes(fileName, www.bytes);
            }

            if (renderer != null)
            {
                var sprite = Sprite.Create(
                    www.texture,
                    new Rect(0, 0, 277, 274),
                    new Vector2(0.5f, 0.5f));

                renderer.sprite = sprite;
            }
        }

        public static IEnumerator Cache(CardData data)
        {
            var url = data.Image;

            if (string.IsNullOrEmpty(data.Title))
            {
                data.Title = "default";
            }

            bool saveToFile;
            string imageName = string.Format(cachedImageNameFormat, data.Title, data.Id);
            string fileName = Application.persistentDataPath + "/" + Path.GetFileName(imageName);
#if !UNITY_WINRT
            if (File.Exists(fileName))
            {
                saveToFile = false;
                url = "file://" + fileName;
            }
            else
#endif
            {
                saveToFile = true;
                url = AzureMuseumImageFormatter.GetImageUri(url, 277, 274).ToString();
            }

            WWW www = null;

            int tries = 3;
            while (--tries >= 0)
            {
                www = new WWW(url);

                yield return www;

                if (www.bytesDownloaded != 0)
                {
                    break;
                }
            }

#if !UNITY_WINRT
            if (saveToFile && www != null && www.bytesDownloaded != 0)
            {
                fileName = Application.persistentDataPath + "/" + Path.GetFileName(imageName);
                var cache = new FileStream(fileName, FileMode.Create);
                cache.Write(www.bytes, 0, www.bytes.Length);
                cache.Close();
            }
#endif
        }

        public static string GetCachedPath(CardData data)
        {
            var url = data.Image;

            if (string.IsNullOrEmpty(data.Title))
            {
                data.Title = "default";
            }

            string imageName = string.Format(cachedImageNameFormat, data.Title, data.Id);
            string fileName = Application.persistentDataPath + "/" + Path.GetFileName(imageName);
#if !UNITY_WINRT
            if (File.Exists(fileName))
            {
                url = "file://" + fileName;
            }
            else
#endif
            {
                url = AzureMuseumImageFormatter.GetImageUri(url, 277, 274).ToString();
            }

            return url;
        }
    }
}
