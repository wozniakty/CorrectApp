using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCorrect
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> RandomSample<T>(this List<T> set, int sampleSize, Func<T, bool> predicate=null)
        {
            var rand = new Random();
            var filtered = predicate == null ? set : set.Where(predicate).ToList();
            double available = filtered.Count;
            double needed = sampleSize;
            foreach(var item in filtered)
            {
                if(rand.NextDouble() < (needed / available))
                {
                    yield return item;
                    needed--;
                }
                available--;
                if (needed == 0)
                    yield break;
            }
        }
    }
}
