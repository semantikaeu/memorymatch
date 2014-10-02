namespace Assets.Scripts.Home
{
    using Assets.GameEngine;

    using UnityEngine;

    public class MenuButtonScript : MonoBehaviour
    {
        public static bool IsPopupVisible;

        public GameObject ShowPopup;
        public GameObject HelpPopup;
        public bool IsInGameMode;

        private string buttonName;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            buttonName = name;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void OnMouseDown()
        {
            if (IsPopupVisible && buttonName != "HowToPlay")
            {
                return;
            }

            switch (buttonName)
            {
                case "Play":
                    Application.LoadLevel("DeckSelector");
                    break;

                case "Achievements":
                    Application.LoadLevel("Achievements");
                    break;

                case "CreateSets":
                    Application.LoadLevel("CreateSets");
                    break;

                case "About":
                    if (ShowPopup != null)
                    {
                        IsPopupVisible = true;
                        ShowPopup.SetActive(true);
                    }

                    break;

                case "HowToPlay":
                    if (HelpPopup != null)
                    {
                        if (!IsInGameMode || (IsInGameMode && !MemoryGame.Current.IsBusy))
                        {
                            HelpPopup.SetActive(true);
                        }
                    }

                    break;
            }
        }
    }

}
