using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoCorrect.Algorithms;

namespace AutoCorrect
{
    public static class Correctors
    {
        public static IDictionary<string, Func<string, IEnumerable<string>>> Options = new Dictionary<string, Func<string, IEnumerable<string>>> {
                { "Autocomplete After Error", SuggestAfterFirstError.Suggest }
            };
    }
}
