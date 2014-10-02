namespace Assets.Scripts.SelectMultiplayer
{
    using Assets.Scripts.Utils;

    using UnityEngine;

    public class SelectPlayers : MonoBehaviour
    {
        public GameObject[] PlayerObjects;
        public int NumberOfPlayers = 0;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            DataMessenger.NumberOfPlayers = 1;

            if (PlayerObjects != null && PlayerObjects.Length > 0)
            {
                for (int i = 0; i < PlayerObjects.Length; ++i)
                {
                    PlayerObjects[i].SendMessage("IsSelected", i == 0);
                }
            }
        }

        private void SetNumberOfPlayers(int numberOfPlayers)
        {
            if (NumberOfPlayers >= 0 && NumberOfPlayers < PlayerObjects.Length)
            {
                // If an object is disable it will not receive new message.
                bool isDisabled = !PlayerObjects[NumberOfPlayers].activeSelf;
                if (isDisabled)
                {
                    PlayerObjects[NumberOfPlayers].SetActive(true);
                }

                PlayerObjects[NumberOfPlayers].SendMessage("Deselect");

                if (isDisabled)
                {
                    PlayerObjects[NumberOfPlayers].SetActive(false);
                }
            }

            NumberOfPlayers = numberOfPlayers;
            DataMessenger.NumberOfPlayers = numberOfPlayers + 1;

            Debug.Log("Selected number of players: " + DataMessenger.NumberOfPlayers);
        }
    }
}
