using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutoCorrect
{
    public record AlgorithmProfile(double AverageSpeed, double PercentMatched, double AverageSpeedOnMatch);

    public static class Profiling
    {
        private record Changed(string Expected, string Edited);
        private static Random rand = new Random();

        private static IEnumerable<Changed> OneChangedLetter = Words.All.RandomSample(1000, w => w.Length > 3)
            .Select(w =>
        {
            var chars = w.ToCharArray();
            chars[rand.Next(0, w.Length - 1)] = (char)('a' + rand.Next(0, 26));
            return new Changed(w, new string(chars));
        }).ToList();
        
        public enum SuggestEvaluation
        {
            Found,
            NotFound,
            NoResult,
            ExactMatch,
            Error
        }

        public record AlgorithmProfileItem(long Speed, SuggestEvaluation Success);

        private record ProfileAccumulator(int Count, long TotalTime, int TotalFound, long TimeForFound);
        public static async Task<AlgorithmProfile> Profile(Corrector corrector)
        {
            return await Task.Run(() => OneChangedLetter.Aggregate<Changed, ProfileAccumulator, AlgorithmProfile>(
                new ProfileAccumulator(0, 0, 0, 0), (aggregate, word) =>
                {
                    var iteration = ProfileIteration(corrector, word);
                    return aggregate with
                    {
                        Count = aggregate.Count + 1,
                        TotalTime = aggregate.TotalTime + iteration.Speed,
                        TotalFound = aggregate.TotalFound + (iteration.Success == SuggestEvaluation.Found ? 1 : 0),
                        TimeForFound = aggregate.TimeForFound + (iteration.Success == SuggestEvaluation.Found ? iteration.Speed : 0)
                    };
                },
                accumulated => new AlgorithmProfile(
                    (double)accumulated.TotalTime / accumulated.Count,
                    (double)accumulated.TotalFound / accumulated.Count,
                    (double)accumulated.TimeForFound / accumulated.TotalFound)
            ));
        }

        private static AlgorithmProfileItem ProfileIteration(Corrector corrector, Changed pair)
        {
            var timer = Stopwatch.StartNew();
            var corrections = corrector.Suggest(pair.Edited);
            timer.Stop();
            return new AlgorithmProfileItem(
                timer.ElapsedTicks,
                corrections switch
                {
                    SuggestedCorrections suggested => suggested.Options.Contains(pair.Expected) ? SuggestEvaluation.Found : SuggestEvaluation.NotFound,
                    ExactMatch => SuggestEvaluation.ExactMatch,
                    Error => SuggestEvaluation.Error,
                    NoMatch => SuggestEvaluation.NoResult,
                    _ => SuggestEvaluation.Error
                }
            );
        }
    }
}
