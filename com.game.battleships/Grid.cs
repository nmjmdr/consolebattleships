using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.battleships
{
    public class Grid
    {
        private Cell[,] cells;
        private List<Cell> freeCells;
        private int size;

        public delegate void CellHitHandler(Cell cell);
        public event CellHitHandler CellHit;
       
        public Grid(int size)
        {
            this.size = size;
            cells = new Cell[size, size];
            freeCells = new List<Cell>();

            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    cells[row, col] = new Cell(row, col);
                    freeCells.Add(cells[row, col]);
                }
            }
        }

        public List<Cell> FreeCells
        {
            get
            {
                return freeCells;
            }
        }

        
        public void MarkCells(IEnumerable<Cell> selectedCells)
        {
            // block the selected cells            
            foreach (Cell c in selectedCells)
            {
                this.FreeCells.Remove(c);
                c.IsUsed = true;
            }
        }

        public void Hit(int row, int col)
        {
            if( (row < 0 || row > this.Size) || (col < 0 || col > this.Size)) {
                // wrong input, ignore
                return;
            }
            // mark the cell as hit
            this.Cells[row, col].Hit();
            this.CellHit(this.Cells[row, col]);       
        }

        

        public Cell[,] Cells
        {
            get
            {
                return cells;
            }
        }

        public int Size
        {
            get
            {
                return this.size;
            }
        }

        public void UnSubscribe(CellHitHandler cellHitHandler)
        {
            CellHit -= cellHitHandler;
        }
    }
}
