using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoCorrect.Algorithms;

namespace AutoCorrect
{
    public static class Correctors
    {
        public static IDictionary<string, Func<string, CorrectionResult>> Options = new Dictionary<string, Func<string, CorrectionResult>> {
            { "Autocomplete After Error", SuggestAfterFirstError.Suggest },
            { "Find matching triplets", CompareTriplets.Suggest }
        };
    }

    public interface CorrectionResult {
        private static NoCorrection none = new NoCorrection();
        public static NoCorrection NoCorrection() => none;
        public static SuggestedCorrections Suggest(IEnumerable<string> options) => new SuggestedCorrections { Options = options };
    }

    public class SuggestedCorrections: CorrectionResult
    {
        public IEnumerable<string> Options { get; init; }
    }

    public class NoCorrection: CorrectionResult { }


}
