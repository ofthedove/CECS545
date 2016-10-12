using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Strand
    {
        private int length;
        private Gene[] genes;
        private bool locked = false;
        
        public int Length
        {
            get { return length; }
        }

        public Gene this[int i]
        {
            get { return genes[i]; }
            set
            {
                if(!locked)
                {
                    genes[i] = value;
                }
                else
                {
                    throw new ApplicationException("Cannot override genes of a locked strand!");
                }
            }
        }

        public Strand(int lengthIn)
        {
            length = lengthIn;
            genes = new int[length];
        }

        /// <summary>
        /// Lock this strand so that it may no longer be altered, only accessed
        /// This action should be taken as soon as the strand is in it's final state
        /// </summary>
        /// <exception cref="ApplicationException">Thrown if Strand is already locked. Attempting to lock a strand twice indicates an error.</exception>
        public void Lock()
        {
            if(locked == false)
            {
                locked = true;
            }
            else
            {
                throw new ApplicationException("Cannot lock already locked Strand!");
            }
        }
    }
}
