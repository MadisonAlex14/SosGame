using System;
using System.Runtime.CompilerServices;
namespace SOSGame
{
    public class SOSGame
    {
        public int Size { get; private set; }
        public char[,] Board {  get; private set; }

        public SOSGame(int size)
        {
            Size = size;
            Board = new char[size, size];
        }
        public bool PlaceMove(int row, int col, char letter)
        {
            if (Board[row, col] != '\0') return false;
            Board[row, col] = letter;
            return true;
        }
        public char GetCell(int row, int col)
        {
            return Board[row, col];
        }
        public void ResetBoard()
        {
            Board = new char[Size, Size];
        }
    }
}
