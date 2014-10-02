namespace Assets.Scripts.Home
{
    using Assets.GameEngine;
    using Assets.Scripts.Utils;

    using UnityEngine;

    public class HelpPopupScript : MonoBehaviour
    {
        public GameObject HidePopupObject;
        public Texture FrameTexture;
        public Texture[] ScreenShoots;
        public GameObject[] HelpFramePositions;

        public Texture NextTexture;
        public Texture BackTexture;
        public Texture CloseTexture;

        public int CurrentScreenShoot;

        private Vector2 scrollContent;
        private float progress = 1;

        private Rect controlRect;
        private Rect scrollRect;
        private Rect scrollContentRect;

        private float imageWidth;
        private float imageHeight;

        private Rect backButtonRect;
        private Rect nextButtonRect;
        private Rect closeButtonRect;

        private bool isGoingBack;

        /// <summary>
        /// Hide popups when enabled.
        /// </summary>
        /// ReSharper disable once UnusedMember.Local
        private void OnEnable()
        {
            MemoryGame.Current.IsBusy = true;
            CurrentScreenShoot = 0;
            isGoingBack = false;
            progress = 1;

            if (HidePopupObject != null)
            {
                HidePopupObject.SetActive(false);
            }

            controlRect = WorldToRect.GetRect(HelpFramePositions);

            scrollRect = controlRect;
            scrollRect.x += controlRect.width * 0.02f;
            scrollRect.y += controlRect.height * 0.02f;
            scrollRect.width = controlRect.width * 0.9505f;

            imageWidth = controlRect.width * 0.950f;
            imageHeight = scrollRect.height;

            scrollContentRect = new Rect(0, 0, scrollRect.width * (ScreenShoots.Length + 1), scrollRect.height - 50);

            float ratio = 800.0f / Screen.height;
            if (Initialize.IsWideScreen)
            {
                ratio = 780.0f / Screen.height;
            }

            float buttonWidth = BackTexture.width / ratio;
            float buttonHeight = BackTexture.height / ratio;
            float height = (imageHeight / 2.0f) - 50;
            float offsetX = (controlRect.x * 1.34f) - (buttonWidth / 2.0f);

            if (Screen.height > 768)
            {
                if (!Initialize.IsWideScreen)
                {
                    offsetX = (controlRect.x * 1.34f) - (buttonWidth / 2.0f);
                }
                else
                {
                    offsetX = (controlRect.x * 1.1f) - (buttonWidth / 2.0f);
                }
            }

            backButtonRect = new Rect(offsetX, controlRect.y + height, buttonWidth, buttonHeight);
            nextButtonRect = new Rect(offsetX + (controlRect.width * 0.9572f), controlRect.y + height, buttonWidth, buttonHeight);
            closeButtonRect = new Rect(offsetX + (controlRect.width * 0.9572f), 0, buttonWidth, buttonHeight);
        }

        /// <summary>
        /// Redraw GUI.
        /// </summary>
        /// ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            GUI.BeginScrollView(scrollRect, scrollContent, scrollContentRect, new GUIStyle(), new GUIStyle());

            int first;
            int second;

            if (!isGoingBack)
            {
                first = CurrentScreenShoot == 0 ? 0 : CurrentScreenShoot - 1;
                second = CurrentScreenShoot == 0 ? 1 : CurrentScreenShoot;
            }
            else
            {
                first = CurrentScreenShoot == 0 ? 0 : CurrentScreenShoot;
                second = CurrentScreenShoot == 0 ? 1 : CurrentScreenShoot + 1;
            }

            float offset = Initialize.Scale * 3;
            GUI.Label(new Rect((imageWidth * first) + offset, offset, imageWidth, imageHeight), ScreenShoots[first]);
            GUI.Label(new Rect((imageWidth * second) + offset, offset, imageWidth, imageHeight), ScreenShoots[second]);

            GUI.EndScrollView();

            // Frame
            GUI.Label(controlRect, FrameTexture);

            // UI
            GUI.Label(backButtonRect, BackTexture);
            GUI.Label(nextButtonRect, NextTexture);
            GUI.Label(closeButtonRect, CloseTexture);

            progress += 0.01f;

            float ox2 = imageWidth * CurrentScreenShoot;
            scrollContent.x = Mathf.SmoothStep(scrollContent.x, ox2, progress);
        }

        private void OnMouseDown()
        {
            if (progress < 0.8f)
            {
                return;
            }

            Vector2 position = Input.mousePosition;
            position.y = Screen.height - position.y;

            if (nextButtonRect.Contains(position))
            {
                progress = 0;
                if (CurrentScreenShoot + 1 < ScreenShoots.Length)
                {
                    // CurrentScreenShoot = (CurrentScreenShoot + 1) % ScreenShoots.Length;
                    ++CurrentScreenShoot;
                }

                isGoingBack = false;
            }

            if (backButtonRect.Contains(position))
            {
                progress = 0;
                if (CurrentScreenShoot > 0)
                {
                    // CurrentScreenShoot = (ScreenShoots.Length + CurrentScreenShoot - 1) % ScreenShoots.Length;
                    --CurrentScreenShoot;
                }

                isGoingBack = true;
            }

            Debug.Log(CurrentScreenShoot);
            if (closeButtonRect.Contains(position))
            {
                gameObject.SetActive(false);
                MenuButtonScript.IsPopupVisible = false;
                MemoryGame.Current.IsBusy = false;
            }
        }
    }
}
