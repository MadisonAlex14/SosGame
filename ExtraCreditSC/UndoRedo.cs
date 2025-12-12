using System.Collections.Generic;

namespace SOSGameApp
{
    public class UndoRedo
    {
        private readonly Form1 form;
        private readonly GameController controller;

        private readonly Stack<GameController.Move> undoStack = new Stack<GameController.Move>();
        private readonly Stack<GameController.Move> redoStack = new Stack<GameController.Move>();

        public UndoRedo(Form1 form, GameController controller)
        {
            this.form = form;
            this.controller = controller;
        }

        public void RecordMove(PlayerColor color, int row, int col, char letter)
        {
            var move = new GameController.Move
            {
                Row = row,
                Col = col,
                Letter = letter,
                Player = color
            };

            undoStack.Push(move);
            redoStack.Clear();
        }

        public void Undo()
        {
            if (undoStack.Count == 0) return;
            var move = undoStack.Pop();
            redoStack.Push(move);

            controller.Game.Board[move.Row, move.Col].Letter = '\0';
            controller.Game.Board[move.Row, move.Col].Color = PlayerColor.None;

            UpdateScores();

            controller.SetCurrentPlayer(controller.Blue.Color == move.Player ? controller.Blue : controller.Red);

            form.SetBoardCell(move.Row, move.Col, '\0', PlayerColor.None);
            form.HighlightAllSequences();
        }

        public void Redo()
        {
            if (redoStack.Count == 0) return;
            var move = redoStack.Pop();
            undoStack.Push(move);

            controller.Game.PlaceMove(move.Row, move.Col, move.Letter, move.Player);

            UpdateScores();

            controller.SetCurrentPlayer(controller.Blue.Color == move.Player ? controller.Red : controller.Blue);

            form.SetBoardCell(move.Row, move.Col, move.Letter, move.Player);
            form.HighlightAllSequences();
        }

        private void UpdateScores()
        {
            form.UpdateScoresUI(controller.Game.BlueScore, controller.Game.RedScore);
        }

        public void ClearStacks()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
        public void RecordMove(GameController.Move move)
        {
            undoStack.Push(move);   
            redoStack.Clear();     
        }
    }
}
