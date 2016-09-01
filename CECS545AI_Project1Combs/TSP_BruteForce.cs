using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using City = System.Collections.Generic.KeyValuePair<int, System.Tuple<double, double>>;
/* KeyValuePair - City
 * {
 *      int id
 *      tuple - CityCoordinates
 *      {
 *          double xCoord
 *          double yCoord
 *      }
 * }
 */

namespace CECS545AI_Project1Combs
{
    class TSP_BruteForce
    {
        private List<City> inputCityList;
        private OutputLog outputLog;

        private List<City> bestRoute;
        private double bestRouteLength;

        private int debug_curDepth = 0;

        public string BestRouteString
        {
            get
            {
                return routeToString(bestRoute);
            }
        }

        public string BestRouteLengthString
        {
            get
            {
                return bestRouteLength.ToString("0.0000");
            }
        }

        public TSP_BruteForce(string inputDataString, OutputLog outputLogParam) : this(parseInputString(inputDataString), outputLogParam) { }

        public TSP_BruteForce(List<City> inputData, OutputLog outputLogParam)
        {
            inputCityList = inputData;
            outputLog = outputLogParam;
            bestRoute = new List<City>();
            bestRouteLength = double.MaxValue;
        }

        public void CalculateBestRoute()
        {
            if(inputCityList.Count == 0 || inputCityList == null)
            {
                bestRouteLength = 0;
                return;
            }

            if (inputCityList.Count == 1)
            {
                bestRoute.Add(inputCityList[0]);
                bestRouteLength = 0;
                return;
            }

            List<City> currentRoute = new List<City>();
            double currentRouteDistance = 0;
            List<City> remainingCities = new List<City>(inputCityList);

            City currentCity = remainingCities[0];

            currentRoute.Add(currentCity);
            remainingCities.Remove(currentCity);

            CalculateBestRouteInner(currentRoute, remainingCities, currentRouteDistance);
        }

        private void CalculateBestRouteInner(List<City> currentRoute, List<City> remainingCities, double currentRouteDistance)
        {
            debug_curDepth++;

            if(remainingCities.Count == 0)
            {
                currentRouteDistance += distanceBetween(currentRoute.Last(), currentRoute.First());
                currentRoute.Add(currentRoute.First());

                bool isCurrentBest = (currentRouteDistance < bestRouteLength);
                if(isCurrentBest)
                {
                    bestRouteLength = currentRouteDistance;
                    bestRoute = currentRoute;
                    outputLog.writeResultData("Route Found.  " + (isCurrentBest ? "CB" : "  ") + "  Distance: " + currentRouteDistance.ToString("0.0000") + "  Route: " + routeToString(currentRoute));
                }

                debug_curDepth--;
                return;
            }

            foreach(City currentCity in remainingCities)
            {
                double innerRouteDistance = currentRouteDistance + distanceBetween(currentRoute.Last(), currentCity);

                List<City> innerRoute = new List<City>(currentRoute);
                List<City> innerRemainingCities = new List<City>(remainingCities);

                innerRoute.Add(currentCity);
                innerRemainingCities.Remove(currentCity);

                CalculateBestRouteInner(innerRoute, innerRemainingCities, innerRouteDistance);
            }

            debug_curDepth--;
        }

        private static List<City> parseInputString(string inputDataString)
        {
            List<City> inputData = new List<City>();

            inputDataString = inputDataString.Substring(inputDataString.IndexOf("NODE_COORD_SECTION") + "NODE_COORD_SECTION".Length);
            inputDataString = inputDataString.Trim();
            
            foreach(string line in inputDataString.Split('\n'))
            {
                string[] values = line.Trim().Split(' ');
                if(values.Length != 3)
                {
                    throw new ArgumentException("Invalid input data! Line does not contain three values. Bad Line: " + line, "inputDataString");
                }

                City newCity = new City(Convert.ToInt32(values[0]), new Tuple<double, double>(Convert.ToDouble(values[1]), Convert.ToDouble(values[2])));

                inputData.Add(newCity);
            }

            return inputData;
        }

        private double distanceBetween(City city1, City city2)
        {
            double city1_X = city1.Value.Item1;
            double city1_Y = city1.Value.Item2;
            double city2_X = city2.Value.Item1;
            double city2_Y = city2.Value.Item2;

            // d = sqrt( (x2-x1)^2 + (y2-y1)^2 )
            return Math.Sqrt(Math.Pow((city2_X = city1_X), 2) + Math.Pow((city2_Y - city1_Y), 2));
        }

        private string routeToString(List<City> route)
        {
            string result = "";
            foreach(City city in route)
            {
                result += city.Key + " -> ";
            }
            return result.Substring(0, result.Length - 4); ;
        }
    }
}
