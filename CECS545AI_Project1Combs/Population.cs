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
        private PopulationProperties popInfo;
        private List<Individual> members;

        #region General Population Information
        private Individual mostFit;
        private Individual leastFit;
        private ulong fitnessSum;

        public int PopulationSize
        {
            get { return members.Count; }
        }

        public int LeastFitFitness
        {
            get { return leastFit.Fitness; }
        }

        public Individual LeastFit
        {
            get { return leastFit; }
        }

        public int MostFitFitness
        {
            get { return mostFit.Fitness; }
        }

        public Individual MostFit
        {
            get { return mostFit; }
        }

        public int AverageFitness
        {
            get
            {
                return (int)(fitnessSum / (ulong)members.Count);
            }
        }

        public double StdDevFitness
        {
            get
            {
                ulong runningSum = 0;
                int averageFitness = AverageFitness;
                foreach(Individual indv in members)
                {
                    runningSum += (ulong)Math.Pow(indv.Fitness - averageFitness, 2);
                }
                double variance = runningSum / (ulong)members.Count;
                double stdDev = Math.Sqrt(variance);
                return stdDev;
            }
        }
        #endregion General Population Information

        public Population(PopulationProperties popInfoIn)
        {
            rand = new Random();
            popInfo = popInfoIn;

            /*Seed();
            while (CheckEndCondition() != false)
            {
                Cull();
                Breed();
                Mutate();
            }*/

            /* (Probably not here)
             * Spawn(1000);
             * CullTo(50);
             * while (!CheckEndCondition())
             *    BreedTo(100);
             *    Mutate(); // (Only affect new (age=0) individuals)
             *    CullTo(50);
             * endwhile
             */
        }

        public void Spawn(int numToSpawn)
        {
            if (numToSpawn < 0)
            {
                throw new ArgumentException("numToSpawn must be positive!");
            }

            // randomly generate numToSpawn new Individuals
            for (; numToSpawn > 0; numToSpawn--)
            {
                Individual indv = GenerateRandomIndividual();
                members.Add(indv);
                UpdateStatsOnAdd(indv);
            }

            // need to update log still

            throw new NotImplementedException();
        }

        public void SpawnTo(int popSize)
        {
            Spawn(popSize - PopulationSize);
        }

        public void Breed(int numToBreed)
        {
            if (numToBreed < 0)
            {
                throw new ArgumentException("numToBreed must be positive!");
            }

            throw new NotImplementedException();
        }

        public void BreedTo(int popSize)
        {
            Breed(popSize - PopulationSize);
        }

        private void Mutate()
        {
            throw new NotImplementedException();
        }

        private void Cull(int numToCull)
        {
            if (numToCull < 0)
            {
                throw new ArgumentException("numToCull must be positive!");
            }

            throw new NotImplementedException();
        }

        public void CullTo(int popSize)
        {
            Cull(PopulationSize - popSize);
        }

        private bool CheckEndCondition()
        {
            throw new NotImplementedException();
        }

        #region private methods
        private Individual GenerateRandomIndividual()
        {
            Individual indv;

            if (popInfo.IndividualsAreDoubleStrand)
            {
                Strand strandA = Strand.GenerateRandomStrand(popInfo.Map.NumCities, popInfo.MaxGeneValue, rand);
                Strand strandB = Strand.GenerateRandomStrand(popInfo.Map.NumCities, popInfo.MaxGeneValue, rand);

                switch (popInfo.GeneStrandType)
                {
                    case GeneStrandType.CityAsValue:
                        indv = new DoubleCityStrandIndividual(strandA, strandB, rand);
                        break;
                    case GeneStrandType.OrderAsValue:
                        indv = new DoublePositionStrandIndividual(strandA, strandB, rand);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {
                Strand strandA = Strand.GenerateRandomStrand(popInfo.Map.NumCities, popInfo.MaxGeneValue, rand);

                switch (popInfo.GeneStrandType)
                {
                    case GeneStrandType.CityAsValue:
                        indv = new SingleCityStrandIndividual(strandA, rand);
                        break;
                    case GeneStrandType.OrderAsValue:
                        indv = new SinglePositionStrandIndividual(strandA, rand);
                        break;
                    default:
                        throw new NotImplementedException();
                }

            }

            return indv;
        }

        private void UpdateStatsOnAdd(Individual indv)
        {
            if(indv.Fitness > leastFit.Fitness)
            {
                leastFit = indv;
            }

            if(indv.Fitness < mostFit.Fitness)
            {
                mostFit = indv;
            }

            fitnessSum += (ulong)indv.Fitness;
        }

        private void UpdateStatsOnRemove(Individual indv)
        {
            if (indv == leastFit)
            {
                throw new NotImplementedException();
            }

            if (indv == mostFit)
            {
                throw new NotImplementedException();
            }

            fitnessSum -= (ulong)indv.Fitness;
        }

        private Individual FindLeastFit()
        {
            // Need to update to handle members being empty
            // (need to update stuff up above to handle this returning null)
            throw new NotFiniteNumberException();

            int worstFitnessYet = 0;
            Individual worstFitYet = null;

            foreach(Individual indv in members)
            {
                if (indv.Fitness > worstFitnessYet)
                {
                    worstFitYet = indv;
                    worstFitnessYet = indv.Fitness;
                }
            }

            if (worstFitYet == null)
            {
                throw new ArgumentException("");
            }
            return worstFitYet;
        }

        private Individual FindMostFit()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
