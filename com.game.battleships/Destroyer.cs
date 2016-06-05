using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.battleships
{
    public class Destroyer : Target
    {
        private const int blockSize = 4;
              

        public Destroyer() : base(blockSize)
        {
        }

        public override string Name
        {
            get
            {
                return "Destroyer";
            }
        }

        public override int Points
        {
            get
            {
                return 5;
            }
        }
    }
}
