using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.battleships
{
    public class Board
    {
        private Grid grid;
        private Target[] targets;
        private const int GridSize = 10;
        
        private Placer placer;
        private IPlacementStrategy placementStrategy;



        public delegate void HitResultHandler(HitResult hitResult);
        public event HitResultHandler HitResultEvent;

        public delegate void TargetDestroyedHandler(ITarget target);
        public event TargetDestroyedHandler TargetDestroyedEvent;

        public Grid Grid
        {
            get
            {
                return grid;
            }
        }

        public Board() 
        {
            this.grid = new Grid(GridSize);
            this.placementStrategy = new RandomPlacementStrategy(this.grid);
            this.placer = new Placer(grid, this.placementStrategy);
        }
        
        // mainly to test
        public Board(Grid grid,IPlacementStrategy strategy)
        {
            this.grid = grid;
            this.placementStrategy = strategy;
            this.placer = new Placer(grid, this.placementStrategy);
        }

        public void init(Target[] targets)
        {
           
            this.targets = targets;
            
           
        }

        private void setup(Target[] targets)
        {
            init(targets);
            grid.CellHit += cellHitHandler;
            foreach(Target target in targets)
            {                
                placer.Place(grid, target);
                target.TargetDestroyed += onTargetDestroyed;
            }

        }

        private void onTargetDestroyed(ITarget destroyedTarget)
        {
            TargetDestroyedEvent?.Invoke(destroyedTarget);
        }

        private void cellHitHandler(Cell cell)
        {
            if(cell.HitCount > 1)
            {
                // has been hit more than once, user hit a cell which was already hit  
                HitResultEvent?.Invoke(new HitResult { Type = HitResultType.AlreadyHit, Target = null });
                return;
            }           

            if(cell.Target == null)
            {
                // the cell did not contain a target, so a miss
                HitResultEvent?.Invoke(new HitResult { Type = HitResultType.Missed, Target = null });
                return;
            }

            
            HitResultEvent?.Invoke(new HitResult { Type = HitResultType.Hit, Target = cell.Target });

            // it has hit a target, tell the target it was hit
            cell.Target.HandleHit(new HitEvent { CellThatWasHit = cell });
        }

        public void Start(Target[] targets)
        {
            setup(targets);
        }

        public void Hit(int row,int col)
        {
            grid.Hit(row, col);
        }

        public void Shutdown()
        {
            grid.UnSubscribe(cellHitHandler);
            foreach(Target target in targets)
            {
                target.TargetDestroyed -= onTargetDestroyed;
            }
        }

        
    }

    
}
