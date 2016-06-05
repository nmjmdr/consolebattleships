using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.battleships
{
    public abstract class Target : ITarget
    {
        public int BlockSize { private set; get; }
        public Orientation Orientation { private set; get; }

        public TargetState State { get; private set; }
        
        private bool alreadySet = false;
        public int HitCount { get; private set; }

        public delegate void TargetDestoyedHandler(ITarget destroyedTarget);
        public event TargetDestoyedHandler TargetDestroyed;

        public abstract string Name
        {
            get;            
        }

        public abstract int Points
        {
            get;
        }

        

        private Cell[] shipCells;

        public Target(int blockSize,Orientation orientation)
        {
            this.BlockSize = blockSize;                    
            this.Orientation = orientation;
            State = TargetState.Standing;
            
        }

        public Target(int blockSize) : this(blockSize,Orientation.Horizontal)
        {            
        }

        public void SetAt(Cell[] cells)
        {
            // set self as the target
            shipCells = cells;
            foreach(Cell cell in cells)
            {
                cell.SetContainedTarget(this);
                
            }
            alreadySet = true;
        }

        public bool Flip()
        {
            if(alreadySet)
            {
                // cannot flip
                return false;
            }

            this.Orientation = this.Orientation == Orientation.Vertical ? Orientation.Horizontal : Orientation.Vertical;
            return true;
        }

        public void HandleHit(HitEvent hitEvent)
        {
            HitCount++;


            if (HitCount < BlockSize)
            {
                return;
            }
            else
            {
                foreach (Cell cell in shipCells)
                {
                    if (cell.HitCount == 0)
                    {
                        return;
                    }
                }
                // all cells have been hit, sink the ship
                State = TargetState.Destroyed;
                //raise the event
                TargetDestroyed?.Invoke(this);
                
            }
        }
    }
}
