using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Windows.Forms;
namespace SOSGameApp
{
    public partial class Form1 : Form
    {
        private Button[,] boardButtons;
        private int boardSize = 3;
        private bool isBlueTurn = true;

        public Form1()
        {
            InitializeComponent();
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.BackColor = Color.White;
            button1.Click += Button1_Click;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out boardSize) || boardSize < 3)
            {
                MessageBox.Show("Please enter a valid board size (3-10)");
                return;
            }
            CreateBoard(boardSize);
        }



        private void CreateBoard(int size)
        {
            panel1.Controls.Clear();
            boardButtons = new Button[size, size];


            int buttonWidth = panel1.Width / boardSize;
            int buttonHeight = panel1.Height / boardSize;
            int buttonSize = Math.Min(panel1.Width, panel1.Height) / size;



            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button btn = new Button();
                    btn.Width = btn.Height = buttonSize - 2;
                    btn.Location = new Point(j * buttonSize, i * buttonSize);
                    btn.Font = new Font("Arial", buttonSize / 2, FontStyle.Bold);
                    btn.Tag = new Point(i, j);
                    btn.TextAlign = ContentAlignment.MiddleCenter;

                    btn.Click += BoardButton_Click;

                    panel1.Controls.Add(btn);
                    boardButtons[i, j] = btn;
                }
            }

            isBlueTurn = true;
            label5.Text = "Current Players Turn: Blue";
        }






        private void BoardButton_Click(object sender, EventArgs e)
        {
            // makes sure no one clicks on the same box twice
            Button btn = sender as Button;
            if (btn.Text != "") return;


            if (!IsCorrectPlayerMove())
            {
                MessageBox.Show("Not your turn or please select S or O!");
                return;
            }

 
            string letter = GetCurrentPlayerLetter();
            btn.Text = letter;
            btn.ForeColor = isBlueTurn ? Color.Blue : Color.Red;

            Point pos = (Point)btn.Tag;
            
            bool winner = false;
            string currentPlayer = isBlueTurn ? "Blue" : "Red";
            

            if (radioButton1.Checked)
            {
                if (CheckSOS())
                    winner = true;
            }
            else if (radioButton2.Checked)
            {
                if (IsBoardFull())
                {
                    winner = true;
                }
            }

            if (winner)
            {
                MessageBox.Show($"{currentPlayer} wins!");
                CreateBoard(boardButtons.GetLength(0)); //resets board
                return;
            }

                isBlueTurn = !isBlueTurn;
                label5.Text = "Current Player: " + (isBlueTurn ? "Blue" : "Red");

        }
        private string GetCurrentPlayerLetter()
        {
            if (isBlueTurn)
                return radioButton3.Checked ? "S" : "O";
        else
                return radioButton5.Checked ? "S" : "O";
         }


        private bool CheckSOS()
        {
            int size = boardButtons.GetLength(0);
            string[,] board = new string[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = boardButtons[i, j].Text;
                }
                    
            }

            for (int i = 0;i < size; i++)
            {
                for (int j = 0;j < size; j++)
                {
                     if (j + 2 < size && board[i, j] == "S" && board[i, j  + 1] == "O" && board[i, j + 2] == "S")
                         return true;
                     if (i + 2 < size && board[i, j] == "S" && board[i + 1, j] == "O" && board[i + 2, j] == "S")
                           return true;
                     if (i + 2 < size && j + 2 < size && board[i, j] == "S" && board[i + 1, j + 1] == "O" && board[i + 2, j + 2] == "S")
                         return true;
                    if (i + 2 < size && j - 2 >= 0 && board[i, j] == "S" && board[i + 1, j - 1] == "O" && board[i + 2, j - 2] == "S")
                        return true;
                }
            }
            
            return false;
        }

        private bool IsBoardFull()
        {
            int size = boardButtons.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (boardButtons[i, j].Text == "")
                        return false;
                }
            }
            return true;
        }

        private bool IsCorrectPlayerMove()
        {
            if (isBlueTurn)
            {
                return radioButton3.Checked || radioButton4.Checked;

            }
            else
            {
                return radioButton5.Checked || radioButton6.Checked;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        // panel resize that isnt f*******ing working.... 
        private void Panel1_Resize(object sender, EventArgs e)
        {
            if (boardButtons == null) return;
            int boardSize = boardButtons.GetLength(0);
            int buttonWidth = panel1.Width / boardSize;
            int buttonHeight = panel1.Height / boardSize;
            int buttonSize = Math.Min(buttonWidth, buttonHeight);

            for (int i = 0;i < boardSize; i++)
            {
                for (int j = 0;j < boardSize; j++)
                {
                    Button btn = boardButtons[i, j];
                    btn.Width = btn.Height = buttonSize - 2;
                    btn.Location = new Point(j * buttonSize, i * buttonSize);
                    btn.Font = new Font("Arial", buttonSize / 2, FontStyle.Bold);
                }
            }
        }

        private bool CheckSOSAt(int row, int col)
        {
            int size = boardButtons.GetLength(0);
            string[,] board = new string[size, size];

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    board[i, j] = boardButtons[i, j].Text;


            // checks hor
            if (col - 2 >= 0 && board[row, col - 2] == "S" && board[row, col - 1] == "O" && board[row, col] == "S")
                return true;
            if (col - 1 >= 0 && col + 1 < size && board[row, col -1] == "S" && board[row, col] == "O" && board[row ,col + 1] == "S")
                return true;
            if (col + 2 < size && board[row, col] == "S" && board[row, col + 1] == "O" && board[row, col + 2]== "S")
                return true;

            // checks vert
            if (row - 2 >= 0 && board[row - 2, col] == "S" && board[row - 1, col] == "O" && board[row, col] == "S")
                return true;
            if ((row - 1) >= 0 && row + 1 < size && board[row -1, col] == "S" && board[row, col] == "O" && board[row + 1, col] == "S")
                return true;
            if (row + 2 < size && board[row, col] == "S" && board[row + 1, col] == "O" && board[row + 2, col] == "S")
                return true;
            // diagonal right
            if (row - 2 >= 0 && col - 2 >= 0 && board[row -2, col -2] == "S" && board[ row -1, col -1] == "O" && board[row, col] == "S")
                return true;
            if (row -1 >= 0 && col -1 >= 0 && row + 1 < size && col + 1 < size && board[row -1, col -1] == "S" && board[row, col] == "O" && board[row +1, col + 1] == "S")
                return true;
            if (row + 2 < size && col + 2 < size && board[row, col] == "S" && board[row + 1, col +1] == "O" && board[row + 2, col + 2] == "S")
                return true;
            //digonal left
            if (row - 2 >= 0 && col + 2 < size && board[row - 2, col + 2] == "S" && board[row -1, col + 1] == "O" && board[row, col] == "S")
                return true;
            if (row - 1 >= 0 && col + 1 < size && row + 1 < size && col - 1 >= 0 && board[row -1, col + 1] == "O" && board[row, col] == "S")
                return true;
            if (row + 2 < size && col - 2 >= 0 && board[row, col] == "S" && board[row + 1, col - 1] == "O" && board[row + 2, col - 2] == "S")
                return true;

            return false;
        }
      
    }
    
}
