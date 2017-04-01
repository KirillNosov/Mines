using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Mines : Form
    {
        const int INDENT = 25;
        Game game;   
        int seconds;

        public Mines()
        {
            InitializeComponent();
        }

        private void myControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!game.IsFinished)
            {
                Point cell = minesFieldControl.IndexesCell(e.X, e.Y);
                timer1.Enabled = true;
                if (e.Button == MouseButtons.Left)
                {
                    if (!game.GetCell(cell.X, cell.Y).IsOpened && !game.GetCell(cell.X, cell.Y).IsMineMark)
                    {
                        game.CheckCell(cell.X, cell.Y);
                        DrawMinesField();
                    }

                    if (game.IsFinished)
                    {
                        timer1.Enabled = false;
                        seconds = 0;
                        if (game.IsWon)
                        {
                            newGameButton.Image = new Bitmap(Properties.Resources.bear_won, newGameButton.Width, newGameButton.Height);
                            label1.Text = "Мин осталось: 0";
                        }
                        else if (!game.IsWon)
                        {
                            newGameButton.Image = new Bitmap(Properties.Resources.bear_lost, newGameButton.Width, newGameButton.Height);
                        }
                    }
                    else
                    {
                        label1.Text = "Мин осталось: " + game.MinesLeft;
                        newGameButton.Image = new Bitmap(Properties.Resources.bear, newGameButton.Width, newGameButton.Height);
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (!game.GetCell(cell.X, cell.Y).IsOpened)
                    {
                        game.SetMineFlag(cell.X, cell.Y);
                        if (game.GetCell(cell.X, cell.Y).IsMineMark)
                        {
                            minesFieldControl.Cells(cell.X, cell.Y).Picture = Drawer.Flag();
                        }
                        else
                        {
                            minesFieldControl.Cells(cell.X, cell.Y).Picture = null;
                        }
                        label1.Text = "Мин осталось: " + game.MinesLeft;
                    }
                }

            }
        }

        private void DrawMinesField()
        {
            for (int i = 0; i < game.RowNum; i++)
                for (int j = 0; j < game.ColNum; j++)
                {
                    if (!game.GetCell(i, j).IsOpened)
                    {
                        if (game.IsFinished)
                        {
                            if (game.GetCell(i, j).IsMineMark && !game.GetCell(i, j).IsMined)
                            {
                                minesFieldControl.Cells(i, j).Picture = Drawer.MineError();
                            }
                            else if (game.IsWon && game.GetCell(i, j).IsMined)
                                minesFieldControl.Cells(i, j).Picture = Drawer.Flag();

                            else if (!game.IsStarted && !game.GetCell(i, j).IsBlownUp && game.GetCell(i, j).IsMined)
                            {
                                if (!game.IsWon && !game.GetCell(i, j).IsMineMark)
                                {
                                    minesFieldControl.Cells(i, j).Picture = Drawer.Mine();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (game.GetCell(i, j).IsMined)
                        {
                            minesFieldControl.Cells(i, j).Picture = Drawer.Bang();
                        }
                        else
                        {
                            minesFieldControl.Cells(i, j).Picture = Drawer.MinesAround(game.GetCell(i, j).MinesAround);
                        }
                    }
                }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            newGameButton_Click(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds++;
            label2.Text = "Время: " + seconds;
        }

        private void minesFieldControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!game.IsFinished)
                    newGameButton.Image = new Bitmap(Properties.Resources.bear_click, newGameButton.Width, newGameButton.Height);
            }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {            
            if (timer1.Enabled)
                timer1.Enabled = false;

            newGameButton.Image = new Bitmap(Properties.Resources.bear, newGameButton.Width, newGameButton.Height);
            game = new Game(Level.rowCount, Level.colCount, Level.colMines);

            label1.Text = "Мин осталось: " + game.MinesLeft;
            label2.Text = "Время: ";

            Width = minesFieldControl.SizeCell * Level.colCount + 2 * INDENT;
            Height = minesFieldControl.SizeCell * Level.rowCount + minesFieldControl.Top + 2 * INDENT;

            minesFieldControl.Clear();
            minesFieldControl.ColCount = Level.colCount;
            minesFieldControl.RowCount = Level.rowCount;

            minesFieldControl.Left = INDENT;
            int i = minesFieldControl.Right;

            label1.Top = minesFieldControl.Bottom;
            label1.Left = minesFieldControl.Right - label1.Width;
            label2.Top = minesFieldControl.Bottom;
            label2.Left = INDENT;

            newGameButton.Left = (int)(Width / 2 - newGameButton.Width / 2);
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!easyToolStripMenuItem.Checked)
            {
                easyToolStripMenuItem.Checked = true;
                mediumToolStripMenuItem.Checked = false;
                hardToolStripMenuItem.Checked = false;
                Level.colMines = 10;
                Level.colCount = 9;
                Level.rowCount = 9;
            }
            newGameButton_Click(sender, e);
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!mediumToolStripMenuItem.Checked)
            {
                easyToolStripMenuItem.Checked = false;
                mediumToolStripMenuItem.Checked = true;
                hardToolStripMenuItem.Checked = false;
                Level.colMines = 40;
                Level.colCount = 16;
                Level.rowCount = 16;
            }
            newGameButton_Click(sender, e);
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!hardToolStripMenuItem.Checked)
            {
                easyToolStripMenuItem.Checked = false;
                mediumToolStripMenuItem.Checked = false;
                hardToolStripMenuItem.Checked = true;
                Level.colMines = 99;
                Level.colCount = 30;
                Level.rowCount = 16;
            }
            newGameButton_Click(sender, e);
        }
    }

    static class Level
    {        
        public static int colMines = 10;
        public static int rowCount = 9;
        public static int colCount = 9;
    }
}
