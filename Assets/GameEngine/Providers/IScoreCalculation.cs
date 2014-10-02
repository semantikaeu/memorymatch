namespace Assets.GameEngine.Providers
{
    using System.Collections.Generic;

    using Assets.GameEngine.Data;

    /// <summary>
    /// This interface is used for abstracting scoring from main logic.
    /// In one of our earlier memory games we had more complex scoring system
    /// which could be used here. However this game has questions as part of game play mechanic
    /// and this will allow us to iterate between multiple scoring system until we find the most optimal solution.
    /// </summary>
    public interface IScoreCalculation
    {
        /// <summary>
        /// Calculates how much it should add to current player score.
        /// Most simplistic scoring is to add 1 if there is a pair otherwise 0.
        /// </summary>
        /// <param name="state">State of the game. (pair found, wrong pair, correct answer, etc.)</param>
        /// <param name="gameMode">Games mode. Q for questions and C for classic.</param>
        /// <param name="rules">Game rules mostly used for determining how much points a specific action is worth.</param>
        /// <param name="cards">Selected cards. This can be used to determine if cards were selected on the first try or not.</param>
        /// <param name="playerScore">Player score. Players with negative score could have different calculation than players with positive scores.</param>
        /// <returns>
        /// Returns number of points to add to current player score.
        /// </returns>
        int Calculate(MemoryGameState state, string gameMode, GameRules rules, IList<CardData> cards, int playerScore);
    }
}
