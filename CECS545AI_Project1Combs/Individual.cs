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

        protected static Random rand;

        private int fitness = -1;

        public int Fitness
        {
            get
            {
                if(fitness == -1)
                {
                    fitness = CalculateFitness();
                }

                return fitness;
            }
        }

        //private Strand expressedStrand;
        //private Solution solution;
        //private int fitness; = solution.length
        //private Individual mate;
        //private int age;
        public Individual(Strand strandAIn, Random randIn)
        {
            strandA = strandAIn;
            rand = randIn;
        }

        public virtual Strand SolutionStrand()
        {
            return strandA;
        }

        public virtual Strand NewSolutionStrand()
        {
            return strandA;
        }

        public virtual Strand MatingStrand()
        {
            return strandA;
        }

        public virtual Strand NewMatingStrand()
        {
            return strandA;
        }

        private int CalculateFitness()
        {
            throw new NotImplementedException();
        }
    }

    abstract class DoubleStrandIndividual : Individual
    {
        protected Strand strandB;
        protected Strand matingStrand = null;
        protected Strand solutionStrand = null;

        public DoubleStrandIndividual(Strand strandAIn, Random randIn) : base(strandAIn, randIn) { }

        public override Strand SolutionStrand()
        {
            if (solutionStrand == null)
            {
                solutionStrand = NewSolutionStrand();
            }

            return solutionStrand;
        }

        public override abstract Strand NewSolutionStrand();

        public override Strand MatingStrand()
        {
            if (matingStrand == null)
            {
                matingStrand = NewMatingStrand();
            }

            return matingStrand;
        }

        public override Strand NewMatingStrand()
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
        public DoubleCityStrandIndividual(Strand strandAIn, Strand strandBIn, Random randIn) : base(strandAIn, randIn)
        {
            strandB = strandBIn;
        }

        public override Strand NewSolutionStrand()
        {
            throw new NotImplementedException();
        }
    }

    class DoublePositionStrandIndividual : DoubleStrandIndividual
    {
        public DoublePositionStrandIndividual(Strand strandAIn, Strand strandBIn, Random randIn) : base(strandAIn, randIn)
        {
            strandB = strandBIn;
        }

        public override Strand NewSolutionStrand()
        {
            throw new NotImplementedException();
        }
    }

    class SingleCityStrandIndividual : Individual
    {
        public SingleCityStrandIndividual(Strand strandAIn, Random randIn) : base(strandAIn, randIn) { }
    }

    class SinglePositionStrandIndividual : Individual
    {
        public SinglePositionStrandIndividual(Strand strandAIn, Random randIn) : base(strandAIn, randIn) { }
    }
}
