using System;
using System.Collections.Generic;
using System.Linq;

namespace _2017_05_08_Concatenated_Integers
{
    class Program
    {
        static void Main(string[] args)
        {
            List<List<string>> input = new List<List<string>>();
            string @in;
            do
            {
                @in = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(@in))
                {
                    input.Add(@in.Split(' ').ToList());
                }
            }
            while (!string.IsNullOrWhiteSpace(@in));

            var comparer = new NumberStringComparer();

            for (int i = 0; i < input.Count; i++)
            {
                var nums = input[i];
                nums.Sort(comparer);

                Console.Write($"{string.Join("", nums)} ");
                nums.Reverse();
                Console.Write(string.Join("", nums));
            }

        }
    }

    class NumberStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return (x + y).CompareTo(y + x);
        }
    }
}
