using GAF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIProject4Nes
{
    public class Log
    {
        private Map map;
        private List<GenerationData> logData;

        public Map OriginalMap { get { return map;  } }
        
        public int Length { get { return logData.Count;  } }

        public class GenerationData
        {
            private double genNum;
            private double genTime;
            private double avgFitness;
            private double stdDevFit;

            private Chromosome mostFit;
            private Chromosome leastFit;
            private Chromosome wocSol;

            public double GenNum { get { return genNum; } }
            public double GenTime { get { return genTime; } }
            public double MaxFitness { get { return (double)mostFit.Tag; } }
            public double MinFitness { get { return (double)leastFit.Tag; } }
            public double WoCFitness { get { return (double)wocSol.Tag; } }
            public double AvgFitness { get { return avgFitness; } }
            public double StdDevFit { get { return stdDevFit; } }

            public Chromosome MostFitSolution { get { return mostFit; } }
            public Chromosome LeastFitSolution { get { return leastFit; } }
            public Chromosome WoCSolution { get { return wocSol; } }

            public string Blurb
            {
                get
                {
                    return string.Format("Gen {0,3:####} in {1,5:#0.0}s : WoC {2,5:####} | Max {3,5:####} | Min {4,5:####} | Avg {5,5:####} | SDv {6,5:####}", genNum, genTime, WoCFitness, MaxFitness, MinFitness, avgFitness, stdDevFit);
                }
            }

            private GenerationData(int genNumIn, double genTimeIn, double avgFitIn, double stdDevFitIn, Chromosome wocSolIn, Chromosome mostFitIn, Chromosome leastFitIn)
            {
                genNum = genNumIn;
                genTime = genTimeIn;
                avgFitness = avgFitIn;
                stdDevFit = stdDevFitIn;

                wocSol = wocSolIn;
                mostFit = mostFitIn;
                leastFit = leastFitIn;
            }

            public static GenerationData GenDataFromPopulation(int genNumIn, double genTimeIn, Population pop, Chromosome wocSolIn)
            {
                double runningSum = 0;
                foreach (Chromosome chrom in pop.Solutions)
                {
                    runningSum += (double)chrom.Tag;
                }
                double avgFitness = runningSum / pop.PopulationSize;

                double stdDevFit = 0;
                runningSum = 0;
                foreach(Chromosome chrom in pop.Solutions)
                {
                    runningSum += System.Math.Pow((double)chrom.Tag - avgFitness, 2);
                }
                stdDevFit = runningSum / (double)pop.PopulationSize;
                stdDevFit = System.Math.Sqrt(stdDevFit);
                
                // This is really important because, turns out, GAF reuses chromosomes.
                // I really should've known better in the first place.....
                Chromosome mostFit = pop.GetTop(1)[0].DeepClone();
                Chromosome leastFit = pop.GetBottom(1)[0].DeepClone();

                // WoC doesn't need to be deep cloned b/c it's generated new in onGenerationFinished so we 
                //    have the only reference to it here.

                return new GenerationData(genNumIn, genTimeIn, avgFitness, stdDevFit, wocSolIn, mostFit, leastFit);
            }
        }

        public Log(Map mapIn)
        {
            map = mapIn;
            logData = new List<GenerationData>();
        }

        public void Write(GenerationData item)
        {
            logData.Add(item);
        }

        public string ReadShort(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentException("Specified index does not exist in log");
            }

            return logData[index].Blurb;
        }

        public GenerationData ReadFull(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentException("Specified index does not exist in log");
            }

            return logData[index];
        }

        public void SaveBrief(string filePath)
        {
            StreamWriter writer = new StreamWriter(filePath);
            for(int i = 0; i < this.Length; i++)
            {
                writer.WriteLine(this.ReadShort(i));
            }
            writer.Close();
        }
    }
}
