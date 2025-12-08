using System.Collections.Generic;

namespace SOSGameApp
{
    public class GeneralGame : Game
    {
        public GeneralGame(int size) : base(size, GameMode.General) { }

        public override bool PlaceMove(int r, int c, char letter, PlayerColor color)
        {
            if (Board[r, c].Color != PlayerColor.None) return false;

            Board[r, c].Letter = letter;
            Board[r, c].Color = color;

            LastMoveSOSCount = CountSOS(r, c, letter, color);

            if (color == PlayerColor.Blue) BlueScore += LastMoveSOSCount;
            else RedScore += LastMoveSOSCount;

            GameOver = IsBoardFull();
            return true;
        }

        public override int CountSOS(int r, int c, char letter, PlayerColor color)
        {
            int count = 0;
            foreach (var seq in SOSPatterns.GetSequences(Board, Size, color))
            {
                foreach (var cell in seq)
                    if (cell.Row == r && cell.Col == c)
                    {
                        count++;
                        break;
                    }
            }
            return count;
        }
    }
}
