namespace Assets.Scripts.SelectDeck
{
    using UnityEngine;

    public class DifficultyButton : MonoBehaviour
    {
        public int Difficulty;
        public GameObject SelectionObject;

        private void OnMouseDown()
        {
            IsSelected(true);

            var obj = GameObject.Find("DifficultyMenu");
            if (obj != null)
            {
                obj.SendMessage("SetDifficulty", Difficulty);
            }
        }

        private void IsSelected(bool selected)
        {
            if (SelectionObject != null)
            {
                SelectionObject.SetActive(selected);
            }
        }

        private void Deselect()
        {
            IsSelected(false);
        }
    }
}
