namespace Assets.Scripts.Utils
{
    using System;
    using System.Linq;
    using System.Security;

    using UnityEngine;

    /// <summary>
    /// Inspired by this javascript code:
    /// http://forum.unity3d.com/threads/5531-Text-Wrap
    /// </summary>
    public static class TextFormatting
    {
        /// <summary>
        /// Calculates text height by ignoring bold style of the font. This will not work with inline bold tags.
        /// NOTE: This will not fix occasional new line when text ends right before end.
        /// </summary>
        /// <param name="content">Text content.</param>
        /// <param name="style">Text style.</param>
        /// <param name="width">Fixed width of text container.</param>
        /// <returns>Returns calculated height of the text.</returns>
        public static float CalculateHeight(GUIContent content, GUIStyle style, float width)
        {
            float height;

            // Bold fonts have much higher chance of having one new line to many than normal font.
            // There were no issues with missing new lines even with couple of extreme cases. (but extra new lines can occur)
            if (style.fontStyle == FontStyle.Bold)
            {
                style.fontStyle = FontStyle.Normal;
                style.wordWrap = true;
                style.fixedWidth = width;
                style.fixedHeight = 0;

                Texture2D t = new Texture2D(1,1);
                content.image = t;
                style.imagePosition = ImagePosition.ImageLeft;

                float min, max;
                style.CalcMinMaxWidth(content, out min, out max);

                style.clipping = TextClipping.Overflow;
                height = style.CalcHeight(content, min);
                style.fontStyle = FontStyle.Bold;
            }
            else
            {
                height = style.CalcHeight(content, width);
            }

            return height;
        }

        /// <summary>
        /// This will process tags, wrap text and scale sizes to match resolutions.
        /// </summary>
        /// <param name="guiText">Unity text block.</param>
        /// <param name="lineWidth">Width of the line used for text wrapping. (ignored if allowTextWrapping is false)</param>
        /// <param name="allowTextWrapping">Allow text wrapping.</param>
        /// <param name="autoScale">Auto scale font and line sizes.</param>
        /// <param name="smallFontSize">Size of the small font.</param>
        /// <param name="processTags">Process custom tags.</param>
        /// <see cref="TextWrap" />
        /// <see cref="ProcessTags"/>
        public static void ProcessText(GUIText guiText, int lineWidth = 400, bool allowTextWrapping = true, bool autoScale = true, int smallFontSize = 9, bool processTags = true)
        {
            // Scale according screen resolution.
            if (autoScale)
            {
                lineWidth = (int)(lineWidth * Initialize.Scale);
                smallFontSize = (int)(smallFontSize * Initialize.Scale);
                guiText.fontSize = (int)(guiText.fontSize * Initialize.Scale);
            }

            // Process custom tags.
            if (processTags)
            {
                guiText.text = ProcessTags(guiText.text, smallFontSize);
            }

            // Process text wrapping.
            if (allowTextWrapping)
            {
                TextWrap(guiText, lineWidth);
            }
        }

        /// <summary>
        /// Processes custom tags. Currently supported:
        /// SS - converts inline text as small text.
        /// </summary>
        /// <param name="text">Unprocessed text.</param>
        /// <param name="smallFontSize">Size of the small font.</param>
        /// <returns>Returns processed text.</returns>
        public static string ProcessTags(string text, int smallFontSize = 9)
        {
            if (text.Contains("<ss>"))
            {
                text = text.Replace("<ss>", "<size=" + smallFontSize + ">").Replace("</ss>", "</size>");
            }

            return text;
        }

        /// <summary>
        /// Texts the wrap.
        /// </summary>
        /// <param name="guiText">Unity text block.</param>
        /// <param name="lineWidth">Width of the line.</param>
        public static void TextWrap(GUIText guiText, int lineWidth)
        {
            string text = guiText.text;

            // This will check if text wrapping is necessary or if it is supported.
            var textSize = guiText.GetScreenRect();
            if (textSize.width <= lineWidth || !text.Contains(" "))
            {
                guiText.text = text;
                return;
            }

            // Split the string into separate words
            var words = text.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            var result = words[0] + " ";
            for (int i = 1; i < words.Length; ++i)
            {
                // Temporary store text with current word.
                var word = words[i];
                string tempSting = result + word + " ";

                // Calculate new size for GUI text component.
                guiText.text = tempSting;
                textSize = guiText.GetScreenRect();

                // Use temporary text if it fits otherwise add a new line before current word.
                if (textSize.width > lineWidth)
                {
                    result += "\n" + word + " ";
                }
                else
                {
                    result = tempSting;
                }
            }

            // Show result on screen
            guiText.text = result;
        }

        public static string AddSpacingBetweenCharacters(string text)
        {
            string result = string.Empty;

            // NOTE: Can't write shorter version because Unity does not support that for WinRT even though it should.
            foreach (var c in text)
            {
                result += c + " ";
            }

            return result;
        }

        public static string AddSpacingBetweenCharactersWithSmallSpace(string text, int smallSpace = 6)
        {
            string result = string.Empty;

            // NOTE: Can't write shorter version because Unity does not support that for WinRT even though it should.
            foreach (var c in text)
            {
                result += c + "<size=" + smallSpace + "> </size>";
            }

            return result;
        }
    }
}
