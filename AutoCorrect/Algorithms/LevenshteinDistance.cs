using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrect.Algorithms
{
    public static class LevenshteinDistance
    {
        /* Cheated with this one by looking it up
         * I knew I wanted to find a measure of how similar
         * two strings were, but I had no clue how to do it by hand.
         * The Levenshtien distance is defined as the number of steps between two strings,
         * either by replacing characters, or adding/removing them
         * Stolen from https://www.dotnetperls.com/levenshtein
         */
        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Verify arguments.
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Begin looping.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }
    }
}
