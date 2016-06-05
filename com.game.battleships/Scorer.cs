using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.battleships
{
    public class Scorer
    {
        public const int BattleshipDestroyPoints = 10;
        public const int DestroyerDestroyPoints = 5;
        public const int HitPoints = 2;
        public const int MissPoints = 1;
        public int Score { get; private set; }

        public void HitResultHanlder(HitResult hitResult)
        {
            switch(hitResult.Type)
            {
                case HitResultType.Missed:
                    Score -= MissPoints;
                    break;
                case HitResultType.AlreadyHit:
                    break;
                case HitResultType.Hit:
                    Score += HitPoints;                
                    break;
            }
        }

        public void TragetDestroyedHandler(ITarget target)
        {
            Score += target.Points;
        }


    }
}
