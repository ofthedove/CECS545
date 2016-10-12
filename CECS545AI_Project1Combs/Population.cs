using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Population
    {
        private Random rand;
        private List<Individual> members;

        public Population()
        {
            rand = new Random();

            Seed();
            while (CheckEndCondition() != false)
            {
                Cull();
                Breed();
                Mutate();
            }
        }

        private void Seed()
        {
            throw new NotImplementedException();
        }

        private void Breed()
        {
            throw new NotImplementedException();
        }

        private void Mutate()
        {
            throw new NotImplementedException();
        }

        private void Cull()
        {
            throw new NotImplementedException();
        }

        private bool CheckEndCondition()
        {
            throw new NotImplementedException();
        }




        private Strand MixdownRandom(Strand strandA, Strand strandB)
        {
            Strand result = new Strand(strandA.Length);
            for(int i = 0; i < strandA.Length; i++)
            {
                int pick = rand.Next(0, 1);
                if(pick == 0)
                {
                    result[i] = strandA[i];
                }
                else
                {
                    result[i] = strandB[i];
                }
            }
            return result;
        }
    }
}
