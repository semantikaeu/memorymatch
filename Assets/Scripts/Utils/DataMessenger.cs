namespace Assets.Scripts.Utils
{
    using System;
    using System.Collections.Generic;

    using Assets.GameEngine.Data;

    using UnityEngine;

    public static class DataMessenger
    {
        static DataMessenger()
        {
            LastQuestion = new QuestionData();
            LastQuestion.Question = "Find the false sentence.";
            LastQuestion.Answers = new List<string> { "Pyrite was used as a source of ignition in early firearms in the 16th and 17th centuries,", "Pyrite remains in commercial use for the production of sulfur dioxide (use in e.g., paper industry),", "Pyrite is often associated with harder chalcopyrite (CuFeS<ss>2</ss>)." };

            LastQuestion.Question = "Find the false sentence. odhfasdo hfdosuhf asdoihfasdi f asdiohf asdohf fh asdklhf asdhf asdp fasdiohf aiosh fasdioh fasdkl END";
            LastQuestion.Answers = new List<string> { "Dogs can see blue and yellow, but have difficulty differentiating red and green colours,", "The height of dogs measured to the withers ranges from 15.2 centimetres in the Chihuahua to about 76 cm in the Irish Wolfhound,", "The dog was the last domesticated animal." };
            LastQuestion.CorrectAnswerIndex = 0;

            LastSetsQuery = string.Empty;
            NumberOfPlayers = 1;
        }

        public static QuestionData LastQuestion { get; set; }

        public static Texture2D LastQuestionImageTexture { get; set; }

        public static string LastSetsQuery { get; set; }

        public static DeckData SelectedDeck { get; set; }

        public static int Difficulty { get; set; }

        public static int NumberOfPlayers { get; set; }

        public static bool IsGameBusy { get; set; }
    }
}
