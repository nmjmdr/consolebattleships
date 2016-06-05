using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.game.battleships;
using Moq;
using System.Threading;

namespace com.game.tests
{
    [TestClass]
    public class BoardTests
    {
        private AutoResetEvent hitResultAutoEvent;
        private HitResult hitResult;

        private AutoResetEvent targetDestroyedAutoEvent;
        private ITarget destroyedTarget;
        [TestMethod]
        public void TestPlacementInGrid()
        {
            Mock<IPlacementStrategy> firstRowStrategy = new Mock<IPlacementStrategy>();

            Grid grid = new Grid(10);
            firstRowStrategy.Setup(c => c.FindPlacement(It.IsAny<Target>())).Returns(new Cell[] { grid.Cells[0, 0], grid.Cells[0, 1], grid.Cells[0, 2], grid.Cells[0, 3] } );

            
            Board board = new Board(grid,firstRowStrategy.Object);
            Target[] targets = new Target[1];
            targets[0] = new Destroyer();
            board.Start(targets);

            // check if its has been placed
            Assert.IsNotNull(board.Grid.Cells[0, 0].Target);
        }

        [TestMethod]
        public void TestHit()
        {
            HitResultType type = testShot(0, 0);
            Assert.IsTrue(type == HitResultType.Hit);
            hitResult = null;
        }

        [TestMethod]
        public void TestMiss()
        {
            HitResultType type = testShot(2, 2);
            Assert.IsTrue(type == HitResultType.Hit);
            hitResult = null;
        }

        [TestMethod]
        public void TestAlreadyHit()
        {
            Mock<IPlacementStrategy> firstRowStrategy = new Mock<IPlacementStrategy>();

            Grid grid = new Grid(10);
            firstRowStrategy.Setup(c => c.FindPlacement(It.IsAny<Target>())).Returns(new Cell[] { grid.Cells[0, 0], grid.Cells[0, 1], grid.Cells[0, 2], grid.Cells[0, 3] });


            Board board = new Board(grid, firstRowStrategy.Object);
            Target[] targets = new Target[1];
            targets[0] = new Destroyer();
            board.Start(targets);

            hitResultAutoEvent = new AutoResetEvent(false);
            board.HitResultEvent += HitResultEvent;
            board.Hit(0, 0);
            hitResultAutoEvent.WaitOne();
            board.Hit(0, 0);
            Assert.IsTrue(hitResult.Type == HitResultType.AlreadyHit);
            hitResult = null;
        }

        [TestMethod]
        public void TestDestroyed()
        {
            Mock<IPlacementStrategy> firstRowStrategy = new Mock<IPlacementStrategy>();

            Grid grid = new Grid(10);
            firstRowStrategy.Setup(c => c.FindPlacement(It.IsAny<Target>())).Returns(new Cell[] { grid.Cells[0, 0], grid.Cells[0, 1], grid.Cells[0, 2], grid.Cells[0, 3] });

            targetDestroyedAutoEvent = new AutoResetEvent(false);

            Board board = new Board(grid, firstRowStrategy.Object);
            Target[] targets = new Target[1];
            targets[0] = new Destroyer();
            board.Start(targets);
            board.TargetDestroyedEvent += TargetDestroyedEvent;

            for (int i = 0; i < 4; i++)
            {
                board.Hit(0, i);
            }
            targetDestroyedAutoEvent.WaitOne();

            Assert.IsTrue(destroyedTarget != null);
            destroyedTarget = null;
        }

        private void TargetDestroyedEvent(ITarget target)
        {
            destroyedTarget = target;
            targetDestroyedAutoEvent.Set();
        }

        public HitResultType testShot(int row,int col) 
        {
            Mock<IPlacementStrategy> firstRowStrategy = new Mock<IPlacementStrategy>();

            Grid grid = new Grid(10);
            firstRowStrategy.Setup(c => c.FindPlacement(It.IsAny<Target>())).Returns(new Cell[] { grid.Cells[0, 0], grid.Cells[0, 1], grid.Cells[0, 2], grid.Cells[0, 3] });


            Board board = new Board(grid, firstRowStrategy.Object);
            Target[] targets = new Target[1];
            targets[0] = new Destroyer();
            board.Start(targets);

            hitResultAutoEvent = new AutoResetEvent(false);
            board.HitResultEvent += HitResultEvent;
            board.Hit(0, 0);
            hitResultAutoEvent.WaitOne();
            return hitResult.Type;
        }

        private void HitResultEvent(HitResult hitResult)
        {
            this.hitResult = hitResult;
            hitResultAutoEvent.Set();
        }

        
    }
}
