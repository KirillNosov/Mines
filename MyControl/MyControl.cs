using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MyControl
{
    public partial class MyControl : UserControl
    {
        private Cell[,] cells;
        private bool clickCell = false;
        private Point activeCell;
        private Point clickedCell;
        private int colCount = 9;
        private int rowCount = 9;
        private int sizeCell = 20;
        private Color activeCellColor = Color.Red;
        private Color clickCellColor = Color.DarkGray;
        private Color borderColor = Color.Black;

        [Category("Appearance"), Description("Цвет подсвечивания активной ячейки"), DefaultValue(typeof(Color), "Red")]
        public Color ActiveCellColor
        {
            get { return activeCellColor; }
            set
            {
                activeCellColor = value;
            }
        }

        [Category("Appearance"), Description("Цвет выделения ячейки при клике"), DefaultValue(typeof(Color), "DarkGray")]
        public Color ClickCellColor
        {
            get { return clickCellColor; }
            set
            {
                clickCellColor = value;
            }
        }

        [Category("Appearance"), Description("Цвет границ ячеек"), DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                OnChangeProperties();
            }
        }

        [Category("Appearance"), Description("Размер ячейки"), DefaultValue(20)]
        public int SizeCell
        {
            get
            {
                return sizeCell;
            }
            set
            {
                sizeCell = value;
                InitCells();
                OnChangeProperties();
            }
        }

        [Category("Appearance"), Description("Количество ячеек по горизонтали"), DefaultValue(9)]
        public int ColCount
        {
            get { return colCount; }
            set
            {
                if (colCount != value)
                {
                    colCount = value;
                    InitCells();
                    OnChangeProperties();
                }
            }
        }

        [Category("Appearance"), Description("Количество ячеек по вертикали"), DefaultValue(9)]
        public int RowCount
        {
            get { return rowCount; }
            set
            {
                if (rowCount != value)
                {
                    rowCount = value;
                    InitCells();
                    OnChangeProperties();
                }
            }
        }

        public Cell Cells(int i, int j)
        {
            return cells[i, j];
        }

        protected virtual void InitCells()
        {
            activeCell.X = -1;
            activeCell.Y = -1;
            cells = new Cell[rowCount, colCount];
            this.Width = (SizeCell - 1) * ColCount + 1;
            this.Height = (SizeCell - 1) * RowCount + 1;
            int y = 0;
            int x;
            for (int i = 0; i < RowCount; i++)
            {
                x = 0;
                for (int j = 0; j < ColCount; j++)
                {
                    cells[i, j] = new Cell();
                    cells[i, j].Position = new Point(x, y);
                    x += SizeCell - 1;
                }
                y += SizeCell - 1;
            }
        }

        public MyControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            InitCells();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            foreach (Cell cell in cells)
            {
                e.Graphics.DrawRectangle(new Pen(BorderColor), new Rectangle(cell.Position.X, cell.Position.Y, SizeCell - 1, SizeCell - 1));
                if (cell.Picture != null)
                {
                    e.Graphics.DrawImage(cell.Picture, cell.Position.X + 1, cell.Position.Y + 1, sizeCell - 2, sizeCell - 2);
                }
            }

            if (activeCell.X >= 0 && activeCell.Y >= 0)
            {
                e.Graphics.DrawRectangle(new Pen(ActiveCellColor), new Rectangle(activeCell.X, activeCell.Y, SizeCell - 1, SizeCell - 1));
            }
            if (clickCell)
            {
                e.Graphics.FillRectangle(new SolidBrush(ClickCellColor), clickedCell.X + 1, clickedCell.Y + 1, SizeCell - 2, SizeCell - 2);
            }
        }

        private void OnChangeProperties()
        {
            Invalidate();
        }

        public virtual Cell GetCellByPoint(Point point)
        {
            point = GetPositionCell(point.X, point.Y);
            foreach (Cell cell in cells)
            {
                if (cell.Position == point)
                    return cell;
            }
            return null;
        }

        public virtual Point IndexesCell(int x, int y)
        {
            Point point = GetPositionCell(x, y);
            int temp = point.Y;
            point.Y = point.X / (SizeCell - 1);
            point.X = temp / (SizeCell - 1);
            return point;
        }

        protected virtual Point GetPositionCell(int x, int y)
        {
            int X = 0;
            while (Math.Abs(X - x) > SizeCell)
            {
                X += SizeCell - 1;
            }

            int Y = 0;
            while (Math.Abs(Y - y) > SizeCell)
            {
                Y += SizeCell - 1;
            }
            return (new Point(X, Y));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.X >= 0 && e.X <= Width && e.Y >= 0 && e.Y <= Height)
            {
                if (GetPositionCell(e.X, e.Y) != activeCell)
                {
                    activeCell = GetPositionCell(e.X, e.Y);
                    if (clickCell)
                    {
                        clickedCell = activeCell;
                    }
                    OnChangeProperties();
                }
            }
            else
                OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                {
                    clickedCell = GetPositionCell(e.X, e.Y);
                    clickCell = true;
                    OnChangeProperties();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.X >= 0 && e.X <= Width && e.Y >= 0 && e.Y <= Height)
            {
                base.OnMouseUp(e);
                clickCell = false;
                OnChangeProperties();

            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            activeCell = new Point(-1, -1);
            clickedCell = new Point(-1, -1);
            clickCell = false;
            OnChangeProperties();
        }

        public virtual void Clear()
        {
            for (int i = 0; i < RowCount; i++)
                for (int j = 0; j < ColCount; j++)
                    Cells(i, j).Picture = null;
            OnChangeProperties();
        }
    }
    public class Cell
    {
        private Bitmap picture;
        private Point position;

        public Bitmap Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }
    }

}
