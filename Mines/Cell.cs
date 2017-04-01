using System;
using System.Collections.Generic;
using System.Text;

namespace MineSweeper
{
    class Cell
    {
        protected bool mined;
        protected bool opened;
        protected bool markedAsMine;
        protected bool blownUp;
        protected int numMinesAround;

        public Cell()
        {
            mined = false;
            opened = false;
            markedAsMine = false;
            blownUp = false;
            numMinesAround = 0;
        }

        public void SetMine()
        {
            mined = true;
        }

        public bool IsMined
        {
            get { return mined; }
        }

        public void Open()
        {
            opened = true;
        }

        public bool IsOpened
        {
            get{return opened;}
        }

        public bool SetMineMark()
        {
            markedAsMine = !markedAsMine;
            return markedAsMine;
        }

        public bool IsMineMark
        {
            get { return (markedAsMine); }
        }

        public void SetMinesAround(int num)
        {
            numMinesAround = num;
        }

        public int MinesAround
        {
            get { return (numMinesAround); }
        }

        public virtual void Bang()
        {
            blownUp = true;
        }

        public bool IsBlownUp
        {
            get { return blownUp; }
        }
    }
}
