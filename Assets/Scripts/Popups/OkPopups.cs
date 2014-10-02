using UnityEngine;

public class OkPopups : MonoBehaviour
{
    private Animator animator;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {
        var popups = GameObject.Find("Popups");
        if (popups != null)
        {
            animator = popups.GetComponent<Animator>();
        }
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {

    }

    private void OnMouseDown()
    {
        animator.SetBool("HasAnswer", false);
        animator.SetBool("IsCorrectAnswer", false);
        animator.SetBool("IsQuestionVisible", false);

        var gameLogic = GameObject.Find("Game");
        if (gameLogic != null)
        {
            gameLogic.SendMessage("QuestionIsAnswered", name != "Sorry");
        }
    }
}
