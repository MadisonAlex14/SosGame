using System.Collections.Generic;

namespace SOSGameApp
{
    public static class SOSPatterns
    {
        public static List<List<Cell>> GetSequences(Cell[,] board, int size, PlayerColor color)
        {
            var sequences = new List<List<Cell>>();

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    if (board[r, c].Letter != 'S' || board[r, c].Color != color) continue;

                    // horizontal
                    if (c <= size - 3 &&
                        board[r, c + 1].Letter == 'O' && board[r, c + 1].Color == color &&
                        board[r, c + 2].Letter == 'S' && board[r, c + 2].Color == color)
                        sequences.Add(new List<Cell> { board[r, c], board[r, c + 1], board[r, c + 2] });
                    // vertical
                    if (r <= size - 3 &&
                        board[r + 1, c].Letter == 'O' && board[r + 1, c].Color == color &&
                        board[r + 2, c].Letter == 'S' && board[r + 2, c].Color == color)
                        sequences.Add(new List<Cell> { board[r, c], board[r + 1, c], board[r + 2, c] });
                    // diagonal down-right
                    if (r <= size - 3 && c <= size - 3 &&
                        board[r + 1, c + 1].Letter == 'O' && board[r + 1, c + 1].Color == color &&
                        board[r + 2, c + 2].Letter == 'S' && board[r + 2, c + 2].Color == color)
                        sequences.Add(new List<Cell> { board[r, c], board[r + 1, c + 1], board[r + 2, c + 2] });
                    // diagonal up-right
                    if (r >= 2 && c <= size - 3 &&
                        board[r - 1, c + 1].Letter == 'O' && board[r - 1, c + 1].Color == color &&
                        board[r - 2, c + 2].Letter == 'S' && board[r - 2, c + 2].Color == color)
                        sequences.Add(new List<Cell> { board[r, c], board[r - 1, c + 1], board[r - 2, c + 2] });
                }
            }

            return sequences;
        }
    }
}
