using GAF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            private double avgFitness;
            private double stdDevFit;
            private double gafFitness;

            private Chromosome mostFit;
            private Chromosome leastFit;

            public double GenNum { get { return genNum; } }
            public double MaxFitness { get { return (double)mostFit.Tag; } }
            public double MinFitness { get { return (double)leastFit.Tag; } }
            public double AvgFitness { get { return avgFitness; } }
            public double StdDevFit { get { return stdDevFit; } }
            public double GAFFitness {  get { return gafFitness; } }

            public Chromosome MostFitSolution { get { return mostFit; } }
            public Chromosome LeastFitSolution { get { return leastFit; } }

            public string Blurb
            {
                get
                {
                    return string.Format("Gen {0,3:###} : Max {1,5:#####} | Min {2,5:.####} | Avg {3,5:#####} | SDv {4,5:#####}", genNum, MaxFitness, MinFitness, avgFitness, gafFitness);
                }
            }

            private GenerationData(int genNumIn, double avgFitIn, double stdDevFitIn, Chromosome mostFitIn, Chromosome leastFitIn, double gafFitIn)
            {
                genNum = genNumIn;
                avgFitness = avgFitIn;
                stdDevFit = stdDevFitIn;
                gafFitness = gafFitIn;

                mostFit = mostFitIn;
                leastFit = leastFitIn;
            }

            public static GenerationData GenDataFromPopulation(int genNumIn, Population pop)
            {
                double gafFit = pop.MaximumFitness;
                Chromosome mostFitt = pop.GetTop(1)[0];
                double value = mostFitt.Fitness;
                Debug.Assert(gafFit == value);

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

                Chromosome mostFit = pop.GetTop(1)[0];
                Chromosome leastFit = pop.GetBottom(1)[0];

                return new GenerationData(genNumIn, avgFitness, stdDevFit, mostFit, leastFit, gafFit);
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
