namespace Assets.Scripts.SelectDeck
{
    using UnityEngine;

    public class PlayButton : MonoBehaviour
    {
        private void OnMouseDown()
        {
            Application.LoadLevel("Game");
        }
    }
}
