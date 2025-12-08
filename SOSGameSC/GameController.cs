using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOSGameApp
{
    public class Move
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public char Letter { get; set; }
        public PlayerColor Player { get; set; }
    }

    public class GameController
    {
        public SOSGame Game { get; private set; }
        public Player Blue { get; private set; }
        public Player Red { get; private set; }
        public Player CurrentPlayer { get; private set; }

        private readonly Random rand = new Random();

        // Recording feature
        public bool IsRecording { get; set; } = false;
        public List<Move> RecordedMoves { get; private set; } = new List<Move>();

        public event Action<int, int, char> OnMoveMade;
        public event Action<Player> OnPlayerChanged;
        public event Action OnGameEnded;

        public GameController(int size, GameMode mode)
        {
            Game = new SOSGame(size, mode);
            Blue = new Player(PlayerColor.Blue, PlayerType.Human);
            Red = new Player(PlayerColor.Red, PlayerType.Human);
            CurrentPlayer = Blue;
        }

        public void SetCurrentPlayer(Player player)
        {
            CurrentPlayer = player;
            OnPlayerChanged?.Invoke(CurrentPlayer);
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == Blue) ? Red : Blue;
            OnPlayerChanged?.Invoke(CurrentPlayer);
        }

        public bool MakeMove(int r, int c, char letter)
        {
            if (Game.GameOver || CurrentPlayer == null) return false;

            bool success = Game.PlaceMove(r, c, letter, CurrentPlayer.Color);
            if (!success) return false;

            // Record this move
            if (IsRecording)
            {
                RecordedMoves.Add(new Move
                {
                    Row = r,
                    Col = c,
                    Letter = letter,
                    Player = CurrentPlayer.Color
                });
            }

            OnMoveMade?.Invoke(r, c, letter);

            // switch player if no SOS scored
            if (Game.LastMoveSOSCount == 0) SwitchPlayer();

            // if the game ends....stop recording and trigger event
            if (Game.GameOver)
                OnGameEnded?.Invoke();

            return true;
        }

        public async Task<(int row, int col, char letter)?> GetComputerMoveAsync()
        {
            return await Task.Run<(int row, int col, char letter)?>(() =>
            {
                var empty = new List<(int, int)>();
                for (int r = 0; r < Game.Size; r++)
                    for (int c = 0; c < Game.Size; c++)
                        if (Game.Board[r, c].Color == PlayerColor.None)
                            empty.Add((r, c));

                if (empty.Count == 0) return null;

                var (row, col) = empty[rand.Next(empty.Count)];
                char letter = rand.Next(2) == 0 ? 'S' : 'O';
                return (row, col, letter);
            });
        }

        public List<List<Cell>> GetSOSSequencesForPlayer(PlayerColor color) =>
            Game.GetSOSSequencesForPlayer(color);

        // replay recorded moves in order
        public async Task ReplayGame(Action<int, int, char> updateUICallback, int delayMs = 500)
        {
            if (RecordedMoves.Count == 0) return;

            int size = Game.Size;
            Game = new SOSGame(size, Game.Mode); // reset game board

            Blue = new Player(PlayerColor.Blue, Blue.Type);
            Red = new Player(PlayerColor.Red, Red.Type);
            CurrentPlayer = Blue;
            OnPlayerChanged?.Invoke(CurrentPlayer);

            foreach (var move in RecordedMoves)
            {
                Game.PlaceMove(move.Row, move.Col, move.Letter, move.Player);
                updateUICallback?.Invoke(move.Row, move.Col, move.Letter);
                await Task.Delay(delayMs);
            }

            OnGameEnded?.Invoke(); // show winner after replay
        }
    }
}
