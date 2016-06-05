using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.game.battleships;
using System.Threading;

namespace com.game.tests
{
    [TestClass]
    public class TargetTests
    {
        private AutoResetEvent targetDestroyedAutoEvent;
        private ITarget destroyedTarget;

        [TestMethod]
        public void TargetSetTest()
        {
            int gridSize = 4;
            Grid grid = new Grid(gridSize);

            Target target = new Destroyer();
            Cell[] selectedCells = new Cell[target.BlockSize];
            for(int i=0;i<target.BlockSize;i++)
            {
                selectedCells[i] = grid.Cells[0, i];
            }

            target.SetAt(selectedCells);

            for (int i = 0; i < target.BlockSize; i++) {
                Assert.IsTrue(grid.Cells[0, i].Target != null);
            }
        }


        [TestMethod]
        public void TargetDestroyedTest()
        {
            int gridSize = 4;
            Grid grid = new Grid(gridSize);

            Target target = new Destroyer();
            Cell[] selectedCells = new Cell[target.BlockSize];
            for (int i = 0; i < target.BlockSize; i++)
            {
                selectedCells[i] = grid.Cells[0, i];
            }

            target.SetAt(selectedCells);

            targetDestroyedAutoEvent = new AutoResetEvent(false);

            target.TargetDestroyed += TargetDestroyed;

            foreach (Cell cell in selectedCells) {
                cell.Hit();
                target.HandleHit(new HitEvent { CellThatWasHit = cell });           
            }
            targetDestroyedAutoEvent.WaitOne();

            Assert.IsTrue(destroyedTarget == target);
        }

        private void TargetDestroyed(ITarget destroyedTarget)
        {
            this.destroyedTarget = destroyedTarget;
            targetDestroyedAutoEvent.Set();
        }
    }
}
