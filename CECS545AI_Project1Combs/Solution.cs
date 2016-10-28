using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Solution
    {
        private List<int> path;

        public Graph BuildGraph;

        private class SolutionBuilder
        {
            private int[] pathArray;
            public SolutionBuilder(Strand strand)
            {
                pathArray = new int[strand.Length];
                Dictionary<int, int> dict = new Dictionary<int, int>();
                for (int i = 0; i < strand.Length; i++)
                {
                    dict.Add(strand[i].Value, i);
                }
                // TODO : Add linq magic to make this work
                // take the dict to list of key value pairs and sort that
            }
        }
    }
}
