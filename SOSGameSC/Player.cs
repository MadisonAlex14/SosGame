namespace SOSGameApp
{
    public enum PlayerColor { Blue, Red }
    public enum PlayerType { Human, Computer }
    public enum GameMode { Simple, General }

    public class Player
    {
        public PlayerColor Color { get; set; }
        public PlayerType Type { get; set; }

        public Player(PlayerColor color, PlayerType type)
        {
            Color = color;
            Type = type;
        }
    }
}
