using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoCorrect.Algorithms;

namespace AutoCorrect
{
    public record Corrector(string DisplayName, Func<string, CorrectionResult> Suggest);

    public static class Correctors
    {
        public static IEnumerable<Corrector> Options = new List<Corrector> {
            new Corrector("Find closest matching triplets",  CompareTriplets.SuggestLevenGreedy),
            new Corrector("Find close matching triplets", CompareTriplets.SuggestLeven(3)),
            new Corrector("Find matching triplets", CompareTriplets.Suggest),
            new Corrector("Hunspell (github: WeCantSpell.Hunspell)", Hunspell.Suggest),
            new Corrector("Autocomplete After Error", SuggestAfterFirstError.Suggest),
        };

        public static IDictionary<string, Corrector> Map = Options.ToDictionary(c => c.DisplayName);
    }

    public interface CorrectionResult {
        private static ExactMatch exact = new ExactMatch();
        private static NoMatch none = new NoMatch();
        public static ExactMatch ExactMatch() => exact;
        public static NoMatch NoMatch() => none;
        public static SuggestedCorrections Suggest(IEnumerable<string> options) => new SuggestedCorrections { Options = options };
        public static Error Fail(string reason) => new Error { Reason = reason };
    }

    public class SuggestedCorrections: CorrectionResult
    {
        public IEnumerable<string> Options { get; init; }
    }

    public class ExactMatch: CorrectionResult { }

    public class NoMatch : CorrectionResult { }

    public class Error : CorrectionResult
    {
        public string Reason { get; init; }
    }
}
