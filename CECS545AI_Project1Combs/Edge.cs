using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    /// <summary>
    /// Edge from City 1 to City 2.
    /// </summary>
    class Edge
    {
        private City city1;
        private City city2;
        private double distance; // weight
        private bool selected;

        public City City1
        {
            get { return city1;  }
        }
        public City City2
        {
            get { return city2; }
        }
        public double Distance
        {
            get { return distance; }
        }
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        public Edge(City city1In, City city2In)
        {
            city1 = city1In;
            city2 = city2In;
            distance = DistanceBetween(city1, city2);
            selected = false;
        }

        public void Select()
        {
            Selected = true;
        }

        public void Deselect()
        {
            Selected = false;
        }

        private static double DistanceBetween(City city1, City city2)
        {
            return Math.Sqrt(Math.Pow((city2.X - city1.X), 2) + Math.Pow((city2.Y - city1.Y), 2));
        }
    }
}
