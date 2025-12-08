using System;
using System.Collections.Generic;
using Xunit;
using SOSGameApp;

namespace SOSGameTests
{
    public class SOSGameTests
    {
        [Fact]
        public void SimpleGame_PlaceMove_Success()
        {
            SOSGame game = new SOSGame(3, GameMode.Simple);

            bool result = game.PlaceMove(0, 0, 'S', PlayerColor.Blue);

            Assert.True(result);
            Assert.Equal('S', game.GetCell(0, 0).Letter);
            Assert.Equal(PlayerColor.Blue, game.GetCell(0, 0).Color);
        }

        [Fact]
        public void SimpleGame_PlaceMove_FailsOnOccupiedCell()
        {
            SOSGame game = new SOSGame(3, GameMode.Simple);
            game.PlaceMove(0, 0, 'S', PlayerColor.Blue);

            bool result = game.PlaceMove(0, 0, 'O', PlayerColor.Red);

            Assert.False(result);
            Assert.Equal('S', game.GetCell(0, 0).Letter);
            Assert.Equal(PlayerColor.Blue, game.GetCell(0, 0).Color);
        }

        [Fact]
        public void GeneralGame_CountSOS_Correct()
        {
            SOSGame game = new SOSGame(3, GameMode.General);

            // place moves to form S-O-S vertically
            game.PlaceMove(0, 0, 'S', PlayerColor.Blue);
            game.PlaceMove(1, 0, 'O', PlayerColor.Blue);
            bool moveResult = game.PlaceMove(2, 0, 'S', PlayerColor.Blue);

            int count = game.CountSOS(2, 0, 'S', PlayerColor.Blue);

            Assert.True(moveResult);
            Assert.Equal(1, count);
            Assert.Equal(1, game.BlueScore);
        }

        [Fact]
        public void Game_GameOver_WhenBoardFull()
        {
            SOSGame game = new SOSGame(2, GameMode.Simple);

            // fill the board
            game.PlaceMove(0, 0, 'S', PlayerColor.Blue);
            game.PlaceMove(0, 1, 'O', PlayerColor.Red);
            game.PlaceMove(1, 0, 'S', PlayerColor.Blue);
            game.PlaceMove(1, 1, 'O', PlayerColor.Red);

            Assert.True(game.GameOver);
        }

        [Fact]
        public void SimpleGame_GetSOSSequence_ReturnsCorrectPositions()
        {
            SOSGame game = new SOSGame(3, GameMode.Simple);

            game.PlaceMove(0, 0, 'S', PlayerColor.Blue);
            game.PlaceMove(1, 1, 'O', PlayerColor.Blue);
            game.PlaceMove(2, 2, 'S', PlayerColor.Blue);

            List<(int row, int col, char letter)> sequence = game.GetSOSSequenceForPlayer(PlayerColor.Blue);

            Assert.Contains((0, 0, 'S'), sequence);
            Assert.Contains((1, 1, 'O'), sequence);
            Assert.Contains((2, 2, 'S'), sequence);
        }
    }
}
