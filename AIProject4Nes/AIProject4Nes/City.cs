using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIProject4Nes
{
    public class City : IComparable
    {
        private int id;
        private double xCoord;
        private double yCoord;

        public int ID { get { return id; } }
        public double X { get { return xCoord; } }
        public double Y { get { return yCoord; } }

        public City(int idIn, double xCoordIn, double yCoordIn)
        {
            id = idIn;
            xCoord = xCoordIn;
            yCoord = yCoordIn;
        }

        public int CompareTo(object obj)
        {
            City c = (City)obj;
            return Math.Abs(c.ID.CompareTo(ID));
        }
    }
}
