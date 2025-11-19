using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOSGameApp
{
    public partial class Form1 : Form
    {
        private GameController? controller;
        private Button[,]? boardButtons;

        public Form1()
        {
            InitializeComponent();

            // since it doesnt want to work I am making simple game mode the defult 
            radioButton1.Checked = true; // Simple mode
            radioButtonBlueHuman.Checked = true;
            radioButtonRedHuman.Checked = true;
            radioButton3.Checked = true; // Blue S
            radioButton5.Checked = true; // Red S

            button1.Click += ButtonCreateBoard_Click;
            buttonNewGame.Click += ButtonNewGame_Click;
            buttonStartComputerGame.Click += ButtonStartComputerGame_Click;

            radioButtonBlueHuman.CheckedChanged += PlayerTypeChanged;
            radioButtonBlueComputer.CheckedChanged += PlayerTypeChanged;
            radioButtonRedHuman.CheckedChanged += PlayerTypeChanged;
            radioButtonRedComputer.CheckedChanged += PlayerTypeChanged;

            buttonStartComputerGame.Visible = false;
        }
        // players switch back and fourth
        private void PlayerTypeChanged(object? sender, EventArgs e)
        {
            if (controller == null) return;

            controller.Blue.Type = radioButtonBlueComputer.Checked ? PlayerType.Computer : PlayerType.Human;
            controller.Red.Type = radioButtonRedComputer.Checked ? PlayerType.Computer : PlayerType.Human;

            buttonStartComputerGame.Visible = controller.Blue.Type == PlayerType.Computer && controller.Red.Type == PlayerType.Computer;
        }
        // press this button and the board will be created on panel1
        private void ButtonCreateBoard_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int size) || size < 3)
            {
                MessageBox.Show("Enter a valid board size ≥ 3.");
                return;
            }

            GameMode mode = radioButton2.Checked ? GameMode.General : GameMode.Simple;
            controller = new GameController(size, mode);

            // set player types -- human or computer
            controller.Blue.Type = radioButtonBlueComputer.Checked ? PlayerType.Computer : PlayerType.Human;
            controller.Red.Type = radioButtonRedComputer.Checked ? PlayerType.Computer : PlayerType.Human;

            InitializeBoard(size);
            UpdateScores();

            controller.SetCurrentPlayer(controller.Blue); // Blue starts
            UpdateCurrentPlayerLabel();

            if (controller.CurrentPlayer.Type == PlayerType.Computer)
                _ = RunComputerMovesAsync();
        }
        // make the board itself
        private void InitializeBoard(int size)
        {
            panel1.Controls.Clear();
            boardButtons = new Button[size, size];

            int btnSize = Math.Min(panel1.Width / size, panel1.Height / size);
            int xOffset = (panel1.Width - btnSize * size) / 2;
            int yOffset = (panel1.Height - btnSize * size) / 2;

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    var btn = new Button
                    {
                        Size = new Size(btnSize, btnSize),
                        Location = new Point(xOffset + c * btnSize, yOffset + r * btnSize),
                        Font = new Font("Arial", 16, FontStyle.Bold),
                        Tag = new Point(r, c),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Text = ""
                    };
                    btn.FlatAppearance.BorderColor = Color.Gray;
                    btn.Click += BoardButton_Click;

                    panel1.Controls.Add(btn);
                    boardButtons[r, c] = btn;
                }
            }
        }

        private async void BoardButton_Click(object? sender, EventArgs e)
        {
            if (controller == null || boardButtons == null || controller.CurrentPlayer == null || controller.Game.GameOver)
                return;

            Button btn = (Button)sender!;
            if (!string.IsNullOrEmpty(btn.Text)) return;
            if (controller.CurrentPlayer.Type != PlayerType.Human) return;

            Point pos = (Point)btn.Tag;
            char letter = GetSelectedLetter();

            await HandleMove(pos.X, pos.Y, letter);
        }

        private char GetSelectedLetter()
        {
            return controller!.CurrentPlayer.Color switch
            {
                PlayerColor.Blue => radioButton3.Checked ? 'S' : 'O',
                PlayerColor.Red => radioButton5.Checked ? 'S' : 'O',
                _ => 'S'
            };
        }
        // makes sure the moves and gameplay is right 
        private async Task HandleMove(int row, int col, char letter)
        {
            if (controller == null || controller.CurrentPlayer == null || controller.Game.GameOver)
                return;

            PlayerColor playerColor = controller.CurrentPlayer.Color;
            bool success = controller.MakeMove(row, col, letter);

            if (!success) return;

            UpdateBoardUI(row, col, letter, playerColor);
            HighlightAllSequences();
            UpdateScores();

            // end game check number 100
            if (controller.Game.GameOver)
            {
                DeclareWinnerAndReset();
                return;
            }

            UpdateCurrentPlayerLabel();

            // if next player is computer, run AI from GameController
            if (controller.CurrentPlayer.Type == PlayerType.Computer)
                await RunComputerMovesAsync();
        }

        private void UpdateBoardUI(int row, int col, char letter, PlayerColor playerColor)
        {
            var btn = boardButtons![row, col];
            btn.Text = letter.ToString();
            btn.ForeColor = playerColor == PlayerColor.Blue ? Color.Blue : Color.Red;
            btn.Click -= BoardButton_Click; // disable further clicks
        }

        private void HighlightAllSequences()
        {
            if (boardButtons == null || controller == null) return;

            foreach (var btn in boardButtons)
                btn.BackColor = Color.White;

            foreach (var cell in controller.Game.GetSOSSequenceForPlayer(PlayerColor.Blue))
                boardButtons[cell.row, cell.col].BackColor = Color.LightBlue;

            foreach (var cell in controller.Game.GetSOSSequenceForPlayer(PlayerColor.Red))
                boardButtons[cell.row, cell.col].BackColor = Color.LightCoral;
        }
        private void UpdateScores()
        {
            if (controller == null) return;
            labelBlueScore.Text = $"Blue: {controller.Game.BlueScore}";
            labelRedScore.Text = $"Red: {controller.Game.RedScore}";
        }
        private void UpdateCurrentPlayerLabel()
        {
            if (controller == null || controller.CurrentPlayer == null) return;
            label5.Text = $"Current Player: {controller.CurrentPlayer.Color}";
            label5.ForeColor = controller.CurrentPlayer.Color == PlayerColor.Blue ? Color.Blue : Color.Red;
        }
        private async Task RunComputerMovesAsync()
        {
            if (controller == null || controller.CurrentPlayer == null || controller.Game.GameOver)
                return;

            while (controller.CurrentPlayer.Type == PlayerType.Computer && !controller.Game.GameOver)
            {
                var move = controller.GetSmartComputerMove();
                if (move == null) break;

                await HandleMove(move.Value.row, move.Value.col, move.Value.letter);
                await Task.Delay(200);

                if (controller.Game.GameOver) break;
            }
        }
        private void DeclareWinnerAndReset()
        {
            if (controller == null) return;

            string message = controller.Game.Mode == GameMode.Simple
                ? controller.Game.Winner switch
                {
                    PlayerColor.Blue => "Blue wins (Simple Mode)!",
                    PlayerColor.Red => "Red wins (Simple Mode)!",
                    _ => "It's a tie!"
                }
                : controller.Game.BlueScore > controller.Game.RedScore ? "Blue wins!" :
                  controller.Game.RedScore > controller.Game.BlueScore ? "Red wins!" : "It's a tie!";

            MessageBox.Show(message, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // reset game board
            controller = null;
            panel1.Controls.Clear();
            labelBlueScore.Text = "Blue: 0";
            labelRedScore.Text = "Red: 0";
            label5.Text = "Current Player: None";
            buttonStartComputerGame.Visible = false;
        }
        private void ButtonNewGame_Click(object? sender, EventArgs e) => DeclareWinnerAndReset();
        private async void ButtonStartComputerGame_Click(object? sender, EventArgs e)
        {
            if (controller == null) return;
            if (controller.Blue.Type != PlayerType.Computer || controller.Red.Type != PlayerType.Computer)
            {
                MessageBox.Show("Both players must be computers.");
                return;
            }
            buttonStartComputerGame.Visible = false;
            await RunComputerMovesAsync();
        }
    }
}
