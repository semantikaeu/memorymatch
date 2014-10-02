namespace Assets.Scripts
{
    using Assets.GameEngine;

    using UnityEngine;

    public class BackButton : MonoBehaviour
    {
        private void OnMouseDown()
        {
            MemoryGame.Current.IsBusy = false;

            Application.LoadLevel("Home");
        }
    }
}
