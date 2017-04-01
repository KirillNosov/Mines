using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace MineSweeper
{
    static class Drawer
    {
        public static Bitmap MinesAround(int count)
        {
            switch (count)
            {
                case 0:
                    return Properties.Resources._0;
                case 1:
                    return Properties.Resources._1;
                case 2:
                    return Properties.Resources._2;
                case 3:
                    return Properties.Resources._3;
                case 4:
                    return Properties.Resources._4;
                case 5:
                    return Properties.Resources._5;
                case 6:
                    return Properties.Resources._6;
                case 7:
                    return Properties.Resources._7;
                case 8:
                    return Properties.Resources._8;
            }
            return null;
        }

        public static Bitmap Mine()
        {
            return Properties.Resources.mine;
        }

        public static Bitmap Flag()
        {
            return Properties.Resources.flag;
        }

        public static Bitmap MineError()
        {
            return Properties.Resources.mine_err;
        }

        public static Bitmap Bang()
        {
            return Properties.Resources.blowUp;
        }
    }
}
