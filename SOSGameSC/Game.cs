using System.Collections.Generic;

namespace SOSGameApp
{
    public enum GameMode { Simple, General }

    public abstract class Game
    {
        public int Size { get; protected set; }
        public Cell[,] Board { get; protected set; }
        public int BlueScore { get; protected set; }
        public int RedScore { get; protected set; }
        public bool GameOver { get; protected set; }
        public int LastMoveSOSCount { get; protected set; }
        public GameMode Mode { get; protected set; }

        public Game(int size, GameMode mode)
        {
            Size = size;
            Mode = mode;
            Board = new Cell[size, size];
            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    Board[r, c] = new Cell(r, c);

            BlueScore = 0;
            RedScore = 0;
            GameOver = false;
            LastMoveSOSCount = 0;
        }

        public abstract bool PlaceMove(int r, int c, char letter, PlayerColor color);
        public abstract int CountSOS(int r, int c, char letter, PlayerColor color);

        public virtual List<List<Cell>> GetSOSSequencesForPlayer(PlayerColor color) =>
            SOSPatterns.GetSequences(Board, Size, color);

        protected bool IsBoardFull()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (Board[r, c].Color == PlayerColor.None)
                        return false;
            return true;
        }
    }
}
