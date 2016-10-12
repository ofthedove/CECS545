using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Gene
    {
        private int value;

        public int Value
        {
            get { return value; }
        }

        public Gene(int valueIn)
        {
            value = valueIn;
        }
    }
}
