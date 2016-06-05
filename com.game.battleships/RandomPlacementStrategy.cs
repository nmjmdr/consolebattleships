using System;

namespace com.game.battleships
{
    public class RandomPlacementStrategy : IPlacementStrategy
    {
        private static Random rangeRandom = new Random();
        private Random randomToss = new Random();

        private Grid grid;

        public RandomPlacementStrategy(Grid grid)
        {
            this.grid = grid;
        }

        private bool heads()
        {
            return randomToss.Next(0, 2) == 1;
        }

        public Cell[] FindPlacement(Target ship)
        {
            // Flip the ship randomly
            if (heads())
            {
                ship.Flip();
            }

            // get random index
            // not worrying about thread safety - check this assumption later
            int selectedIndex;
            bool found;

            int trial = 0;

            for(;;) {
                
                if(trial > (grid.Size * grid.Size) )
                {
                    // stop, we could not find any
                    throw new Exception("Could not find any place on the grid, reduce the number of targets");
                }
                int index = rangeRandom.Next(0, grid.FreeCells.Count);

                if(isFeasible(grid.FreeCells[index],ship))
                {
                    found = true;
                    selectedIndex = index;
                    break;
                }
                trial++;
            }

            if(!found)
            {
                throw new Exception("not found! change this later");
            }

            return selectCells(selectedIndex, ship);
            
        }

        private Cell[] selectCells(int selectedIndex, Target ship)
        {
            Cell[] selected = new Cell[ship.BlockSize];
            Cell c = grid.FreeCells[selectedIndex];
            selected[0] = c;

            for(int counter =1;counter<ship.BlockSize;counter++)
            {
                selected[counter] = nextCell(c,ship.Orientation);
                c = selected[counter];
            }
            return selected;
        }

        private bool isFeasible(Cell cell,Target ship)
        {
            
            int feasibleCells = 0;
            for(Cell c = cell; c != null && feasibleCells < ship.BlockSize; c = nextCell(c,ship.Orientation) )
            {
                if (!c.IsUsed)
                {
                    feasibleCells++;
                }
                else
                {
                    return false;
                }
            }

            return feasibleCells == ship.BlockSize;
            
        }

        private Cell nextCell(Cell c, Orientation orientation)
        {            
            int row = c.Row;
            int col = c.Col;

            if(orientation == Orientation.Vertical)
            {
                row++;
            }
            else
            {
                col++;
            }

            if (row < grid.Size && col < grid.Size)
            {
                return grid.Cells[row,col];
            }

            return null;
        }
    }
}
