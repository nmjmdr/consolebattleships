using com.game.battleships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static com.game.ConsoleGame.Printer;

namespace com.game.ConsoleGame
{
    public class Game
    {
        private Board board;
        private Scorer scorer;
        private CellPrintFunction cellPrintFn;
        private CellPrintFunction showShipsFn;
        private AutoResetEvent resetEvent;
        private int targetsDestroyed;
        private Target[] targets;
        private bool trial;

        public Game(bool trial)
        {
            this.trial = trial;

            board = new Board();
            scorer = new Scorer();
            resetEvent = new AutoResetEvent(false);
            
            cellPrintFn = (cell) => {
                if (cell.HitCount > 0)
                {
                    if (cell.Target == null)
                    {
                        return "x";
                    }
                    return "^";
                }
                return " ";
            };

            showShipsFn = (cell) =>
            {

                if (cell.Target != null)
                {
                    return "*";
                }
                return "-";
            };
        }

        public void Start()
        {
            targets = new Target[3];
            targets[0] = new Battleship();
            targets[1] = new Destroyer();
            targets[2] = new Destroyer();
            board.HitResultEvent += scorer.HitResultHanlder;
            
            board.Start(targets);

            printMessages();
            Console.ReadKey();

            Console.Clear();

            board.HitResultEvent += HitResultEvent;
            board.TargetDestroyedEvent += OnTargetDestroyed;
            gameLoop();

        }

        private void printScore(string message)
        {
            Console.WriteLine(string.Format("{0} {1}",message,scorer.Score));
        }

        private void gameLoop()
        {
            for (;;)
            {

                Console.Clear();
                if (trial)
                {
                    Printer.Print(board.Grid.Cells, showShipsFn);
                    Console.WriteLine();
                }
                Printer.Print(board.Grid.Cells, cellPrintFn);
                Console.WriteLine();
                printScore("Score");
                Console.WriteLine("row col (quit to exit): ");
                string line = Console.ReadLine();
                if (line == "quit")
                {
                    break;
                }
                Tuple<int, int> rowCol = parseRowCol(line.Trim());
                if (rowCol == null)
                {
                    Console.WriteLine("Please check your input");
                    printIntructions();
                    Console.WriteLine("Press any key");
                    Console.ReadKey();
                    continue;
                }
                board.Hit(rowCol.Item1, rowCol.Item2);
                resetEvent.WaitOne();

                if(isGameWon())
                {
                    Console.WriteLine(string.Format("You won, your final score: {0}", scorer.Score));
                    break;
                }

                resetEvent.Reset();
            }
        }

        private bool isGameWon()
        {
            return targetsDestroyed == targets.Length;            
        }

        private Tuple<int, int> parseRowCol(string val)
        {
            if (String.IsNullOrEmpty(val) || val.Length < 2)
            {
                return null;
            }
            val = val.ToUpper();
            char rowChar = val[0];
            
            int row = rowChar - 'A';

            val = val.Remove(0, 1);

            int col;
            if(!int.TryParse(val, out col))
            {
                return null;
            }
            col = col - 1;
            if( row < board.Grid.Cells.GetLowerBound(0) || row > board.Grid.Cells.GetUpperBound(0))
            {
                return null;
            }
            if (col < board.Grid.Cells.GetLowerBound(1) || col > board.Grid.Cells.GetUpperBound(1))
            {
                return null;
            }

            return new Tuple<int, int>(row, col);
        }

        private static void printIntro()
        {            
            Console.WriteLine(bede.ConsoleGame.Resources.IntroText);
        }

        private static void printIntructions()
        {
            Console.WriteLine(bede.ConsoleGame.Resources.InstructionsText);
        }

        private static void printScoring()
        {
            Console.WriteLine(string.Format(bede.ConsoleGame.Resources.ScoringText,Scorer.HitPoints, Scorer.MissPoints, Scorer.BattleshipDestroyPoints, Scorer.DestroyerDestroyPoints));
            
        }

        private static void printMessages()
        {
            printIntro();
            printIntructions();
            printScoring();
            Console.WriteLine("press any key to start...");
        }

        private void OnTargetDestroyed(ITarget target)
        {

            Console.WriteLine(string.Format("{0} sunk!", target?.Name));
            targetsDestroyed++;
            Thread.Sleep(900);       
        }

        private void HitResultEvent(HitResult hitResult)
        {
            if (hitResult.Type == HitResultType.AlreadyHit)
            {
                Console.WriteLine("You have already hit that cell, try another cell");
            }
            else if(hitResult.Type == HitResultType.Hit)
            {
                Console.WriteLine("A hit!");
            }            
            else if(hitResult.Type == HitResultType.Missed)
            {
                Console.WriteLine("Missed");
            }          

            //Console.WriteLine("Press any key");
            //Console.ReadKey();   
            Thread.Sleep(900);
            resetEvent.Set();
            
        }
    }
}
