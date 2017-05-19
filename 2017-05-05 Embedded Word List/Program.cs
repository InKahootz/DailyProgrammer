using System;
using System.Collections.Generic;
using System.Linq;

namespace _2017_05_05_Embedded_Word_List
{
    class Program
    {
        static void Main()
        {
            List<string> input = new List<string>();
            string word;
            do
            {
                word = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(word))
                {
                    input.Add(word);
                }
            } while (!string.IsNullOrWhiteSpace(word));
            Console.WriteLine(EmbedList(input));
            Console.ReadLine();
        }

        private static string EmbedList(List<string> words)
        {
            if (words.Count == 0) return "";
            if (words.Count == 1) return words[0];
            string embeddedString = words[words.Count - 1];
            for (int i = words.Count - 2; i >= 0; i--)
            {
                string currentWord = words[i];
                int minInsertionIndex = 0;

                Dictionary<char, int> letterRepeats = new Dictionary<char, int>();
                for (int j = currentWord.Length - 1; j >= 0 ; j--)
                {
                    char currentChar = currentWord[j];
                    if (letterRepeats.ContainsKey(currentChar))
                    {
                        letterRepeats[currentChar]++;
                    }
                    else
                    {
                        letterRepeats.Add(currentChar, 1);
                    }
                    var result = embeddedString.Select((c, index) => new { Item = c, Index = index })
                                                .Where(c => c.Item == currentChar)
                                                .Select(c => c.Index);
                    if (result.Count() < letterRepeats[currentChar])
                    {
                        embeddedString = embeddedString.Insert(minInsertionIndex+1, currentChar.ToString());
                        minInsertionIndex++;
                    }
                    else
                    {
                        minInsertionIndex = result.First();
                    }
                }
            }
            return embeddedString;
        }
    }
}
