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
            if(input.Length <= 3)
            {
                return CorrectionResult.Fail("Sorry, I didn't get this working for word sizes less than 3...");
            }
            if (Words.AllSet.Contains(input))
            {
                return CorrectionResult.ExactMatch();
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
            if (!matches.Any())
            {
                return CorrectionResult.NoMatch();
            }
            // I could optimize by not ordering these in the leven suggest cases, buuuut that's tomorrow's problem
            return CorrectionResult.Suggest(matches.OrderByDescending(kv => kv.Value).Select(kv => kv.Key));
        }

        public static CorrectionResult Suggest(int maxDistance, string input) =>
            Suggest(input) switch
            {
                SuggestedCorrections corrections => corrections.Options.Where(s => LevenshteinDistance.Compute(input, s) <= maxDistance) switch
                {
                    var result when !result.Any() => CorrectionResult.NoMatch(),
                    var result => CorrectionResult.Suggest(result),
                },
                var result => result
            };


        public static Func<string, CorrectionResult> SuggestLeven(int maxDistance) => (string input) => Suggest(maxDistance, input);

        public static CorrectionResult SuggestLevenGreedy(string input) =>
            Suggest(input) switch
            {
                SuggestedCorrections corrections => CorrectionResult.Suggest(
                    corrections.Options
                        .Select(w => new { Word = w, Distance = LevenshteinDistance.Compute(input, w) })
                        .GroupBy(wd => wd.Distance).OrderBy(group => group.Key).First().Select(wd => wd.Word)
                ),
                var result => result
            };
    }
}
