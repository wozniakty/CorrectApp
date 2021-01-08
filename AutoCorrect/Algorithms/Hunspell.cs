using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeCantSpell.Hunspell;

namespace AutoCorrect.Algorithms
{
    public static class Hunspell
    {
        public static WordList Checker = WordList.CreateFromWords(Words.All);

        public static CorrectionResult Suggest(string input) =>
            Checker.Check(input) switch
            {
                false => Checker.Suggest(input) switch
                {
                    var results when results.Any() => CorrectionResult.Suggest(results),
                    _ => CorrectionResult.NoMatch()
                },
                true => CorrectionResult.ExactMatch()
            };
    }
}
