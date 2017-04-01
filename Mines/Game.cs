using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper
{
    class Game
    {
        protected int rowNum;
        protected int colNum;
        protected int mineNum;
        protected int minesLeft;
        protected Cell[,] cells;
        protected int openedCells;
        protected bool started;
        protected bool finished;
        protected bool won;

        public Game(int rowNum, int colNum, int mineNum)
        {
            cells = new Cell[rowNum, colNum];
            for (int i = 0; i < rowNum; i++)
                for (int j = 0; j < colNum; j++)
                    cells[i, j] = new Cell();
            this.rowNum = rowNum;
            this.colNum = colNum;
            this.mineNum = mineNum;
            minesLeft = mineNum;
            openedCells = 0;
            started = false;
            finished = false;
        }

        public Cell GetCell(int i, int j)
        {
            return (cells[i, j]);
        }

        public void SetMineFlag(int xCell, int yCell)
        {
            if (cells[xCell, yCell].SetMineMark())
                minesLeft--;
            else
                minesLeft++;
        }

        public int RowNum
        {
            get { return rowNum; }
        }

        public int ColNum
        {
            get { return colNum; }
        }

        public int MinesNum
        {
            get{return mineNum;}
        }

        public int MinesLeft
        {
            get{return minesLeft;}
        }

        public bool IsStarted
        {
            get { return started; }
        }

        public bool IsFinished
        {
            get { return finished; }
        }

        public bool IsWon
        {
            get { return won; }
        }

        public virtual void CheckCell(int xCell, int yCell)
        {
            if (!started)
                Start(xCell, yCell);
            OpenCell(xCell, yCell);
        }

        protected virtual void Start(int xStart, int yStart)
        {
            started = true;
            LayMines(xStart, yStart);
            InitBoard();
        }

        protected virtual void LayMines(int xStart, int yStart)
        {
            int x;
            int y;
            int laidMines = 0;
            Random random = new Random();
            do
            {            
                x = (int)(random.NextDouble() * rowNum);
                y = (int)(random.NextDouble() * colNum);
                if ((!cells[x, y].IsMined) && ((x != xStart) || (y != yStart)))
                {
                    cells[x, y].SetMine();
                    laidMines++;
                }
            }
            while (laidMines < mineNum);
        }

        protected virtual void InitBoard()
        {
            for (int i = 0; i < rowNum; i++)
                for (int j = 0; j < colNum; j++)
                    cells[i, j].SetMinesAround(GetNumOfMinesAroundCell(i, j));
        }

        protected virtual int GetNumOfMinesAroundCell(int xCell, int yCell)
        {
            int minesAround = 0;
            for (int i = xCell - 1; i <= xCell + 1; i++)
                for (int j = yCell - 1; j <= yCell + 1; j++)
                    try
                    {
                        if (cells[i, j].IsMined)
                            minesAround++;
                    }
                    catch (IndexOutOfRangeException)
                    {

                    }
            return (minesAround - (cells[xCell, yCell].IsMined ? 1 : 0));
        }

        protected virtual void OpenCell(int xCell, int yCell)
        {
            if (cells[xCell, yCell].IsMined)
            {
                cells[xCell, yCell].Open();
                cells[xCell, yCell].Bang();
                FinishGame(false);
            }
            else
                ShowCell(xCell, yCell);
        }

        protected virtual void ShowCell(int xCell, int yCell)
        {
            cells[xCell, yCell].Open();
            openedCells++;
            if (openedCells < (rowNum * colNum - mineNum))
            {
                if (cells[xCell, yCell].MinesAround == 0)
                    for (int i = xCell - 1; i <= xCell + 1; i++)
                        for (int j = yCell - 1; j <= yCell + 1; j++)
                            try
                            {
                                if (!cells[i, j].IsOpened && !cells[i, j].IsMineMark)
                                    ShowCell(i, j);
                            }
                            catch (IndexOutOfRangeException)
                            {

                            }
            }
            else
            {
                FinishGame(true);
            }
        }

        protected virtual void FinishGame(bool won)
        {
            this.won = won;
            started = false;
            finished = true;
        }
    }
}
