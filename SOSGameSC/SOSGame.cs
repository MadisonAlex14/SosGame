using System.Collections.Generic;

namespace SOSGameApp
{
    public class SOSGame
    {
        private Game GameInstance;

        public int Size => GameInstance.Size;
        public GameMode Mode => GameInstance.Mode;
        public Cell[,] Board => GameInstance.Board;
        public int BlueScore => GameInstance.BlueScore;
        public int RedScore => GameInstance.RedScore;
        public bool GameOver => GameInstance.GameOver;
        public int LastMoveSOSCount => GameInstance.LastMoveSOSCount;

        public SOSGame(int size, GameMode mode)
        {
            GameInstance = mode == GameMode.Simple
                ? new SimpleGame(size) as Game
                : new GeneralGame(size) as Game;
        }

        public Cell GetCell(int r, int c) => Board[r, c];

        public bool PlaceMove(int r, int c, char letter, PlayerColor color) =>
            GameInstance.PlaceMove(r, c, letter, color);

        public int CountSOS(int r, int c, char letter, PlayerColor color) =>
            GameInstance.CountSOS(r, c, letter, color);

        public List<List<Cell>> GetSOSSequencesForPlayer(PlayerColor color) =>
            GameInstance.GetSOSSequencesForPlayer(color);

        public List<(int row, int col, char letter)> GetSOSSequenceForPlayer(PlayerColor color)
        {
            var flat = new List<(int, int, char)>();
            foreach (var seq in GetSOSSequencesForPlayer(color))
                foreach (var cell in seq)
                    if (!flat.Contains((cell.Row, cell.Col, cell.Letter)))
                        flat.Add((cell.Row, cell.Col, cell.Letter));
            return flat;
        }
    }
}
