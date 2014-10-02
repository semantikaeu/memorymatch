namespace Assets.GameEngine.Utils
{
    using System;
    using System.Linq;
    using System.Text;

    using Assets.GameEngine.Providers;

    public class AzureMuseumImageFormatter
    {
        /// <summary>
        /// Gets a uri to the image which is created from logical uri of museums images
        /// </summary>
        /// <param name="pUrl">Logical museums image path</param>
        /// <param name="pWidth">Desired image width</param>
        /// <param name="pHeight">Desired image height</param>
        /// <param name="pShouldCrop">Should image be cropped</param>
        /// <returns>Returns image URI</returns>
        public static Uri GetImageUri(string pUrl, int pWidth = 100, int pHeight = 100, bool pShouldCrop = true)
        {
#if !UNITY_METRO
            if (string.IsNullOrEmpty(pUrl))
            {
                return null;
            }

            pUrl = pUrl.Replace("\\", "/");
            string url = pUrl.StartsWith("http")
                             ? pUrl
                             : "https://museums.blob.core.windows.net/data" + pUrl.Replace(" ", "%20");

            var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(url));

            var crypto = new CryptoProvider();
            var hmac = crypto.CreateHmac(
                Encoding.UTF8.GetBytes("laf#lg383ht()/;:O(/)(/)=g987ewt;2twfqw"), Encoding.UTF8.GetBytes(b64));

            string shortHash = CleanUpUrl(Convert.ToBase64String(hmac.Take(8).ToArray()));
            string mode = pShouldCrop ? "crop" : "max";

            string size = string.Empty;
            if (pWidth > 0 && pHeight > 0)
            {
                size = string.Format("&width={0}&height={1}", pWidth, pHeight);
            }

            string newUrl =
                string.Format(
                    "http://museu.ms/remote.jpg.ashx?mode={3}&format=png{1}&404=no_image.gif&urlb64={0}&hmac={2}",
                    b64,
                    size,
                    shortHash,
                    mode);

            return new Uri(newUrl);
#else
            return new Uri(pUrl);
#endif
        }

        /// <summary>
        /// Replaces unsupported symbols with supported ones.
        /// </summary>
        /// <param name="pUrl">The url.</param>
        /// <returns>Returns cleaned url.</returns>
        private static string CleanUpUrl(string pUrl)
        {
            return pUrl.Replace("=", string.Empty).Replace('+', '-').Replace('/', '_');
        }

        public Uri GetUri(string p_baseUri)
        {
            return GetImageUri(p_baseUri);
        }

        public Uri GetCroppedImageUrl(string p_baseUri, int p_width, int p_height)
        {
            return GetImageUri(p_baseUri, p_width, p_height);
        }

        public Uri GetImageUrl(string p_baseUri, int p_width, int p_height)
        {
            return GetImageUri(p_baseUri, p_width, p_height, false);
        }
    }
}
