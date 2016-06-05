using com.game.battleships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.game.ConsoleGame
{
    public static class Printer
    {
        public delegate string CellPrintFunction(Cell cell);

        public static void Print(Cell[,] cells, CellPrintFunction cellPrintFn)
        {

            int u1 = cells.GetUpperBound(0);
            int u2 = cells.GetUpperBound(1);
            int l1 = cells.GetLowerBound(0);
            int l2 = cells.GetLowerBound(1);

            string[,] printArray = new string[u1 + 2, u2 + 2];
            setPrintContent(cells, cellPrintFn, u1, u2, l1, l2, printArray);
            setPrintRowHeading(printArray);
            setPrintColHeading(printArray);
            printIt(printArray);

        }

        private static void printIt(string[,] printArray)
        {
            for (int row = printArray.GetLowerBound(0); row <= printArray.GetUpperBound(0); row++)
            {
                for (int col = printArray.GetLowerBound(1); col <= printArray.GetUpperBound(1); col++)
                {
                    Console.Write(printArray[row, col]);
                    Console.Write(" ");
                }
                Console.WriteLine("");
            }
        }

        private static void setPrintColHeading(string[,] printArray)
        {
            for (int col = printArray.GetLowerBound(1); col <= printArray.GetUpperBound(1); col++)
            {
                if (col == 0)
                {
                    printArray[0, col] = " ";
                }
                else
                {
                    printArray[0, col] = col.ToString();
                }
            }
        }

        private static void setPrintRowHeading(string[,] printArray)
        {
            for (int row = printArray.GetLowerBound(0); row <= printArray.GetUpperBound(0); row++)
            {
                if (row == 0)
                {
                    printArray[row, 0] = " ";
                }
                else
                {                   
                    printArray[row, 0] = ((char)('A' + row - 1)).ToString();
                }
            }
        }

        private static void setPrintContent(Cell[,] cells, CellPrintFunction cellPrintFn, int u1, int u2, int l1, int l2, string[,] printArray)
        {
            for (int row = l1 + 1; row < u1 + 2; row++)
            {
                for (int col = l2 + 1; col < u2 + 2; col++)
                {
                    printArray[row, col] = cellPrintFn(cells[row - 1, col - 1]);
                }
            }
        }
    }
}
