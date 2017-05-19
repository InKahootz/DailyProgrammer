using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace _2017_05_18_Game_of_Life_Twist
{
    class FastColorLogger
    {
        private static Dictionary<char, ConsoleColor> colorScheme =
            new Dictionary<char, ConsoleColor>
            {
                        { '.', ConsoleColor.Gray },
                        { '#', ConsoleColor.Cyan },
                        { '*', ConsoleColor.Red },
            };

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
                IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        public static void WriteCharArray(char[,] array)
        {
            SafeFileHandle h = CreateFile("CONOUT$", 0x4000_0000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            if (!h.IsInvalid)
            {
                short rows = (short)array.GetLength(0);
                short cols = (short)array.GetLength(1);
                CharInfo[] buf = new CharInfo[rows * cols];
                SmallRect rect = new SmallRect() { Left = 0, Top = 0, Bottom = rows, Right = cols };

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        buf[i * cols + j].Attributes = (short)colorScheme[array[i, j]];
                        buf[i * cols + j].Char.AsciiChar = (byte)array[i, j];
                    }
                }

                bool b = WriteConsoleOutput(h, buf,
                    new Coord() { X = cols, Y = rows },
                    new Coord() { X = 0, Y = 0 },
                    ref rect); 
            }
        }
    }
}
