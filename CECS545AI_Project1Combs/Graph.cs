using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    public class Graph
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
            AddEdge(new Edge(edge.City1, city));
            AddEdge(new Edge(city, edge.City2));
        }

        public City GetClosestCityToCity(City cityIn)
        {
            double curMinDistance = double.MaxValue;
            City curClosestCity = null;
            foreach(City city in UnusedCities)
            {
                if(city.CompareTo(cityIn) == 0)
                {
                    continue;
                }
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
                if(edge.City1.CompareTo(city) == 0 || edge.City2.CompareTo(city) == 0)
                {
                    continue;
                }
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
            City v = edge.City1;
            City w = edge.City2;
            City p = city;
            double l2 = Math.Pow((edge.Distance), 2);
            if (l2 == 0.0) return Math.Sqrt(Math.Pow((v.X - p.X), 2) + Math.Pow((v.Y - p.Y), 2));
            double t = Math.Max(0, Math.Min(1, ((((p.X - v.X) * (w.X - v.X)) + ((p.Y - v.Y) * (w.Y - v.Y))) / l2)));
         
            double projx = v.X + t * (w.X - v.X);
            double projy = v.Y + t * (w.Y - v.Y);

            return Math.Sqrt(Math.Pow((projx - p.X), 2) + Math.Pow((projy - p.Y), 2));
        }
    }
}
