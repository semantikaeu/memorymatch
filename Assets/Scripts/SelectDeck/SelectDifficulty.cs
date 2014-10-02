namespace Assets.Scripts.SelectDeck
{
    using Assets.Scripts.Utils;

    using UnityEngine;

    public class SelectDifficulty : MonoBehaviour
    {
        public GameObject[] DifficultyButtons;
        public int Difficulty = 0;

        private void Start()
        {
            DataMessenger.Difficulty = Difficulty;
        }

        private void SetDifficulty(int difficulty)
        {
            // ReSharper disable once IntroduceOptionalParameters.Local
            SetDifficulty(difficulty, false);
        }

        private void SetDifficulty(int difficulty, bool reselect)
        {
            if (Difficulty >= 0 && Difficulty != difficulty && Difficulty < DifficultyButtons.Length)
            {
                // If an object is disable it will not receive new message.
                bool isDisabled = !DifficultyButtons[Difficulty].activeSelf;
                if (isDisabled)
                {
                    DifficultyButtons[Difficulty].SetActive(true);
                }

                DifficultyButtons[Difficulty].SendMessage("Deselect");

                if (isDisabled)
                {
                    DifficultyButtons[Difficulty].SetActive(false);
                }
            }

            DataMessenger.Difficulty = Difficulty = difficulty;

            if (reselect)
            {
                if (!DifficultyButtons[Difficulty].activeSelf)
                {
                    DifficultyButtons[Difficulty].SetActive(true);
                }

                DifficultyButtons[Difficulty].SendMessage("IsSelected", true);
            }
        }

        private void UpdateDifficulty(int maxDifficulty)
        {
            if (Difficulty > maxDifficulty - 1)
            {
                SetDifficulty(maxDifficulty - 1, true);
            }

            int size = DifficultyButtons.Length;
            for (int i = 0; i < size; ++i)
            {
                DifficultyButtons[i].SetActive(i < maxDifficulty);
            }
        }
    }
}
