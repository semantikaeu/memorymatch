namespace Assets.Scripts.SelectMultiplayer
{
    using Assets.GameEngine.Utils;

    using UnityEngine;

    public class PlayersButton : MonoBehaviour
    {
        public int NumberOfPlayers;
        public GameObject MenuObject;
        public GameObject SelectionObject;
        public float SelectedScale = 1.25f;
        public float NormalScale = 0.8f;

        private bool isSelected = false;

        private void Start()
        {
            Vector3 scale = transform.localScale;
            scale.x = scale.y = NormalScale;
        }

        private void OnMouseDown()
        {
            if (isSelected)
            {
                return;
            }

            IsSelected(true);

            if (MenuObject != null)
            {
                MenuObject.SendMessage("SetNumberOfPlayers", NumberOfPlayers);
            }
        }

        private void IsSelected(bool selected)
        {
            isSelected = selected;

            if (SelectionObject != null)
            {
                StartCoroutine(AnimationUtils.AnimateScale(SelectionObject, selected ? SelectedScale : NormalScale, 0.15f));
            }
        }

        private void Deselect()
        {
            IsSelected(false);
        }
    }
}
