namespace Assets.Scripts.CreateSet
{
    using UnityEngine;

    public class CancelButton : MonoBehaviour
    {
        public GameObject GameLogicObject;

        /// <summary>
        /// Called when mouse is pressed down.
        /// </summary>
        private void OnMouseDown()
        {
            if (GameLogicObject != null)
            {
                GameLogicObject.SendMessage("CancelSet");
            }
        }
    }
}
