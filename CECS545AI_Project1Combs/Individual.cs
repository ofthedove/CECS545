using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Individual
    {
        private Strand AStrand;
        private Strand BStrand;
        private Strand expressedStrand;
        //private Solution solution;
        //private int fitness; = solution.length
        private Individual mate;
        private int age;

        public Individual(Strand strandA, Strand strandB)
        {
            AStrand = strandA;
            BStrand = strandB;
            expressedStrand = Express(AStrand, BStrand);
            age = 0;
        }

        private static Strand Express(Strand strandA, Strand strandB)
        {
            // Use the dominant/recessive traits of the genes to determine which genes are expressed
            throw new NotImplementedException();
        }
    }
}
