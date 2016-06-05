using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.battleships
{
    public class Placer : IPlacer
    {
        // placement as strategy can be random or try to be intelligent

        private IPlacementStrategy strategy;
       

        public Placer(Grid grid,IPlacementStrategy placementStrategy)
        {
            strategy = placementStrategy;
        }

        

        public void Place(Grid grid,Target ship)
        {         

            Cell[] selectedCells = strategy.FindPlacement(ship);

            if(selectedCells == null || selectedCells.Length < ship.BlockSize)
            {
                throw new Exception("Something went wrong");
            }

            grid.MarkCells(selectedCells);

            ship.SetAt(selectedCells);
        }
    }
}
