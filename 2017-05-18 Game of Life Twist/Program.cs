using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _2017_05_18_Game_of_Life_Twist
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            int[] inputs = { 50, 50, 50 };
#else
            int[] inputs = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
#endif
            int W = inputs[0], H = inputs[1], N = inputs[2];
            var lb = new LifeBoard(W, H, N);
            Console.SetWindowSize(W+6, H+6);
            lb.PrintBoard();
            lb.IterateBoard(true);
            lb.PrintBoard();
            //Console.ReadLine();
        }
    }

    class LifeBoard
    {
        char[] symbols = new[] { '.', '#', '*' };
        private char[,] state;

        public LifeBoard(int W, int H, int N)
        {
            state = new char[H, W];
            this.W = W;
            this.H = H;
            this.N = N;

            Populate();
        }

        public char this[int x, int y]
        {
            get { return state[WrapY(y), WrapX(x)]; }
            set { state[WrapY(y), WrapX(x)] = value; }
        }

        public int H { get; }

        public int N { get; }

        public int W { get; }

        public void IterateBoard(bool print)
        {
            for (int i = 0; i < N; i++)
            {
                char[,] newState = new char[H, W];
                for (int y = 0; y < H; y++)
                {
                    for (int x = 0; x < W; x++)
                    {
                        newState[y,x] = IterateCell(x, y);
                    }
                }
                state = newState;

                if (print)
                {
                    PrintBoard();
                    Thread.Sleep(100);
                }
            }
        }

        public char IterateCell(int x, int y)
        {
            char[] neighbors = GetNeighbors(x, y);
            char currentCell = neighbors[4];

            int numHash = neighbors.Count(c => c == '#');
            int numStar = neighbors.Count(c => c == '*');

            bool changeColor = currentCell == '#' ? numStar > numHash : numHash > numStar;

            if (currentCell != '.' && changeColor)
            {
                return currentCell == '*' ? '#' : '*';
            }

            int cellsOn = neighbors.Count(c => c != '.');

            // cell off
            if (currentCell == '.' && cellsOn == 3)
            {
                currentCell = numHash > numStar ? '#' : '*';
            }
            // cell on, 2 or 3 neighbors
            if (currentCell != '.')
            {
                cellsOn--;
                if (cellsOn < 2)
                {
                    currentCell = '.';
                }
                if (cellsOn > 3)
                {
                    currentCell = '.';
                }
            }

            return currentCell;
        }

        public void PrintBoard()
        {
            FastColorLogger.WriteCharArray(state);
        }

        private void Populate()
        {
            var rnd = new Random();
            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
#if DEBUG
                    state[y, x] = '.';
#else
                    state[y, x] = symbols[rnd.Next(0, symbols.Length)];
#endif

                }
            }
#if DEBUG
            // exploder test
            state[25, 22] = '#';
            state[25, 23] = '#';
            state[25, 24] = '#';
            state[25, 25] = '#';
            state[25, 26] = '#';

            state[29, 22] = '#';
            state[29, 23] = '#';
            state[29, 24] = '#';
            state[29, 25] = '#';
            state[29, 26] = '#';

            state[27, 22] = '#';
            state[27, 26] = '#'; 
#endif
        }

        private char[] GetNeighbors(int x, int y)
        {
            //return new char[]
            //    {
            // top row
                var tl =   this[-1 + x, -1 + y];
                var tm =   this[-1 + x,  0 + y];
                var tr =   this[-1 + x,  1 + y];
                var ml =   this[ 0 + x, -1 + y];
                var mm =   this[ 0 + x,  0 + y];
                var mr =   this[ 0 + x,  1 + y];
                var bl =   this[ 1 + x, -1 + y];
                var bm =   this[ 1 + x,  0 + y];
                var br =   this[ 1 + x,  1 + y];
            //};
            return new char[] { tl, tm, tr, ml, mm, mr, bl, bm, br};
        }

        private int WrapX(int x)
        {
            while (x < 0) x += W;
            return x % W;
        }

        private int WrapY(int y)
        {
            while (y < 0) y += H;
            return y % H;
        }
    }

    class ColorLogger
    {
        private static Dictionary<char, ConsoleColor> colorScheme =
            new Dictionary<char, ConsoleColor>
            {
                { '.', ConsoleColor.Gray },
                { '#', ConsoleColor.Cyan },
                { '*', ConsoleColor.Red },
            };

        public static void Write(char value)
        {
            Console.ForegroundColor = colorScheme[value];
            Console.Write(value);
        }
    }
}
