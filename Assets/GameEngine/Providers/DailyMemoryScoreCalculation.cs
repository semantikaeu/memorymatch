namespace Assets.GameEngine.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Assets.GameEngine.Data;

    public class DailyMemoryScoreCalculation : IScoreCalculation
    {
        public int Calculate(MemoryGameState state, string gameMode, GameRules rules, IList<CardData> cards, int playerScore)
        {
            if (cards == null)
            {
                throw new ArgumentNullException();
            }

            int score;
            int firstTry = cards.Count(card => !card.WasFlipped);

            if (state == MemoryGameState.PairFound || state == MemoryGameState.CorrectAnswer)
            {
                switch (firstTry)
                {
                    case 2:
                        score = rules.LucyPoints;
                        break;

                    case 1:
                        score = rules.ConcludePoints;
                        break;

                    default:
                        score = rules.PairFoundPoints;
                        break;
                }
            }
            else
            {
                score = (2 - firstTry) * rules.MissPoints;
            }

            foreach (var card in cards)
            {
                card.WasFlipped = true;
            }

            return score;
        }
    }
}
