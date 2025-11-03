using System;
namespace SOSGameApp
{
    public enum Player { Blue, Red }
    public class SOSGame
    {
        private char[,] board;
        public int Size { get; private set; }
        public int BlueScore { get; private set; }
        public int RedScore { get; private set; }

        public SOSGame(int sizse)
        {
            if (Size < 3)
                throw new ArgumentException("Please choose correct board size (>3)");
            Size = Size;
            board = new char[Size, Size];
            BlueScore = 0; RedScore = 0;
        }

        public void PlaceLetter(int row, int col, string letter)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                throw new ArgumentOutOfRangeException();
            if (board[row, col] != '\0')
                return;
            if (letter != "S" && letter != "O")
                throw new ArgumentException("Only 'S' or 'O' are allowed");
            board[row, col] = letter[0];
        }

        //sos detection logic 
        public int CheckForSOS(int row, int col, Player player)
        {
            int score = 0;
            // horizontal 
            if (col >= 2 && board[row, col - 2] == 'S' && board[row, col - 1] == 'O' && board[row, col] == 'S') score++;
            if (col <= Size - 3 && board[row, col] == 'S' && board[row, col + 1] == 'O' && board[row, col + 2] == 'S') score++;
            //vertical 
            if (row >= 2 && board[row - 2, col] == 'S' && board[row - 1, col] == 'O' && board[row, col] == 'S') score++;
            if (row <= Size - 3 && board[row, col] == 'S' && board[row + 1, col] == 'S' && board[row + 2, col] == 'S') score++;
            // diagonal this way --> \
            if (row >= 2 && col >= 2 && board[row - 2, col - 2] == 'S' && board[row - 1, col - 1] == 'O' && board[row, col] == 'S') score++;
            if (row <= Size - 3 && col <= Size - 3 && board[row, col] == 'S' && board[row + 1, col + 1] == 'O' && board[row + 2, col + 2] == 'S') score++;
            // diagonal this way --> / 
            if (row >= 2 && col <= Size - 3 && board[row - 2, col + 2] == 'S' && board[row - 1, col + 1] == 'O' && board[row, col] == 'S') score++;
            if (row <= Size - 3 && col >= 2 && board[row, col] == 'S' && board[row + 1, col - 1] == 'O' && board[row + 2, col - 2] == 'S') score++;

            //updates player's score
            if (player == Player.Blue)
                BlueScore += score;
            else 
                RedScore += score;
            return score;
        }

        public bool IsBoardFull()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (board[r, c] == '\0')
                        return false;
            return true;
        }

        //ties into Form1.cs compability 
        public string GetWinner()
        {
            if (BlueScore > RedScore)
                return "Blue";
            else if (RedScore > BlueScore)
                return "Red";
            else
                return null;
        }

        public void Reset()
        {
            board = new char[Size, Size];
            BlueScore = 0; RedScore = 0;
        }
    }
}
