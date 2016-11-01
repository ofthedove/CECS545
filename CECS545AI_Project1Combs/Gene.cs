using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Gene : IComparable
    {
        private int value; // The value of the gene
        private int location; // The location of the gene in the strand

        public int Value
        {
            get { return value; }
        }

        public int Position
        {
            get { return location; }
        }

        public Gene(int valueIn, int positionIn)
        {
            value = valueIn;
            location = positionIn;
        }

        public int CompareTo(object obj)
        {
            if (obj is Gene)
            {
                return Position.CompareTo((obj as Gene).Position);
            }
            else
            {
                return -1;
            }
        }

        public static IComparer sortValueAscending()
        {
            return (IComparer)new sortValueAscendingHelper();
        }

        private class sortValueAscendingHelper : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                Gene g1 = a as Gene;
                Gene g2 = b as Gene;
                if (g1 == null && g2 == null) { return 0; }
                if (g1 == null) { return 1; }
                if (g2 == null) { return -1; }
                return g1.value.CompareTo(g2.value);
            }
        }
    }
}
