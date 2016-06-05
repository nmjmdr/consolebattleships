using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            bool trial = false;
            if(args.Length == 1 && args[0] != null && args[0].ToLower() == "trial")
            {
                trial = true;
            }
            Game game = new Game(trial);
            game.Start();
        }
    }
}
