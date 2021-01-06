using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrect.Algorithms
{
    public static class SuggestAfterFirstError
    {
        // AKA Autocomplete, not autocorrect
        private class DictTreeNode
        {
            public char? Value { get; init; }
            public Dictionary<char, DictTreeNode> Next { get; } = new();
        }

        private static DictTreeNode Head = BuildInOrderTree(Words.All);

        private static DictTreeNode BuildInOrderTree(List<string> words) =>
            words.Aggregate(new DictTreeNode { Value = null }, (head, word) =>
            {
                var current = head;
                foreach (var letter in word)
                {
                    if (!current.Next.ContainsKey(letter))
                    {
                        current.Next[letter] = new DictTreeNode { Value = letter };
                    }
                    current = current.Next[letter];
                }
                return head;
            });
        public static IEnumerable<string> Suggest(string input)
        {
            var match = "";
            var current = Head;
            foreach (var letter in input)
            {
                if (!current.Next.ContainsKey(letter))
                {
                    return AllChildStringsAfter(current).Select(s => $"{match}{s}");
                }
                match = $"{match}{letter}";
                current = current.Next[letter];
            }
            return new List<string> { "Nice, that's a real word!" };
        }

        private static IEnumerable<string> AllChildStringsAfter(DictTreeNode node) =>
            node.Next.SelectMany(pair => AllChildStrings(pair.Value));

        private static IEnumerable<string> AllChildStrings(DictTreeNode node) =>
            node.Next.Count == 0
                ? new List<string> { $"{node.Value}" }
                : node.Next.SelectMany(pair => AllChildStrings(pair.Value), (current, next) => $"{node.Value}{next}");
    }
}
