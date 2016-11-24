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
        private City city;

        public int ID { get { return id; } }
        public double X { get { return xCoord; } }
        public double Y { get { return yCoord; } }

        public City(int idIn, double xCoordIn, double yCoordIn)
        {
            id = idIn;
            xCoord = xCoordIn;
            yCoord = yCoordIn;
        }

        public double DistanceTo(City city)
        {
            return DistanceBetween(this, city);
        }

        /// <summary>
        /// Calculates distance between two cities to be used as weight of edge
        /// </summary>
        /// <param name="city1">First city</param>
        /// <param name="city2">Second city</param>
        /// <returns>Distance between input cities</returns>
        private static double DistanceBetween(City city1, City city2)
        {
            return Math.Sqrt(Math.Pow((city2.X - city1.X), 2) + Math.Pow((city2.Y - city1.Y), 2));
        }

        public int CompareTo(object obj)
        {
            City c = (City)obj;
            return Math.Abs(c.ID.CompareTo(ID));
        }
    }
}
