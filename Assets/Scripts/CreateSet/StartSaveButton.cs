using UnityEngine;

public class StartSaveButton : MonoBehaviour
{
    public GameObject GameLogicObject;
    
    /// <summary>
    /// Raises the mouse down event.
    /// </summary>
    private void OnMouseDown()
    {
        if (GameLogicObject != null)
        {
            GameLogicObject.SendMessage("ShowSavePopup");
        }
    }
}
