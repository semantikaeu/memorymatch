namespace Assets.Scripts.Game
{
    using Assets.GameEngine;

    using UnityEngine;

    /// <summary>
    /// This is used to reset game logic.
    /// </summary>
    public class RestartGameScript : MonoBehaviour
    {
        public GameObject GameLogicObject;

        /// <summary>
        /// Called when mouse is pressed down.
        /// </summary>
        private void OnMouseDown()
        {
            if (GameLogicObject != null)
            {
                MemoryGame.Current.IsBusy = false;

                var popups = GameObject.Find("Popups");
                if (popups != null)
                {
                    var animator = popups.GetComponent<Animator>();
                    animator.SetBool("ShowGameOver", false);
                }

                GameLogicObject.SendMessage("Start", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
