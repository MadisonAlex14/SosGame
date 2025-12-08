namespace SOSGameApp
{
    public enum PlayerColor { None, Blue, Red }

    public class Cell
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public char Letter { get; set; } = '\0';
        public PlayerColor Color { get; set; } = PlayerColor.None;

        public Cell() { }
        public Cell(int r, int c)
        {
            Row = r;
            Col = c;
        }
    }
}
