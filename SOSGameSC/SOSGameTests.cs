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

        public SOSGame(int size)
        {
            if (size < 3)
                throw new AggregateException("Board size must be greater than 3 please!");
            Size = size;
            board = new char[size, size];
            BlueScore = 0; RedScore = 0;
        }
        // move handling
        public void PlaceLetter(int row, int col, string letter)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                throw new ArgumentOutOfRangeException();
            if (board[row, col] != '\0')
                return;
            if (letter != "S" && letter != "O")
                throw new ArgumentException("Only 'S' or 'O' please");
            board[row, col] = letter[0];
        }

        public bool MakeMove(int row, int col, char letter)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                throw new ArgumentOutOfRangeException();
            if (board[row, col] != 'O')
                return false;
            if (letter != 'S' && letter != 'O')
                throw new ArgumentException("Only 'S' or 'O' please");
            board[row, col] = letter;
            return true;
        }
        // check SOS game logic 
        public int CheckForSOS(int row, int col, Player player)
        {
            int score = 0;
            //horizontal 
            if (col >= 2 && board[row, col - 2] == 'S' && board[row, col - 1] == 'O' && board[row, col] == 'S') score++;
            if (col <= Size - 3 && board[row, col] == 'S' && board[row, col + 1] == 'O' && board[row, col + 2] == 'S') score++;
            //vertical 
            if (row >= 2 && board[row - 2, col] == 'S' && board[row - 1, col] == 'O' && board[row, col] == 'S') score++;
            if (row <= Size - 3 && board[row, col] == 'S' && board[row + 1, col] == 'O' && board[row + 2, col] == 'S') score++;
            // diagonal this way --> \ 
            if (row >= 2 && col >= 2 && board[row - 2, col - 2] == 'S' && board[row - 1, col - 1] == 'O' && board[row, col] == 'S') score++;
            if (row <= Size - 3 && col <= Size - 3 && board[row, col] == 'S' && board[row + 1, col + 1] == 'O' && board[row + 2, col + 2] == 'S') score++;
            // diagonal this way --> / 
            if (row >= 2 && col <= Size - 3 && board[row - 2, col + 2] == 'S' && board[row - 1, col + 1] == 'O' && board[row, col] == 'S') score++;
            if (row <= Size - 3 && col >= 2 && board[row, col] == 'S' && board[row + 1, col - 1] == 'O' && board[row + 2, col - 2] == 'S') score++;

            //updates scores
            if (player == Player.Blue)
                BlueScore += score;
            else 
                RedScore += score;
            return score;
        }

        //game state
        public bool IsBoardFull()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    return false;
            return true;
        }
        //legacy method for the tests
        public bool IsGameOver()
        {
            return IsBoardFull();
        }

        public string GetWinner()
        {
            if (BlueScore > RedScore)
                return "Blue";
            else if (RedScore > BlueScore)
                return "Red";
            else
                return null;
        }
        public int GetScore(Player player)
        {
            return player == Player.Blue ? BlueScore : RedScore;
        }
        public void Reset()
        {
            board = new char[Size, Size];
            BlueScore = 0;
            RedScore = 0;
        }

    }
}
