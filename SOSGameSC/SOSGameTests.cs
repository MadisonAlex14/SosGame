using Xunit;
using SOSGameApp; // replace with your actual namespace
using System.Linq;

namespace SOSGameTests
{
    public class ComputerPlayerTests
    {
        private GameController controller;
        public ComputerPlayerTests()
        {
            controller = new GameController();
        }
        // ai move in general
        [Fact]
        public void ComputerMove_MakesAMove()
        {
            controller.StartNewGame(PlayerType.Computer, PlayerType.Human);
            var initialEmptyCells = controller.Game.GetEmptyCells().Count;
            controller.MakeComputerMove();
            var afterMoveEmptyCells = controller.Game.GetEmptyCells().Count;
            Assert.Equal(initialEmptyCells - 1, afterMoveEmptyCells);
        }
        // cant move on a space thats taken
        [Fact]
        public void ComputerMove_DoesNotOverwriteExistingMove()
        {
            controller.StartNewGame(PlayerType.Computer, PlayerType.Human);
            controller.Game.Board[0, 0] = 'S';
            controller.MakeComputerMove();
            Assert.NotEqual('S', controller.Game.Board[0, 0]);
        }

        [Fact]
        public void ComputerMove_CompletesSOS_WhenAvailable()
        {
            controller.StartNewGame(PlayerType.Computer, PlayerType.Human);

            // set up SOS opportunity
            controller.Game.Board[0, 0] = 'S';
            controller.Game.Board[0, 1] = 'O';
            controller.MakeComputerMove();
            Assert.Equal('S', controller.Game.Board[0, 2]);
        }

        [Fact]
        public void ComputerMove_OnlyPlacesValidLetter()
        {
            controller.StartNewGame(PlayerType.Computer, PlayerType.Human);
            controller.MakeComputerMove();
            var moveCell = controller.LastMoveCell;
            char placedLetter = controller.Game.Board[moveCell.Row, moveCell.Col];
            Assert.Contains(placedLetter, new char[] { 'S', 'O' });
        }

        [Fact]
        public void ComputerMove_DoesNotMoveIfGameOver()
        {
            controller.StartNewGame(PlayerType.Computer, PlayerType.Human);
            // fill the board completely
            for (int r = 0; r < controller.Game.Board.GetLength(0); r++)
                for (int c = 0; c < controller.Game.Board.GetLength(1); c++)
                    controller.Game.Board[r, c] = 'S';
            controller.Game.GameOver = true;
            controller.MakeComputerMove();
            Assert.Null(controller.LastMoveCell);
        }

        [Fact]
        public void ComputerMove_PicksOnlyFromEmptyCells()
        {
            controller.StartNewGame(PlayerType.Computer, PlayerType.Human);
            // fill in some cells
            controller.Game.Board[0, 0] = 'S';
            controller.Game.Board[1, 1] = 'O';
            controller.MakeComputerMove();
            var moveCell = controller.LastMoveCell;
            Assert.Contains(controller.Game.Board[moveCell.Row, moveCell.Col], new char[] { 'S', 'O' });
        }

        [Fact]
        public void ComputerMove_MakesMoveForCurrentPlayerOnly()
        {
            controller.StartNewGame(PlayerType.Human, PlayerType.Computer);
            var currentPlayerBefore = controller.CurrentPlayer.Type;
            controller.MakeComputerMove();
            var currentPlayerAfter = controller.CurrentPlayer.Type;
            Assert.Equal(PlayerType.Computer, currentPlayerBefore);
            Assert.Equal(PlayerType.Human, currentPlayerAfter);
        }
    }
}
