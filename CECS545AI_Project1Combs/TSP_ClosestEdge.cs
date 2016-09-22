using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class TSP_ClosestEdge
    {
        OutputLog log;

        Graph graph;
        City startingCity;

        private double bestRouteLength;

        public string BestRouteString
        {
            get
            {
                return generateRouteString(graph, startingCity);
            }
        }

        public string BestRouteLengthString
        {
            get
            {
                return calculateRouteLength(graph, startingCity).ToString("0.0000");
            }
        }

        public TSP_ClosestEdge(Graph inputGraph, OutputLog logIn)
        {
            log = logIn;
            graph = inputGraph;
            startingCity = graph.GetCityByID(1);
            log.writeLogMessage("Choose starting city (arbitrary) : City 1");
        }

        public void CalculateBestRoute()
        {

        }

        private static double calculateRouteLength(Graph graph, City firstCity)
        {
            double distance = 0;
            City nextCity = graph.GetEdgeByStartingCity(firstCity).City2;
            while(firstCity.CompareTo(nextCity) != 0)
            {
                Edge edge = graph.GetEdgeByStartingCity(nextCity);
                if(edge == null)
                {
                    return -1;
                }
                distance += edge.Distance;
                nextCity = edge.City2;
            }
            return distance;
        }

        private static string generateRouteString(Graph graph, City firstCity)
        {
            string route = firstCity.ID.ToString() + " -> ";
            City nextCity = graph.GetEdgeByStartingCity(firstCity).City2;
            while (firstCity.CompareTo(nextCity) != 0)
            {
                Edge edge = graph.GetEdgeByStartingCity(nextCity);
                if (edge == null)
                {
                    return "Incomplete Path!";
                }
                route += nextCity.ID.ToString() + " -> ";
                nextCity = edge.City2;
            }
            route += firstCity.ID.ToString();
            return route;
        }
    }
}
