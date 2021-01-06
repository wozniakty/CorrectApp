using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CorrectFixtures;

namespace AutoCorrect
{
    public static class Words
    {
        static Words()
        {
            Console.WriteLine("Loading words from file.");
            All = AlphaWords.All;
            Console.WriteLine("Words loaded.");
        }

        public static List<string> All { get; private set; }
    }
}
