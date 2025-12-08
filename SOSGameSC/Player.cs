namespace SOSGameApp
{
    public enum PlayerType { Human, Computer }

    public class Player
    {
        public PlayerColor Color { get; set; }
        public PlayerType Type { get; set; }

        public Player() { }

        public Player(PlayerColor color, PlayerType type = PlayerType.Human)
        {
            Color = color;
            Type = type;
        }
    }
}
