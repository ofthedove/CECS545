using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    abstract class Individual
    {
        protected Strand strandA;

        protected static Random rand = new Random();

        //private Strand expressedStrand;
        //private Solution solution;
        //private int fitness; = solution.length
        //private Individual mate;
        //private int age;

        protected virtual Strand SolutionStrand()
        {
            return strandA;
        }

        public virtual Strand MatingStrand()
        {
            return strandA;
        }

        public virtual Strand NewMatingStrand()
        {
            return MatingStrand();
        }
    }

    abstract class DoubleStrandIndividual : Individual
    {
        protected Strand strandB;
        protected Strand matingStrand = null;
        protected Strand solutionStrand = null;

        protected override Strand SolutionStrand()
        {
            return strandA;
        }

        public override Strand MatingStrand()
        {
            if (matingStrand == null)
            {
                matingStrand = NewMatingStrand();
            }

            return matingStrand;
        }

        public virtual Strand NewMatingStrand()
        {
            strandA.Sort();
            strandB.Sort();
            matingStrand = new Strand(strandA.Length);
            for (int i = 0; i < matingStrand.Length; i++)
            {
                bool selector = rand.Next(0, 1) == 1;
                matingStrand[i] = selector ? strandA[i] : strandB[i];
            }
            matingStrand.Lock();
            return matingStrand;
        }
    }

    class DoubleCityStrandIndividual : DoubleStrandIndividual
    {
        public DoubleCityStrandIndividual(Strand strandAIn, Strand strandBIn)
        {
            strandA = strandAIn;
            strandB = strandBIn;
        }

        protected override Strand SolutionStrand()
        {
            throw new NotImplementedException();
        }
    }

    class DoublePositionStrandIndividual : DoubleStrandIndividual
    {
        public DoublePositionStrandIndividual(Strand strandAIn, Strand strandBIn)
        {
            strandA = strandAIn;
            strandB = strandBIn;
        }

        protected override Strand SolutionStrand()
        {
            throw new NotImplementedException();
        }
    }

    class SingleCityStrandIndividual : Individual
    {
        public SingleCityStrandIndividual(Strand strandAIn)
        {
            strandA = strandAIn;
        }
    }

    class SinglePositionStrandIndividual : Individual
    {
        public SinglePositionStrandIndividual(Strand strandAIn)
        {
            strandA = strandAIn;
        }
    }
}
