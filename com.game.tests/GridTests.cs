using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.game.battleships;
using static com.game.battleships.Grid;
using System.Threading;

namespace com.game.tests
{
    [TestClass]
    public class GridTests
    {       
        private Cell theCellHit;
        private AutoResetEvent cellHitAutoEvent;

        [TestMethod]
        public void GridTestCellsMarked()
        {
            int gridSize = 2;
            int numberOfCells = gridSize * gridSize;
            Grid grid = new Grid(gridSize);
            Cell[] cells = new Cell[1] { grid.Cells[0, 0] };

            grid.MarkCells(cells);

            Assert.IsTrue(grid.Cells[0, 0].IsUsed);
            Assert.IsTrue(grid.FreeCells.Count == numberOfCells - 1);

        }


        [TestMethod]
        public void GridTestHit()
        {
            int gridSize = 2;
            int numberOfCells = gridSize * gridSize;
            Grid grid = new Grid(gridSize);
            Cell[] cells = new Cell[1] { grid.Cells[0, 0] };

            cellHitAutoEvent = new AutoResetEvent(false);
            grid.CellHit += cellHit;
            grid.Hit(0, 0);

            Assert.IsTrue(theCellHit != null);
            Assert.IsTrue((theCellHit.Row == 0 && theCellHit.Col == 0));

        }

        private void cellHit(Cell cell)
        {
            theCellHit = cell;
            cellHitAutoEvent.Set();
        }
    }
}
