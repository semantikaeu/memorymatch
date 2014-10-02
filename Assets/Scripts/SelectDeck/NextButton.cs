namespace Assets.Scripts.SelectDeck
{
    using UnityEngine;

    public class NextButton : MonoBehaviour
    {
        private void OnMouseDown()
        {
            Application.LoadLevel("SlectNumberOfPlayers");
        }
    }
}
