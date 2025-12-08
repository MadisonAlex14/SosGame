using SOSGameApp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RecordReplay
{
    public class Move
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public char Letter { get; set; }
        public PlayerColor Player { get; set; }

        public Move(int row, int col, char letter, PlayerColor player)
        {
            Row = row;
            Col = col;
            Letter = letter;
            Player = player;
        }
    }

    private readonly List<Move> moves = new List<Move>();
    private readonly Form1 form;

    public int MoveCount => moves.Count;
    public bool HasMoves() => moves.Count > 0;

    public RecordReplay(Form1 gameForm)
    {
        form = gameForm;
    }

    // record one move
    public void RecordMove(int row, int col, char letter, PlayerColor player)
    {
        moves.Add(new Move(row, col, letter, player));
    }

    // clear all saved moves
    public void ClearMoves()
    {
        moves.Clear();
    }

    // replays the full recorded game
    public async Task StartReplay(int delayMs = 500)
    {
        if (moves.Count == 0)
            return;

        form.DisableBoard();

        // reset UI board
        form.ResetBoardUI();

        foreach (var move in moves)
        {
            form.SetBoardCell(move.Row, move.Col, move.Letter, move.Player);

            // updates highlights after each move
            form.HighlightAllSequences();

            await Task.Delay(delayMs);
        }

        form.EnableBoard();
    }
}
