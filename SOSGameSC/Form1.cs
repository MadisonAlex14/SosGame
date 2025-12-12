using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOSGameApp
{
    public partial class Form1 : Form
    {
        private GameController controller;
        private Button[,] boardButtons;
        private RecordReplay recordReplay;
        private DifficultyLevel selectedDifficulty = DifficultyLevel.Easy;
        private UndoRedo undoRedo;
        private Button btnUndo;
        private Button btnRedo;

        private bool lastGameEnded = false;

        public Form1()
        {
            InitializeComponent();
            InitializeDifficultyButtons();

            // default UI state
            radioButton1.Checked = true; // Simple mode
            radioButtonBlueHuman.Checked = true;
            radioButtonRedHuman.Checked = true;
            radioButton3.Checked = true; // Blue S
            radioButton5.Checked = true; // Red S

            buttonStartComputerGame.Visible = false;
            btnReplay.Visible = false;

            recordReplay = new RecordReplay(this);

            // theme buttons
            btnLightTheme.Click += (s, e) => ApplyTheme(Theme.Light);
            btnDarkTheme.Click += (s, e) => ApplyTheme(Theme.Dark);
            btnPinkTheme.Click += (s, e) => ApplyTheme(Theme.Pink);

            textBox1.Text = trackBarSize.Value.ToString();
            textBox1.Visible = false;

            // board button actions
            button1.Click += ButtonCreateBoard_Click;
            buttonNewGame.Click += ButtonNewGame_Click;
            buttonStartComputerGame.Click += ButtonStartComputerGame_Click;

            // Replay button behavior
            btnReplay.Click += async (s, e) =>
            {
                btnReplay.Enabled = false;
                await recordReplay.StartReplay(500);
                btnReplay.Enabled = true;
            };

            // undo and redo buttons 
            btnUndo = new Button
            {
                Text = "Undo",
                Size = new Size(75, 30),
                Location = new Point(20, 500)
            };
            btnUndo.Click += BtnUndo_Click;
            Controls.Add(btnUndo);

            btnRedo = new Button
            {
                Text = "Redo",
                Size = new Size(75, 30),
                Location = new Point(105, 500)
            };
            btnRedo.Click += BtnRedo_Click;
            Controls.Add(btnRedo);

            radioButtonBlueHuman.CheckedChanged += PlayerTypeChanged;
            radioButtonBlueComputer.CheckedChanged += PlayerTypeChanged;
            radioButtonRedHuman.CheckedChanged += PlayerTypeChanged;
            radioButtonRedComputer.CheckedChanged += PlayerTypeChanged;

            // difficulty buttons
            btnEasy.Click += (s, e) => selectedDifficulty = DifficultyLevel.Easy;
            btnMedium.Click += (s, e) => selectedDifficulty = DifficultyLevel.Medium;
            btnHard.Click += (s, e) => selectedDifficulty = DifficultyLevel.Hard;
        }

        private void ApplyTheme(Theme theme)
        {
            switch (theme)
            {
                case Theme.Light:
                    BackColor = SystemColors.Control;
                    panel1.BackColor = Color.White;
                    ForeColor = Color.Black;
                    break;

                case Theme.Dark:
                    BackColor = Color.FromArgb(30, 30, 30);
                    panel1.BackColor = Color.FromArgb(45, 45, 48);
                    ForeColor = Color.White;
                    break;

                case Theme.Pink:
                    BackColor = Color.FromArgb(255, 235, 240);
                    panel1.BackColor = Color.FromArgb(255, 245, 250);
                    ForeColor = Color.FromArgb(80, 20, 60);
                    break;
            }
        }

        private void PlayerTypeChanged(object sender, EventArgs e)
        {
            if (controller == null) return;

            controller.Blue.Type = radioButtonBlueComputer.Checked ? PlayerType.Computer : PlayerType.Human;
            controller.Red.Type = radioButtonRedComputer.Checked ? PlayerType.Computer : PlayerType.Human;

            buttonStartComputerGame.Visible =
                controller.Blue.Type == PlayerType.Computer &&
                controller.Red.Type == PlayerType.Computer;
        }

        private void ButtonCreateBoard_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int size) || size < 3)
            {
                MessageBox.Show("Enter a valid board size ≥ 3.");
                return;
            }

            GameMode mode = radioButton2.Checked ? GameMode.General : GameMode.Simple;
            controller = new GameController(size, mode);

            controller.Blue.Type = radioButtonBlueComputer.Checked ? PlayerType.Computer : PlayerType.Human;
            controller.Red.Type = radioButtonRedComputer.Checked ? PlayerType.Computer : PlayerType.Human;

            controller.OnMoveMade += UpdateBoardDuringReplay;
            controller.OnPlayerChanged += _ => UpdateCurrentPlayerLabel();
            controller.OnGameEnded += OnGameEnded;

            InitializeBoard(size);
            UpdateScores();
            controller.SetCurrentPlayer(controller.Blue);

            btnReplay.Visible = false;
            recordReplay.ClearMoves();
            lastGameEnded = false;

            if (controller.CurrentPlayer.Type == PlayerType.Computer)
                _ = RunComputerMovesAsync();

            undoRedo = new UndoRedo(this, controller);
        }

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

        private async void BoardButton_Click(object sender, EventArgs e)
        {
            if (controller == null || boardButtons == null || controller.CurrentPlayer == null || controller.Game.GameOver)
                return;

            Button btn = (Button)sender;
            if (!string.IsNullOrEmpty(btn.Text)) return;
            if (controller.CurrentPlayer.Type != PlayerType.Human) return;

            Point pos = (Point)btn.Tag;
            int row = pos.X;
            int col = pos.Y;
            char letter = GetSelectedLetter();

            bool moveSucceeded = await HandleMove(row, col, letter);

            if (controller.CurrentPlayer.Type == PlayerType.Computer && !controller.Game.GameOver)
                await RunComputerMovesAsync();
        }

        private char GetSelectedLetter()
        {
            if (controller.CurrentPlayer.Color == PlayerColor.Blue)
                return radioButton3.Checked ? 'S' : 'O';
            else
                return radioButton5.Checked ? 'S' : 'O';
        }

        private async Task<bool> HandleMove(int row, int col, char letter)
        {
            if (controller == null || controller.CurrentPlayer == null || controller.Game.GameOver)
                return false;

            Player playerBeforeMove = controller.CurrentPlayer;

            bool success = controller.MakeMove(row, col, letter);
            if (!success) return false;

            recordReplay.RecordMove(row, col, letter, playerBeforeMove.Color);
            undoRedo.RecordMove(playerBeforeMove.Color, row, col, letter);


            UpdateBoardUI(row, col, letter, playerBeforeMove.Color);
            HighlightAllSequences();
            UpdateScores();

            if (controller.Game.GameOver)
            {
                string message;
                if (controller.Game is SimpleGame)
                    message = $"{playerBeforeMove.Color} wins!";
                else
                    message = controller.Game.BlueScore > controller.Game.RedScore
                        ? "Blue wins!"
                        : controller.Game.RedScore > controller.Game.BlueScore
                            ? "Red wins!"
                            : "It's a tie!";

                MessageBox.Show(message, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DisableAllButtons();
                controller.IsRecording = false;
                lastGameEnded = true;
                return true;
            }

            UpdateCurrentPlayerLabel();
            return true;
        }

        private void UpdateBoardUI(int row, int col, char letter, PlayerColor color)
        {
            var btn = boardButtons[row, col];
            btn.Text = letter.ToString();
            btn.ForeColor = color == PlayerColor.Blue ? Color.Blue : Color.Red;
            btn.Click -= BoardButton_Click;

            HighlightAllSequences();
        }

        private void UpdateBoardDuringReplay(int row, int col, char letter)
        {
            if (boardButtons == null || controller == null) return;

            var color = controller.Game.Board[row, col].Color;
            boardButtons[row, col].Text = letter.ToString();
            boardButtons[row, col].ForeColor = color == PlayerColor.Blue ? Color.Blue : Color.Red;

            HighlightAllSequences();
        }

        private void DisableAllButtons()
        {
            if (boardButtons == null) return;

            for (int r = 0; r < boardButtons.GetLength(0); r++)
                for (int c = 0; c < boardButtons.GetLength(1); c++)
                    boardButtons[r, c].Enabled = false;
        }

        public void HighlightAllSequences()
        {
            if (boardButtons == null || controller == null) return;

            foreach (var btn in boardButtons)
                btn.BackColor = Color.White;

            var blueSequences = controller.Game.GetSOSSequencesForPlayer(PlayerColor.Blue);
            foreach (var sequence in blueSequences)
                foreach (var cell in sequence)
                    if (boardButtons[cell.Row, cell.Col].BackColor == Color.White)
                        boardButtons[cell.Row, cell.Col].BackColor = Color.LightBlue;

            var redSequences = controller.Game.GetSOSSequencesForPlayer(PlayerColor.Red);
            foreach (var sequence in redSequences)
                foreach (var cell in sequence)
                    boardButtons[cell.Row, cell.Col].BackColor = Color.LightCoral;
        }

        public void UpdateScoresUI(int blueScore, int redScore)
        {
            labelBlueScore.Text = $"Blue: {blueScore}";
            labelRedScore.Text = $"Red: {redScore}";
        }

        public void UpdateScores()
        {
            if (controller != null && controller.Game != null)
                UpdateScoresUI(controller.Game.BlueScore, controller.Game.RedScore);
            else
                UpdateScoresUI(0, 0); // default if no game exists
        }

        private void UpdateCurrentPlayerLabel()
        {
            if (controller == null || controller.CurrentPlayer == null) return;

            label5.Text = $"Current Player: {controller.CurrentPlayer.Color}";
            label5.ForeColor = controller.CurrentPlayer.Color == PlayerColor.Blue ? Color.Blue : Color.Red;
        }

        private async Task RunComputerMovesAsync()
        {
            while (controller != null &&
                   controller.CurrentPlayer.Type == PlayerType.Computer &&
                   !controller.Game.GameOver)
            {
                // No cast needed
                var move = Difficulty.GetMove(controller.Game, controller.CurrentPlayer.Color, selectedDifficulty);
                if (move == null) break;

                bool success = await HandleMove(move.Value.row, move.Value.col, move.Value.letter);
                if (!success) break;

                await Task.Delay(250);
            }
        }

        private void OnGameEnded()
        {
            if (controller == null) return;

            string message =
                controller.Game is SimpleGame
                    ? $"{controller.CurrentPlayer.Color} wins!"
                    : controller.Game.BlueScore > controller.Game.RedScore
                        ? "Blue wins!"
                        : controller.Game.RedScore > controller.Game.BlueScore
                            ? "Red wins!"
                            : "It's a tie!";

            MessageBox.Show(message, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

            DisableAllButtons();
            controller.IsRecording = false;
            lastGameEnded = true;
        }

        private void ResetBoard()
        {
            if (boardButtons == null) return;

            foreach (var btn in boardButtons)
            {
                btn.Text = "";
                btn.BackColor = Color.White;
                btn.Click -= BoardButton_Click;
                btn.Click += BoardButton_Click;
            }

            UpdateScores();
            UpdateCurrentPlayerLabel();
        }

        private void ButtonNewGame_Click(object sender, EventArgs e)
        {
            btnReplay.Visible = lastGameEnded && recordReplay.MoveCount > 0;
            if (!btnReplay.Visible)
            {
                controller = null;
                panel1.Controls.Clear();
                ResetBoard();
                recordReplay.ClearMoves();
                undoRedo?.Clear();

                labelBlueScore.Text = "Blue: 0";
                labelRedScore.Text = "Red: 0";
                label5.Text = "Current Player: None";

                buttonStartComputerGame.Visible = false;
            }

            lastGameEnded = false;
        }

        private async void ButtonStartComputerGame_Click(object sender, EventArgs e)
        {
            if (controller == null) return;

            if (controller.Blue.Type != PlayerType.Computer ||
                controller.Red.Type != PlayerType.Computer)
            {
                MessageBox.Show("Both players must be computers.");
                return;
            }

            buttonStartComputerGame.Visible = false;
            await RunComputerMovesAsync();
        }

        public void SetBoardCell(int row, int col, char letter, PlayerColor color)
        {
            if (boardButtons == null) return;

            var btn = boardButtons[row, col];
            btn.Text = letter.ToString();
            btn.ForeColor = color == PlayerColor.Blue ? Color.Blue : Color.Red;
            btn.Click -= BoardButton_Click;
        }

        public void DisableBoard() => DisableAllButtons();

        public void EnableBoard()
        {
            if (boardButtons == null) return;

            for (int r = 0; r < boardButtons.GetLength(0); r++)
                for (int c = 0; c < boardButtons.GetLength(1); c++)
                    if (string.IsNullOrEmpty(boardButtons[r, c].Text))
                        boardButtons[r, c].Enabled = true;
        }

        public void ResetBoardUI() => ResetBoard();

        private enum Theme { Light, Dark, Pink }

        private void InitializeDifficultyButtons()
        {
            btnEasy.Click += (s, e) => SetDifficulty(DifficultyLevel.Easy);
            btnMedium.Click += (s, e) => SetDifficulty(DifficultyLevel.Medium);
            btnHard.Click += (s, e) => SetDifficulty(DifficultyLevel.Hard);

            SetDifficulty(selectedDifficulty);
        }
        private void SetDifficulty(DifficultyLevel level)
        {
            selectedDifficulty = level;

            btnEasy.BackColor = level == DifficultyLevel.Easy ? Color.LightGreen : SystemColors.Control;
            btnMedium.BackColor = level == DifficultyLevel.Medium ? Color.Yellow : SystemColors.Control;
            btnHard.BackColor = level == DifficultyLevel.Hard ? Color.Red : SystemColors.Control;
        }
        private void BtnUndo_Click(object sender, EventArgs e)
        {
            undoRedo?.Undo();

            if (controller.CurrentPlayer.Type == PlayerType.Computer && !controller.Game.GameOver)
                _ = RunComputerMovesAsync(); // keep async for computer moves
        }

        private void BtnRedo_Click(object sender, EventArgs e)
        {
            undoRedo?.Redo();

            if (controller.CurrentPlayer.Type == PlayerType.Computer && !controller.Game.GameOver)
                _ = RunComputerMovesAsync();
        }
    }
}
