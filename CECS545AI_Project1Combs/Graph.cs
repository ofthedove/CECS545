using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Graph
    {
        private List<City> Cities;
        private List<City> UnusedCities;
        private List<Edge> Edges;

        public int NumCities
        {
            get { return Cities.Count; }
        }

        public int NumEdges
        {
            get { return Edges.Count; }
        }

        public List<Edge> EdgeList
        {
            get { return Edges; }
        }

        public Graph()
        {
            Cities = new List<City>();
            UnusedCities = new List<City>(Cities);
            Edges = new List<Edge>();
        }

        public bool AddCity(City city)
        {
            if (Cities.Contains(city))
            {
                return false;
            }
            else
            {
                Cities.Add(city);
                UnusedCities.Add(city);
                return true;
            }
        }

        public bool AddEdge(City city1, City city2)
        {
            Edge edge = new Edge(city1, city2);
            return AddEdge(edge);
        }

        public bool AddEdge(Edge edge)
        {
            if(Edges.Contains(edge))
            {
                return false;
            }
            else
            {
                Edges.Add(edge);
                UnusedCities.Remove(edge.City1);
                UnusedCities.Remove(edge.City2);
                return true;
            }
        }

        public City GetCityByID(int id)
        {
            foreach(City city in Cities)
            {
                if(city.ID == id)
                {
                    return city;
                }
            }
            return null;
        }

        public Edge GetEdgeByStartingCity(City city)
        {
            foreach(Edge edge in Edges)
            {
                if (edge.City1.CompareTo(city) == 0)
                    return edge;
            }
            return null;
        }

        public void BreakCityIntoEdge(Edge edge, City city)
        {
            Edges.Remove(edge);
            Edges.Add(new Edge(edge.City1, city));
            Edges.Add(new Edge(city, edge.City2));
        }

        public City GetClosestCityToCity(City cityIn)
        {
            double curMinDistance = double.MaxValue;
            City curClosestCity = null;
            foreach(City city in UnusedCities)
            {
                Edge tempEdge = new Edge(cityIn, city);
                if (tempEdge.Distance < curMinDistance || (tempEdge.Distance == curMinDistance && city.ID < curClosestCity.ID))
                {
                    curMinDistance = tempEdge.Distance;
                    curClosestCity = city;
                }
            }
            return curClosestCity;
        }
        
        public City GetClosestCityToEdge(Edge edge, out double distanceSqr)
        {
            double curMinDistanceSqr = double.MaxValue;
            City curClosestCity = null;
            foreach (City city in UnusedCities)
            {
                double distSqr = GetEdgeCityDistanceSquared(edge, city);
                if (distSqr < curMinDistanceSqr || (distSqr == curMinDistanceSqr && city.ID < curClosestCity.ID))
                {
                    curMinDistanceSqr = distSqr;
                    curClosestCity = city;
                }
            }
            distanceSqr = curMinDistanceSqr;
            return curClosestCity;
        }

        // From Stack Overflow stackoverflow.com/questions/849211
        public double GetEdgeCityDistanceSquared(Edge edge, City city)
        {
            double l2 = Math.Pow(edge.Distance, 2);
            double t = ((city.X - edge.City1.X) * (edge.City2.X - edge.City1.X) + (city.Y - edge.City1.Y) * (edge.City2.Y - edge.City1.Y)) / l2;
            t = Math.Max(0, Math.Min(1, t));
            double px = edge.City1.X + (t * (edge.City2.X - edge.City1.X));
            double py = edge.City1.Y + (t * (edge.City2.Y - edge.City1.Y));
            return Math.Pow((city.X - px), 2) + Math.Pow((city.Y - py), 2);
        }
    }
}
