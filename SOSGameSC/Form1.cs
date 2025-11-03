using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
namespace SOSGameApp
{
    public partial class Form1 : Form
    {
        private SOSGame game;
        private Button[,] boardButtons;
        private bool isBlueTurn = true; // blue always goes first 
        private bool isSimpleGame = true;
        private bool gameModeLocked = false; // i have to lock it cause s**t wasnt staying\
        
        public Form1()
        {
            InitializeComponent();

            radioButton1.Checked = true;
            radioButton3.Checked = true;
            radioButton5.Checked = true;
            button1.Click += Button1_Click;
            buttonNewGame.Click += ButtonNewGame_Click;

            radioButton3.CheckedChanged += PlayerLetterChanged;
            radioButton4.CheckedChanged += PlayerLetterChanged;
            radioButton5.CheckedChanged += PlayerLetterChanged;
            radioButton6.CheckedChanged += PlayerLetterChanged;

            UpdateCurrentPlayerLabel();
        }

        // creates the board when button is clicked
        private void Button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int size) || size < 3)
            {
                MessageBox.Show("Enter in valid board size (>3!)");
                return;
            }

            game = new SOSGame(size);
            InitializeBoard(size);

            //lock game after board is created
            gameModeLocked = true;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }
        private void InitializeBoard(int size)
        {
            panel1.Controls.Clear();
            boardButtons = new Button[size, size];
            int buttonSize = Math.Min(panel1.Width / size, panel1.Height / size);
            int boardWidth = buttonSize * size;
            int boardHeight = buttonSize * size;
            int xOffset = (panel1.Width - boardWidth) / 2;
            int yOffset = (panel1.Height - boardHeight) / 2;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(buttonSize, buttonSize),
                        Location = new Point(xOffset + j * buttonSize, yOffset + i * buttonSize),
                        Font = new Font("Arial", 16, FontStyle.Bold),
                        Tag = new Point(i, j)
                    };
                    btn.Click += boardButtons_Click;
                    panel1.Controls.Add(btn);
                    boardButtons[i, j] = btn;
                }
            }
            // resets scores and turns 
            labelBlueScore.Text = "Blue: 0";
            labelRedScore.Text = "Red: 0";
            isBlueTurn = true;
            UpdateCurrentPlayerLabel();
        }

        private void boardButton_Click(object sender, EventArgs e)
        {
            if (game == null) return;

            Button clicked = sender as Button;
            Point pos = (Point)clicked.Tag;
            int row = pos.X;
            int col = pos.Y;

            if (!string.IsNullOrEmpty(clicked.Text)) return;

            string letter = GetSelectedLetter();
            clicked.Text = letter;
            clicked.ForeColor = isBlueTurn ? Color.Blue : Color.Red;

            //update game board
            game.PlaceLetter(row, col, letter);

            //check for SOS
            int sos = game.CheckForSOS(row, col, isBlueTurn ? Player.Blue : Player.Red);
            labelBlueScore.Text = $"Blue: {game.BlueScore}";
            labelRedScore.Text = $"Red: {game.RedScore}";

            //SIMPLE GAME
            if (isSimpleGame)
            {
                if (sos > 0)
                {
                    MessageBox.Show($"{(isBlueTurn ? "Blue" : "Red")} scored an SOS {(isBlueTurn ? "Blue" : "Red")} wins!");
                    ResetGameAfterSimpleEnd();
                    return;
                }
                if (game.IsBoardFull())
                {
                    MessageBox.Show("Draw, play again!");
                    ResetGameAfterSimpleEnd();
                    return;
                }
            }
            // GENERAL GAME
            if (!isSimpleGame && game.IsBoardFull())
            {
                var winner = game.GetWinner();
                string msg = winner == null ? "It's a tie!" : $"{winner} wins!";
                MessageBox.Show(msg);
                ResetGameAfterGeneralEnd();
                return;
            }
            //switch turns
            isBlueTurn = !isBlueTurn;
            UpdateCurrentPlayerLabel();
        }
        private string GetSelectedLetter()
        {
            if (isBlueTurn)
                return radioButton3.Checked ? "S" : "O";
            else 
                return radioButton5.Checked ? "S" : "O";
        }
        private void UpdateCurrentPlayerLabel()
        {
            label5.Text = $"Current Player: {(isBlueTurn ? "Blue" : "Red")}";
            label5.ForeColor = isBlueTurn ? Color.Blue : Color.Red;
        }
        private void PlayerLetterChanged(object sender, EventArgs e)
        {

        }
        private void radioButton1_CheckedCHanged(object sender, EventArgs e)
        {
            if (!gameModeLocked && radioButton1.Checked)
                isSimpleGame = true;
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!gameModeLocked && radioButton2.Checked)
                isSimpleGame = false;
        }
        private void ButtonNewGame_Click(object sender, EventArgs e)
        {
            if (game != null)
                game.Reset();
            if (game != null)
                InitializeBoard(game.Size);
            gameModeLocked = false;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }
        private void boardButtons_Click(object sender, EventArgs e)
        {

        }

        private void DisableBoard()
        {
            if (boardButtons == null) return;
            foreach (Button btn in boardButtons)
                btn.Enabled = false;
        }
        private void EndGame()
        {
            DisableBoard();
            gameModeLocked = false;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }
        private void ResetGameAfterSimpleEnd()
        {
            EndGame();
            if (game != null)
            {
                game.Reset();
                InitializeBoard(game.Size);
            }
        }
        private void ResetGameAfterGeneralEnd()
        {
            EndGame();
            if (game != null)
            {
                game.Reset();
                InitializeBoard(game.Size);
            }
        }
    }
}
