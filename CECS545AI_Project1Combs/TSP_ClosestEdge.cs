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
            City secondCity = graph.GetClosestCityToCity(startingCity);
            Edge firstEdge = new Edge(startingCity, secondCity);
            graph.AddEdge(firstEdge);
            log.writeResultData("Add Edge " + startingCity.ID + " - " + secondCity.ID);

            double trash;
            City thirdCity = graph.GetClosestCityToEdge(firstEdge, out trash);
            Edge secondEdge = new Edge(secondCity, thirdCity);
            graph.AddEdge(secondEdge);
            Edge thirdEdge = new Edge(thirdCity, startingCity);
            graph.AddEdge(thirdEdge);
            log.writeResultData("Add Edge " + secondCity.ID + " - " + thirdCity.ID);
            log.writeResultData("Add Edge " + thirdCity.ID + " - " + startingCity.ID);

            while (graph.NumCities > graph.NumEdges)
            {
                double curMinDistance = double.MaxValue;
                Edge curEdge = null;
                City curClosestCity = null;
                foreach(Edge edge in graph.EdgeList)
                {
                    double tempDist;
                    City tempCity = graph.GetClosestCityToEdge(edge, out tempDist);
                    if (tempDist < curMinDistance || (tempDist == curMinDistance && tempCity.ID < curClosestCity.ID))
                    {
                        curMinDistance = tempDist;
                        curClosestCity = tempCity;
                        curEdge = edge;
                    }
                }
                graph.BreakCityIntoEdge(curEdge, curClosestCity);
                log.writeResultData("Remove Edge " + curEdge.City1.ID + " - " + curEdge.City2.ID);
                log.writeResultData("Add Edge " + curEdge.City1.ID + " - " + curClosestCity.ID);
                log.writeResultData("Add Edge " + curClosestCity.ID + " - " + curEdge.City2.ID);
            }
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
