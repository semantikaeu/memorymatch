namespace Assets.GameEngine.Data
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class QuestionData
    {
        public QuestionData()
        {
            Answers = new List<string>();
        }

        public QuestionData(string question, string answer1, string answer2, string answer3, int indexOfCorrectAnswer, string questionType = "normal")
            : this(string.Empty, question, answer1, answer2, answer3, indexOfCorrectAnswer, questionType)
        {
        }

        public QuestionData(string title, string question, string answer1, string answer2, string answer3, int indexOfCorrectAnswer, string questionType = "normal")
        {
            if (questionType != "normal" && !string.IsNullOrEmpty(questionType))
            {
                question = TranslateQuestion(question, questionType);
                answer1 = TranslateQuestion(answer1, questionType);
                answer2 = TranslateQuestion(answer2, questionType);
                answer3 = TranslateQuestion(answer3, questionType);
            }

            Title = title;
            Question = question;
            Answers = new List<string>();
            Answers.Add(answer1);
            Answers.Add(answer2);
            Answers.Add(answer3);

            CorrectAnswerIndex = indexOfCorrectAnswer;
        }

        public int CardId { get; set; }

        public string Title { get; set; }

        public string Question { get; set; }

        public List<string> Answers { get; set; }

        public int CorrectAnswerIndex { get; set; }

        [XmlIgnore]
        public bool ContainsQuestion
        {
            get { return Answers.Count > 0; }
        }

        [XmlIgnore]
        public string CorrectAnswer
        {
            get
            {
                if (Answers == null || Answers.Count <= CorrectAnswerIndex)
                {
                    return string.Empty;
                }

                return Answers[CorrectAnswerIndex];
            }
        }

        public static string TranslateQuestion(string question, string questionType)
        {
            if (questionType == "chem")
            {
                
            }

            return question;
        }
    }
}
