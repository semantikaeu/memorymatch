namespace Assets.GameEngine.Data
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using UnityEngine;

    [XmlRoot]
    public class CardData
    {
        public CardData()
        {
            Questions = new List<QuestionData>();
        }

        public CardData(CardData data)
            : this()
        {
            Id = data.Id;
            Image = data.Image;
            Instance = data.Instance;
            Questions.AddRange(data.Questions);
        }

        public CardData(int id, string image)
            : this(id, image, null, null)
        {
        }

        public CardData(int id, string image, QuestionData question)
            : this(id, image, question, null)
        {
        }

        public CardData(int id, string image, QuestionData question1, QuestionData question2)
            : this()
        {
            Id = id;
            Image = image;

            if (question1 != null)
            {
                Questions.Add(question1);
            }

            if (question2 != null)
            {
                Questions.Add(question2);
            }
        }

        public CardData(int id, string image, IEnumerable<QuestionData> questions)
            : this()
        {
            Id = id;
            Image = image;
            Questions.AddRange(questions);
        }

        [XmlElement]
        public int Id { get; set; }

        [XmlElement]
        public string Title { get; set; }

        [XmlElement]
        public string Image { get; set; }

        [XmlElement]
        public bool WasFlipped { get; set; }

        [XmlIgnore]
        public QuestionData Question
        {
            get { return Questions.Count == 0 ? null : Questions[Random.Range(0, Questions.Count)]; }
        }

        [XmlIgnore]
        public Texture2D Texture { get; set; }

        public List<QuestionData> Questions { get; set; }

        [XmlIgnore]
        public GameObject Instance { get; set; }
    }
}