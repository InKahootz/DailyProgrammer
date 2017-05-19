using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _2017_05_10_Comparing_Rotated_Words
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "pneumonoultramicroscopicsilicovolcanoconiosis";
            string minimal = input;
            int index = 0;
            for (int i = 1; i < input.Length; i++)
            {
                var s1 = input.Substring(0, i);
                var s2 = input.Substring(i, input.Length - i);
                var s3 = s2 + s1;

                if (s3.CompareTo(minimal) < 0)
                {
                    minimal = s3;
                    index = i;
                }
            }

            Console.WriteLine($"{index} {minimal}");
            Console.ReadLine();
        }
    }
}
