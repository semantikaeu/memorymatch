namespace Assets.Scripts.Popups
{
    using System;
    using System.Collections.Generic;

    using Assets.GameEngine.Data;
    using Assets.Scripts.Utils;

    using UnityEngine;

    public class QuestionLayout : MonoBehaviour
    {
        public Texture2D DotTexture;
        private int QuestionWidth = 350;
        public int AnswerWidth = 226;
        public int SmallFontSize = 9;
        public int NormalFontSize = 14;
        public Font DefaultFont;
        public GameObject ImagePosition;

        private readonly List<GUIStyle> answerStyles = new List<GUIStyle>();
        private readonly List<GUIContent> answerContents = new List<GUIContent>();
        private readonly List<Rect> answerPositions = new List<Rect>();

        private readonly List<Rect> dotPositions = new List<Rect>();
        private GUIStyle dotStyle;
        private GUIContent dotContent;

        private GUIStyle questionStyle;
        private GUIContent questionContent;
        private Rect questionPosition;

        private int scaledSmallFontSize = 10;
        private int scaledNormalFontSize = 15;
        private bool isMousePressed;

        private bool isEnabled;

        private Animator animator;
        private QuestionData currentQuestion;

        public void GenerateQuestions(QuestionData question)
        {
            Debug.Log("Image position: " + ImagePosition);
            scaledSmallFontSize = Scale(SmallFontSize);
            scaledNormalFontSize = Scale(NormalFontSize);

            currentQuestion = question;
            isEnabled = true;

            answerPositions.Clear();
            answerContents.Clear();
            answerStyles.Clear();
            dotPositions.Clear();

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

            dotContent = new GUIContent(DotTexture);
            dotStyle = new GUIStyle();
            dotStyle.fixedWidth = dotStyle.fixedHeight = Scale(20);

            questionStyle = new GUIStyle();
            questionStyle.richText = true;
            questionStyle.wordWrap = true;
            questionStyle.fontSize = scaledNormalFontSize;
            //questionStyle.fontStyle = FontStyle.Bold;
            questionStyle.font = DefaultFont;

            string text = "<color=#503a28>" + question.Question + "</color>";
            questionContent = new GUIContent(TextFormatting.ProcessTags(text, scaledSmallFontSize));

            int scaledWidth = Scale(QuestionWidth);
            questionPosition = new Rect(screenPosition.x, Screen.height - screenPosition.y, scaledWidth, Scale(100));

            float height = TextFormatting.CalculateHeight(questionContent, questionStyle, scaledWidth) + Scale(16);

            int i = 0;
            foreach (var answer in question.Answers)
            {
                height = AddQuestion(answer, height, i);
                ++i;
            }
        }

        private void Start()
        {
            GameObject popups = GameObject.Find("Popups");
            if (popups != null)
            {
                animator = popups.GetComponent<Animator>();
            }
        }

        private void OnEnable()
        {
            GenerateQuestions(DataMessenger.LastQuestion);
        }

        private void OnGUI()
        {
            GUI.Label(questionPosition, questionContent, questionStyle);

            if (DataMessenger.LastQuestionImageTexture != null)
            {
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                screenPosition.y = Screen.height - screenPosition.y;
                screenPosition.x += Scale(260);
                screenPosition.y += Scale(140);
                Rect rect = new Rect(screenPosition.x, screenPosition.y, Scale(110), Scale(110));
                GUI.DrawTexture(rect, DataMessenger.LastQuestionImageTexture, ScaleMode.ScaleToFit);
            }

            int size = answerContents.Count;
            for (int i = 0; i < size; ++i)
            {
                GUI.Label(answerPositions[i], answerContents[i], answerStyles[i]);
                GUI.Label(dotPositions[i], dotContent, dotStyle);
            }

            if (Input.GetMouseButtonDown(0) && !isMousePressed && isEnabled)
            {
                isMousePressed = true;

                MouseDown();
            }
            else
            {
                isMousePressed = false;
            }
        }

        private void MouseDown()
        {
            Vector2 position = Input.mousePosition;
            position.y = Screen.height - position.y;

            int size = answerPositions.Count;
            for (int i = 0; i < size; ++i)
            {
                Rect rect = answerPositions[i];
                rect.x = dotPositions[i].x;

                bool contains = rect.Contains(position);
                if (contains)
                {
                    bool correct = currentQuestion.CorrectAnswerIndex == i;
                    animator.SetBool("IsCorrectAnswer", correct);
                    animator.SetBool("HasAnswer", true);
                    break;
                }
            }
        }

        private float AddQuestion(string question, float height, int i)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

            var rect = new Rect(screenPosition.x - 2, Screen.height - screenPosition.y + height, dotStyle.fixedWidth, dotStyle.fixedHeight);
            dotPositions.Add(rect);

            int scaledWidth = Scale(AnswerWidth);

            GUIStyle style = new GUIStyle();
            style.richText = true;
            style.wordWrap = true;
            style.fontSize = scaledNormalFontSize;
            style.fixedWidth = scaledWidth;
            style.fontStyle = FontStyle.Bold;

            var content = new GUIContent(TextFormatting.ProcessTags("<color=" + Initialize.QuestionsFontColor + ">" + question + "</color>", scaledSmallFontSize));
            float scaledHeight = style.CalcHeight(content, scaledWidth);
            rect = new Rect(screenPosition.x - 2 + Scale(25), Screen.height - screenPosition.y + height, scaledWidth, scaledHeight);

            GUI.Label(rect, content, style);

            answerPositions.Add(rect);
            answerContents.Add(content);
            answerStyles.Add(style);

            height += rect.height + Scale(9);

            return height;
        }

        private int Scale(int number)
        {
            return (int)(number * Initialize.Scale);
        }
    }
}
