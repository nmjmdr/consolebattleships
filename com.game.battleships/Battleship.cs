using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.battleships
{
    public class Battleship : Target
    {
        private const int blockSize = 5;

        public Battleship() : base(blockSize)
        {
        }

        public override string Name
        {
            get
            {
                return "Battleship";
            }
        }

        public override int Points
        {
            get
            {
                return 10;
            }
        }
    }
}
