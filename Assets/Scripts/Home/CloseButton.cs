using UnityEngine;

namespace Assets.Scripts.Home
{
    public class CloseButton : MonoBehaviour
    {
        public GameObject[] HidePopupObject;

        // Update is called once per frame
        private void OnMouseDown()
        {
            if (HidePopupObject != null)
            {
                MenuButtonScript.IsPopupVisible = false;

                foreach (var popup in HidePopupObject)
                {
                    popup.SetActive(false);
                }
            }
        }
    }
}
