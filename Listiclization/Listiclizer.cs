using System;
using System.Collections.Generic;
using System.Linq;

namespace Listiclization
{
    ///<summary> Splits lists into listicles (sub-groups), with each list beginning with a given criteria </summary>
    public static class Listiclizer
    {
        public static List<List<T>> SplitIntoListicles<T>(List<T> inputs, Func<T, bool> predicateCriteria)
        {
            var listOfLists = new List<List<T>>();
            if (!inputs.Any())
                return listOfLists;

            int GetNextStartIndex(int i) => inputs.FindIndex(startIndex: i + 1, 
                                                             match: input => predicateCriteria(input));

            int totalTaken = 0, startIndex = 0, nextStartIndex = GetNextStartIndex(0);
            while(nextStartIndex != -1)
            {
                var numberToTake = nextStartIndex - startIndex;
                var listicle = inputs.Skip(totalTaken).Take(numberToTake).ToList();
                listOfLists.Add(listicle);
                totalTaken += numberToTake;
                startIndex = nextStartIndex;
                nextStartIndex = GetNextStartIndex(startIndex);
            }
            if (totalTaken == inputs.Count)
                return listOfLists;
            var remaining = inputs.Skip(totalTaken).ToList();
            listOfLists.Add(remaining);
            return listOfLists;
        }
    }
}