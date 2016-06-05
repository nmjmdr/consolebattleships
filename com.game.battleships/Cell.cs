using System;

namespace com.game.battleships
{
    public class Cell
    {        

        public bool IsUsed { get; set; }        
        public int Row { private set; get; }
        public int Col { private set; get;  }
        public int HitCount { get; private set; }

        public ITarget Target { get; private set; }
        
        public Cell(int row,int col)
        {
            this.Row = row;
            this.Col = col;
        }   
        
        public void Hit()
        {
            HitCount++;                       
        }  
        
        public void SetContainedTarget(ITarget target)
        {
            Target = target;
        }   
    }
}