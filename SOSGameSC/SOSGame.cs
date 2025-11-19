using System.Collections.Generic;

namespace SOSGameApp
{
    public class SOSGame
    {
        public int Size { get; }
        public GameMode Mode { get; }
        private readonly Cell[,] board;

        public int BlueScore { get; private set; }
        public int RedScore { get; private set; }

        public bool GameOver { get; private set; } = false;
        public int LastMoveSOSCount { get; private set; } = 0;
        public PlayerColor? Winner { get; private set; } = null;

        public SOSGame(int size, GameMode mode)
        {
            if (size < 3) throw new System.ArgumentException("Board size must be ≥ 3");
            Size = size;
            Mode = mode;
            board = new Cell[size, size];

            for (int r = 0; r < size; r++)
                for (int c = 0; c < size; c++)
                    board[r, c] = new Cell();
        }

        public Cell GetCell(int r, int c) => board[r, c];

        public bool PlaceMove(int r, int c, char letter, PlayerColor player)
        {
            if (GameOver || !IsValid(r, c) || !string.IsNullOrEmpty(board[r, c].Letter))
                return false;
            board[r, c].Letter = letter.ToString();
            board[r, c].Color = player;
            int sosCount = CountSOS(r, c, letter);
            LastMoveSOSCount = sosCount;

            // add score to current player
            if (player == PlayerColor.Blue) BlueScore += sosCount;
            else RedScore += sosCount;

            // end game in Simple mode
            if (Mode == GameMode.Simple && sosCount > 0)
            {
                GameOver = true;
                Winner = player;
            }
            // end game in General mode if board is full
            if (Mode == GameMode.General && IsBoardFull())
            {
                GameOver = true;
                Winner = BlueScore > RedScore ? PlayerColor.Blue :
                         RedScore > BlueScore ? PlayerColor.Red : null;
            }

            return true;
        }

        public bool IsBoardFull()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (string.IsNullOrEmpty(board[r, c].Letter)) return false;
            return true;
        }

        public List<(int row, int col)> GetSOSSequenceForPlayer(PlayerColor color)
        {
            var seq = new List<(int row, int col)>();
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    if (board[r, c].Letter != "S" || board[r, c].Color != color)
                        continue;
                    // hori
                    if (c + 2 < Size &&
                        board[r, c + 1].Letter == "O" && board[r, c + 1].Color == color &&
                        board[r, c + 2].Letter == "S" && board[r, c + 2].Color == color)
                        seq.AddRange(new[] { (r, c), (r, c + 1), (r, c + 2) });
                    // vert
                    if (r + 2 < Size &&
                        board[r + 1, c].Letter == "O" && board[r + 1, c].Color == color &&
                        board[r + 2, c].Letter == "S" && board[r + 2, c].Color == color)
                        seq.AddRange(new[] { (r, c), (r + 1, c), (r + 2, c) });
                    // diagonal this way --> \
                    if (r + 2 < Size && c + 2 < Size &&
                        board[r + 1, c + 1].Letter == "O" && board[r + 1, c + 1].Color == color &&
                        board[r + 2, c + 2].Letter == "S" && board[r + 2, c + 2].Color == color)
                        seq.AddRange(new[] { (r, c), (r + 1, c + 1), (r + 2, c + 2) });
                    // diagonal this way --> /
                    if (r - 2 >= 0 && c + 2 < Size &&
                        board[r - 1, c + 1].Letter == "O" && board[r - 1, c + 1].Color == color &&
                        board[r - 2, c + 2].Letter == "S" && board[r - 2, c + 2].Color == color)
                        seq.AddRange(new[] { (r, c), (r - 1, c + 1), (r - 2, c + 2) });
                }
            }

            return seq;
        }
        // keeping score for general game
        private int CountSOS(int r, int c, char letter)
        {
            int count = 0;
            if (letter != 'S' && letter != 'O') return 0;
            PlayerColor color = board[r, c].Color;
            (int dr, int dc)[] dirs = { (0, 1), (1, 0), (1, 1), (-1, 1) };
            foreach (var (dr, dc) in dirs)
            {
                int r1 = r + dr, c1 = c + dc;
                int r2 = r + 2 * dr, c2 = c + 2 * dc;
                if (!IsValid(r2, c2)) continue;
                var a = board[r, c];
                var b = board[r1, c1];
                var c3 = board[r2, c2];
                if (a.Letter == "S" && b.Letter == "O" && c3.Letter == "S" &&
                    a.Color == color && b.Color == color && c3.Color == color)
                {
                    count++;
                }
            }

            return count;
        }
        private bool IsValid(int r, int c) => r >= 0 && r < Size && c >= 0 && c < Size;
    }

    public class Cell
    {
        public string Letter { get; set; } = "";
        public PlayerColor Color { get; set; }
    }
}
