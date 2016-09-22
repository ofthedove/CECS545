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
        private List<Edge> Edges;

        public Graph()
        {
            Cities = new List<City>();
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
    }
}
