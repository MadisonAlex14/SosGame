using System;
using System.Collections.Generic;
// all for the comptuer player logic 
namespace SOSGameApp
{
    public class GameController
    {
        public SOSGame Game { get; private set; }
        public Player Blue { get; private set; }
        public Player Red { get; private set; }
        public Player CurrentPlayer { get; private set; }
        private readonly Random rand = new Random();
        public GameController(int size, GameMode mode)
        {
            Game = new SOSGame(size, mode);
            Blue = new Player(PlayerColor.Blue, PlayerType.Human);
            Red = new Player(PlayerColor.Red, PlayerType.Human);
            CurrentPlayer = Blue;
        }

        public void SetCurrentPlayer(Player player) => CurrentPlayer = player;
        public void SwitchPlayer() => CurrentPlayer = CurrentPlayer == Blue ? Red : Blue;
        // make a move without human interventation
        public bool MakeMove(int r, int c, char letter)
        {
            if (Game.GameOver || CurrentPlayer == null) return false;

            bool success = Game.PlaceMove(r, c, letter, CurrentPlayer.Color);

            // Only switch if no SOS was scored
            if (success && Game.LastMoveSOSCount == 0)
                SwitchPlayer();

            return success;
        }
        // try to win or make smart moves against other player
        public (int row, int col, char letter)? GetSmartComputerMove()
        {
            if (Game.GameOver) return null;

            var empty = new List<(int, int)>();
            for (int r = 0; r < Game.Size; r++)
                for (int c = 0; c < Game.Size; c++)
                    if (string.IsNullOrEmpty(Game.GetCell(r, c).Letter))
                        empty.Add((r, c));

            if (empty.Count == 0) return null;

            // try to complete SOS on its own
            foreach (var (r, c) in empty)
            {
                foreach (char letter in new[] { 'S', 'O' })
                {
                    var clone = CloneGame();
                    int before = CurrentPlayer.Color == PlayerColor.Blue ? clone.BlueScore : clone.RedScore;
                    clone.PlaceMove(r, c, letter, CurrentPlayer.Color);
                    int after = CurrentPlayer.Color == PlayerColor.Blue ? clone.BlueScore : clone.RedScore;
                    if (after > before) return (r, c, letter);
                }
            }

            // block opponent if it can
            var opponentColor = CurrentPlayer.Color == PlayerColor.Blue ? PlayerColor.Red : PlayerColor.Blue;
            foreach (var (r, c) in empty)
            {
                foreach (char letter in new[] { 'S', 'O' })
                {
                    var clone = CloneGame();
                    int before = opponentColor == PlayerColor.Blue ? clone.BlueScore : clone.RedScore;
                    clone.PlaceMove(r, c, letter, opponentColor);
                    int after = opponentColor == PlayerColor.Blue ? clone.BlueScore : clone.RedScore;
                    if (after > before) return (r, c, letter);
                }
            }

            // random fallback just incase
            var (row, col) = empty[rand.Next(empty.Count)];
            char fallback = rand.Next(2) == 0 ? 'S' : 'O';
            return (row, col, fallback);
        }
        private SOSGame CloneGame()
        {
            var clone = new SOSGame(Game.Size, Game.Mode);
            for (int r = 0; r < Game.Size; r++)
                for (int c = 0; c < Game.Size; c++)
                {
                    var cell = Game.GetCell(r, c);
                    if (!string.IsNullOrEmpty(cell.Letter))
                        clone.PlaceMove(r, c, cell.Letter[0], cell.Color);
                }
            return clone;
        }
    }
}
