namespace Assets.Scripts.CreateSet
{
    using Assets.Scripts.Utils;

    using UnityEngine;

    public class SearchButton : MonoBehaviour
    {
        /// <summary>
        /// Called when mouse is pressed down.
        /// </summary>
        private void OnMouseDown()
        {
            Debug.Log("Start search from button...");
            GetStuffFromEuropeana.Current.StartSearch(DataMessenger.LastSetsQuery);
        }
    }
}
