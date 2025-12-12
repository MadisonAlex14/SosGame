using System.Drawing;
using System.Windows.Forms;

namespace SOSGameApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            radioButton1 = new RadioButton();
            radioButton2 = new RadioButton();
            button1 = new Button();
            label2 = new Label();
            textBox1 = new TextBox();
            label3 = new Label();
            groupBoxBluePlayerType = new GroupBox();
            radioButtonBlueHuman = new RadioButton();
            radioButtonBlueComputer = new RadioButton();
            groupBoxBlueLetter = new GroupBox();
            radioButton3 = new RadioButton();
            radioButton4 = new RadioButton();
            label4 = new Label();
            groupBoxRedPlayerType = new GroupBox();
            radioButtonRedHuman = new RadioButton();
            radioButtonRedComputer = new RadioButton();
            groupBoxRedLetter = new GroupBox();
            radioButton5 = new RadioButton();
            radioButton6 = new RadioButton();
            panel1 = new Panel();
            labelBlueScore = new Label();
            labelRedScore = new Label();
            label5 = new Label();
            buttonNewGame = new Button();
            buttonStartComputerGame = new Button();
            chkRecordGame = new CheckBox();
            btnReplay = new Button();
            
            groupBoxBluePlayerType.SuspendLayout();
            groupBoxBlueLetter.SuspendLayout();
            groupBoxRedPlayerType.SuspendLayout();
            groupBoxRedLetter.SuspendLayout();
            SuspendLayout();

            // label1
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            label1.Location = new Point(26, 29);
            label1.Name = "label1";
            label1.Size = new Size(81, 29);
            label1.TabIndex = 0;
            label1.Text = "SOS: ";
            // radioButton1
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(122, 38);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(76, 24);
            radioButton1.TabIndex = 1;
            radioButton1.Text = "Simple";
            // radioButton2
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(220, 38);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(81, 24);
            radioButton2.TabIndex = 2;
            radioButton2.Text = "General";
            // button1
            button1.Location = new Point(791, 33);
            button1.Name = "button1";
            button1.Size = new Size(140, 34);
            button1.TabIndex = 3;
            button1.Text = "Create Board";
            // label2
            label2.AutoSize = true;
            label2.Location = new Point(554, 40);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 4;
            label2.Text = "Board Size:";
            // textBox1
            textBox1.Location = new Point(643, 37);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(89, 27);
            textBox1.TabIndex = 5;
            // label3
            label3.AutoSize = true;
            label3.Font = new Font("Rockwell", 12F, FontStyle.Bold);
            label3.ForeColor = SystemColors.ActiveCaption;
            label3.Location = new Point(48, 188);
            label3.Name = "label3";
            label3.Size = new Size(124, 24);
            label3.TabIndex = 6;
            label3.Text = "Blue Player";
            // groupBoxBluePlayerType
            groupBoxBluePlayerType.Controls.Add(radioButtonBlueHuman);
            groupBoxBluePlayerType.Controls.Add(radioButtonBlueComputer);
            groupBoxBluePlayerType.Location = new Point(52, 230);
            groupBoxBluePlayerType.Name = "groupBoxBluePlayerType";
            groupBoxBluePlayerType.Size = new Size(120, 70);
            groupBoxBluePlayerType.TabIndex = 7;
            groupBoxBluePlayerType.TabStop = false;
            groupBoxBluePlayerType.Text = "Player Type";
            // radioButtonBlueHuman
            radioButtonBlueHuman.AutoSize = true;
            radioButtonBlueHuman.Location = new Point(10, 20);
            radioButtonBlueHuman.Name = "radioButtonBlueHuman";
            radioButtonBlueHuman.Size = new Size(78, 24);
            radioButtonBlueHuman.TabIndex = 0;
            radioButtonBlueHuman.Text = "Human";
            // radioButtonBlueComputer
            radioButtonBlueComputer.AutoSize = true;
            radioButtonBlueComputer.Location = new Point(10, 40);
            radioButtonBlueComputer.Name = "radioButtonBlueComputer";
            radioButtonBlueComputer.Size = new Size(96, 24);
            radioButtonBlueComputer.TabIndex = 1;
            radioButtonBlueComputer.Text = "Computer";
            // groupBoxBlueLetter
            groupBoxBlueLetter.Controls.Add(radioButton3);
            groupBoxBlueLetter.Controls.Add(radioButton4);
            groupBoxBlueLetter.Location = new Point(48, 333);
            groupBoxBlueLetter.Name = "groupBoxBlueLetter";
            groupBoxBlueLetter.Size = new Size(120, 70);
            groupBoxBlueLetter.TabIndex = 8;
            groupBoxBlueLetter.TabStop = false;
            groupBoxBlueLetter.Text = "Human Letter Move";
            // radioButton3
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(10, 20);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(38, 24);
            radioButton3.TabIndex = 0;
            radioButton3.Text = "S";
            // radioButton4
            radioButton4.AutoSize = true;
            radioButton4.Location = new Point(10, 40);
            radioButton4.Name = "radioButton4";
            radioButton4.Size = new Size(41, 24);
            radioButton4.TabIndex = 1;
            radioButton4.Text = "O";
            // label4
            label4.AutoSize = true;
            label4.Font = new Font("Rockwell", 12F, FontStyle.Bold);
            label4.ForeColor = Color.IndianRed;
            label4.Location = new Point(791, 188);
            label4.Name = "label4";
            label4.Size = new Size(118, 24);
            label4.TabIndex = 9;
            label4.Text = "Red Player";
            // groupBoxRedPlayerType
            groupBoxRedPlayerType.Controls.Add(radioButtonRedHuman);
            groupBoxRedPlayerType.Controls.Add(radioButtonRedComputer);
            groupBoxRedPlayerType.Location = new Point(791, 230);
            groupBoxRedPlayerType.Name = "groupBoxRedPlayerType";
            groupBoxRedPlayerType.Size = new Size(120, 70);
            groupBoxRedPlayerType.TabIndex = 10;
            groupBoxRedPlayerType.TabStop = false;
            groupBoxRedPlayerType.Text = "Player Type";
            // radioButtonRedHuman
            radioButtonRedHuman.AutoSize = true;
            radioButtonRedHuman.Location = new Point(10, 20);
            radioButtonRedHuman.Name = "radioButtonRedHuman";
            radioButtonRedHuman.Size = new Size(78, 24);
            radioButtonRedHuman.TabIndex = 0;
            radioButtonRedHuman.Text = "Human";
            // radioButtonRedComputer
            radioButtonRedComputer.AutoSize = true;
            radioButtonRedComputer.Location = new Point(10, 40);
            radioButtonRedComputer.Name = "radioButtonRedComputer";
            radioButtonRedComputer.Size = new Size(96, 24);
            radioButtonRedComputer.TabIndex = 1;
            radioButtonRedComputer.Text = "Computer";
            // groupBoxRedLetter
            groupBoxRedLetter.Controls.Add(radioButton5);
            groupBoxRedLetter.Controls.Add(radioButton6);
            groupBoxRedLetter.Location = new Point(791, 333);
            groupBoxRedLetter.Name = "groupBoxRedLetter";
            groupBoxRedLetter.Size = new Size(120, 70);
            groupBoxRedLetter.TabIndex = 11;
            groupBoxRedLetter.TabStop = false;
            groupBoxRedLetter.Text = "Human Letter Move";
            // radioButton5
            radioButton5.AutoSize = true;
            radioButton5.Location = new Point(10, 20);
            radioButton5.Name = "radioButton5";
            radioButton5.Size = new Size(38, 24);
            radioButton5.TabIndex = 0;
            radioButton5.Text = "S";
            // radioButton6
            radioButton6.AutoSize = true;
            radioButton6.Location = new Point(10, 40);
            radioButton6.Name = "radioButton6";
            radioButton6.Size = new Size(41, 24);
            radioButton6.TabIndex = 1;
            radioButton6.Text = "O";
            // panel1
            panel1.Location = new Point(220, 150);
            panel1.Name = "panel1";
            panel1.Size = new Size(500, 330);
            panel1.TabIndex = 12;
            // labelBlueScore
            labelBlueScore.AutoSize = true;
            labelBlueScore.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelBlueScore.Location = new Point(67, 414);
            labelBlueScore.Name = "labelBlueScore";
            labelBlueScore.Size = new Size(69, 28);
            labelBlueScore.TabIndex = 13;
            labelBlueScore.Text = "Blue: 0";
            // labelRedScore
            labelRedScore.AutoSize = true;
            labelRedScore.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelRedScore.Location = new Point(814, 429);
            labelRedScore.Name = "labelRedScore";
            labelRedScore.Size = new Size(65, 28);
            labelRedScore.TabIndex = 14;
            labelRedScore.Text = "Red: 0"; 
            // label5
            label5.AutoSize = true;
            label5.Font = new Font("MV Boli", 20F, FontStyle.Bold);
            label5.Location = new Point(256, 500);
            label5.Name = "label5";
            label5.Size = new Size(369, 45);
            label5.TabIndex = 15;
            label5.Text = "Current Player: Blue";
            // buttonNewGame
            buttonNewGame.Location = new Point(777, 510);
            buttonNewGame.Name = "buttonNewGame";
            buttonNewGame.Size = new Size(132, 61);
            buttonNewGame.TabIndex = 16;
            buttonNewGame.Text = "New Game";
            // buttonStartComputerGame
            buttonStartComputerGame.Location = new Point(332, 107);
            buttonStartComputerGame.Name = "buttonStartComputerGame";
            buttonStartComputerGame.Size = new Size(281, 37);
            buttonStartComputerGame.TabIndex = 17;
            buttonStartComputerGame.Text = "Start Computer Game";
            // new track bar for board sizing
            trackBarSize = new TrackBar();
            trackBarSize.Minimum = 3;
            trackBarSize.Maximum = 10;
            trackBarSize.Value = 3;
            trackBarSize.TickStyle = TickStyle.BottomRight;
            trackBarSize.SmallChange = 1;
            trackBarSize.LargeChange = 1;
            trackBarSize.Width = 150;
            trackBarSize.Location = new Point(textBox1.Left, textBox1.Top - 8);
            trackBarSize.ValueChanged += (s, e) => { textBox1.Text = trackBarSize.Value.ToString(); };
            Controls.Add(trackBarSize);
            // different color theme buttons
            btnLightTheme = new Button { Text = "Light", Size = new Size(70, 28), Location = new Point(270, 545) };
            btnDarkTheme = new Button { Text = "Dark", Size = new Size(70, 28), Location = new Point(360, 545) };
            btnPinkTheme = new Button { Text = "Pink", Size = new Size(70, 28), Location = new Point(450, 545) };
            Controls.Add(btnLightTheme);
            Controls.Add(btnDarkTheme);
            Controls.Add(btnPinkTheme);
            // record
            chkRecordGame = new CheckBox();
            chkRecordGame.AutoSize = true;
            chkRecordGame.Location = new Point(20, 470); // bottom-left
            chkRecordGame.Name = "chkRecordGame";
            chkRecordGame.Size = new Size(120, 24);
            chkRecordGame.TabIndex = 18;
            chkRecordGame.Text = "Record Game";
            chkRecordGame.UseVisualStyleBackColor = true;
            // replay
            btnReplay = new Button();
            btnReplay.Location = new Point(135, 465); // next to checkbox
            btnReplay.Name = "btnReplay";
            btnReplay.Size = new Size(75, 30);
            btnReplay.TabIndex = 19;
            btnReplay.Text = "Replay";
            btnReplay.UseVisualStyleBackColor = true;
            btnReplay.Visible = false; // hidden until a game is recorded
            //difficulty level buttons
            btnEasy = new Button { Text = "Easy", Size = new Size(70, 28), Location = new Point(122, 70) };
            btnMedium = new Button { Text = "Medium", Size = new Size(70, 28), Location = new Point(202, 70) };
            btnHard = new Button { Text = "Hard", Size = new Size(70, 28), Location = new Point(282, 70) };
            // Form1
            ClientSize = new Size(950, 600);
            Controls.Add(label1);
            Controls.Add(radioButton1);
            Controls.Add(radioButton2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label3);
            Controls.Add(groupBoxBluePlayerType);
            Controls.Add(groupBoxBlueLetter);
            Controls.Add(label4);
            Controls.Add(groupBoxRedPlayerType);
            Controls.Add(groupBoxRedLetter);
            Controls.Add(panel1);
            Controls.Add(labelBlueScore);
            Controls.Add(labelRedScore);
            Controls.Add(label5);
            Controls.Add(buttonNewGame);
            Controls.Add(buttonStartComputerGame);
            Name = "Form1";
            Text = "SOS Game";
            groupBoxBluePlayerType.ResumeLayout(false);
            groupBoxBluePlayerType.PerformLayout();
            groupBoxBlueLetter.ResumeLayout(false);
            groupBoxBlueLetter.PerformLayout();
            groupBoxRedPlayerType.ResumeLayout(false);
            groupBoxRedPlayerType.PerformLayout();
            groupBoxRedLetter.ResumeLayout(false);
            groupBoxRedLetter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
            Controls.Add(chkRecordGame);
            Controls.Add(btnReplay);
            Controls.Add(btnEasy);
            Controls.Add(btnMedium);
            Controls.Add(btnHard);

        }
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxBluePlayerType;
        private System.Windows.Forms.RadioButton radioButtonBlueHuman;
        private System.Windows.Forms.RadioButton radioButtonBlueComputer;
        private System.Windows.Forms.GroupBox groupBoxBlueLetter;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxRedPlayerType;
        private System.Windows.Forms.RadioButton radioButtonRedHuman;
        private System.Windows.Forms.RadioButton radioButtonRedComputer;
        private System.Windows.Forms.GroupBox groupBoxRedLetter;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelBlueScore;
        private System.Windows.Forms.Label labelRedScore;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonNewGame;
        private System.Windows.Forms.Button buttonStartComputerGame;
        private System.Windows.Forms.TrackBar trackBarSize;
        private System.Windows.Forms.Button btnLightTheme;
        private System.Windows.Forms.Button btnDarkTheme;
        private System.Windows.Forms.Button btnPinkTheme;
        private System.Windows.Forms.CheckBox chkRecordGame;
        private System.Windows.Forms.Button btnReplay;
        private System.Windows.Forms.Button btnEasy;
        private System.Windows.Forms.Button btnMedium;
        private System.Windows.Forms.Button btnHard;
    }
}
