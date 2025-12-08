using System.Collections.Generic;

namespace SOSGameApp
{
    public class SimpleGame : Game
    {
        public SimpleGame(int size) : base(size, GameMode.Simple) { }

        public PlayerColor? MakeMove(int r, int c, char letter, PlayerColor color)
        {
            // invalid move
            if (r < 0 || r >= Size || c < 0 || c >= Size) return null;
            if (Board[r, c].Color != PlayerColor.None) return null;

            Board[r, c].Letter = letter;
            Board[r, c].Color = color;

            int sosCount = CountSOS(r, c, letter, color);

            if (sosCount > 0)
            {
                GameOver = true;
                return color; // winner
            }

            GameOver = IsBoardFull();
            return null;
        }

        public override bool PlaceMove(int r, int c, char letter, PlayerColor color)
        {
            // calls MakeMove internally
            PlayerColor? winner = MakeMove(r, c, letter, color);

            if (color == PlayerColor.Blue) BlueScore += CountSOS(r, c, letter, color);
            else if (color == PlayerColor.Red) RedScore += CountSOS(r, c, letter, color);

            // return true if move succeeded
            return winner != null || Board[r, c].Color == color;
        }

        public override int CountSOS(int r, int c, char letter, PlayerColor color)
        {
            int count = 0;
            foreach (var seq in SOSPatterns.GetSequences(Board, Size, color))
            {
                foreach (var cell in seq)
                {
                    if (cell.Row == r && cell.Col == c)
                    {
                        count++;
                        break;
                    }
                }
            }
            return count;
        }
    }
}
