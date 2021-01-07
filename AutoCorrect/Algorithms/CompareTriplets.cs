using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrect.Algorithms
{
    public class CompareTriplets
    {
        public static Dictionary<string, List<string>> TripletsToWords = BuildTripletsMap(Words.All);

        public static Dictionary<string, List<string>> BuildTripletsMap(IEnumerable<string> words) =>
            words.Aggregate(new Dictionary<string, List<string>>(), (triplets, word) =>
            {
                foreach(var triplet in Triplets(word))
                {
                    if (!triplets.ContainsKey(triplet))
                    {
                        triplets[triplet] = new List<string>();
                    }
                    triplets[triplet].Add(word);
                }
                return triplets;
            });

        public static IEnumerable<string> Triplets(string word) =>
            word.Length >= 3
                ? Enumerable.Range(0, word.Length - 3).Select(i => word.Substring(i, 3))
                : new List<string> { word };

        public static CorrectionResult Suggest(string input)
        {
            if (Words.AllSet.Contains(input))
            {
                return CorrectionResult.NoCorrection();
            }

            var matches = Triplets(input).Aggregate(new Dictionary<string, int>(), (matches, triplet) => {
                if (!TripletsToWords.ContainsKey(triplet))
                {
                    return matches;
                }
                foreach(var match in TripletsToWords[triplet])
                {
                    if (!matches.ContainsKey(match))
                    {
                        matches[match] = 1;
                    }
                    else
                    {
                        matches[match] = matches[match] + 1;
                    }
                }
                return matches;
            });
            return CorrectionResult.Suggest(matches.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
        }
    }
}
