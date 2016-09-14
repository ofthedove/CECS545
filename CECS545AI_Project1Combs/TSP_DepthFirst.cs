using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TO DO: make this a real class. Seriously. Things are getting ugly...
using City = System.Collections.Generic.KeyValuePair<int, System.Tuple<double, double, System.Collections.Generic.List<System.Tuple<int, double>>>>;
/* KeyValuePair - City
 * {
 *      int id
 *      tuple - data
 *      {
 *          double xCoord
 *          double yCoord
 *          list - connections
 *          {
 *              tuple - connection
 *              {
 *                  int id
 *                  double distance
 *              }
 *          }
 *      }
 * }
 */

namespace CECS545AI_Project1Combs
{
    class TSP_DepthFirst
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

        public TSP_DepthFirst(string inputDataString, string connectionsDataString, OutputLog outputLogParam) : this(parseInputString(inputDataString, connectionsDataString), outputLogParam) { }

        public TSP_DepthFirst(List<City> inputData, OutputLog outputLogParam)
        {
            inputCityList = inputData;
            outputLog = outputLogParam;
            bestRoute = new List<City>();
            bestRouteLength = double.MaxValue;
        }

        public void CalculateBestRoute()
        {
            if (inputCityList.Count == 0 || inputCityList == null)
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

            List<Node> graph = new List<Node>();

            foreach (City iCity in inputCityList)
            {
                Node node = new Node() { city = iCity, distance = -1, parent = -1 };
                graph.Add(node);
            }

            Stack<Node> stack = new Stack<Node>();

            Node current;
            Node root = getNodeFromListByID(graph, 1);
            root.distance = 0;

            stack.Push(root);

            while (stack.Count > 0)
            {
                current = stack.Pop();
                for (int i = 0; i < graph.Count; i++)
                {
                    Node node = graph[i];
                    if (areCititesConnected(current.city, node.city))
                    {
                        if (node.distance == -1 || node.distance > current.distance + distanceBetween(current.city, node.city))
                        {
                            node.distance = current.distance + distanceBetween(current.city, node.city);
                            node.parent = current.city.Key;
                            graph[i] = node;
                            stack.Push(node);
                        }
                    }
                }
            }

            // determine the goal city (the one with the highest id)
            Node goal = new Node();
            goal.city = new City(0, new Tuple<double, double, List<Tuple<int, double>>>(0, 0, new List<Tuple<int, double>>()));
            foreach (Node node in graph)
            {
                if (node.city.Key > goal.city.Key)
                {
                    goal = node;
                }
            }

            // Backtrack the best route
            Stack<Node> bestRouteBuilder = new Stack<Node>();
            bestRouteBuilder.Push(goal);
            while (bestRouteBuilder.Peek().city.Key != 1)
            {
                bestRouteBuilder.Push(getNodeFromListByID(graph, bestRouteBuilder.Peek().parent));
            }

            // Put the best route in order
            bestRoute = new List<City>();
            while (bestRouteBuilder.Count > 0)
            {
                bestRoute.Add(bestRouteBuilder.Pop().city);
            }
            bestRouteLength = goal.distance;
        }

        private static List<City> parseInputString(string inputDataString, string connectionsDataString)
        {
            List<City> inputData = new List<City>();
            Dictionary<int, List<int>> connectionsData = new Dictionary<int, List<int>>();

            // Clean up input data
            inputDataString = inputDataString.Substring(inputDataString.IndexOf("NODE_COORD_SECTION") + "NODE_COORD_SECTION".Length);
            inputDataString = inputDataString.Trim();

            // Build the list of cities
            foreach (string line in inputDataString.Split('\n'))
            {
                string[] values = line.Trim().Split(' ');
                if (values.Length != 3)
                {
                    throw new ArgumentException("Invalid input data! Line does not contain three values. Bad Line: " + line, "inputDataString");
                }

                City newCity = new City(Convert.ToInt32(values[0]), new Tuple<double, double, List<Tuple<int, double>>>(Convert.ToDouble(values[1]), Convert.ToDouble(values[2]), new List<Tuple<int, double>>()));

                inputData.Add(newCity);
            }

            // Build a dictionary of connection data
            foreach (string line in connectionsDataString.Split('\n'))
            {
                int curID = Convert.ToInt32(line.Trim().Split(' ')[0].Trim());

                List<int> destinationList = new List<int>();
                if (line.Trim().Split(' ').Count() > 1)
                {
                    foreach (string destinationID in line.Trim().Split(' ')[1].Trim().Split(','))
                    {
                        getCityFromListByID(inputData, curID).Value.Item3.Add(new Tuple<int, double>(Convert.ToInt32(destinationID), distanceBetween(inputData, curID, Convert.ToInt32(destinationID))));
                    }
                }
            }



            return inputData;
        }

        private static double distanceBetween(City city1, City city2)
        {
            double city1_X = city1.Value.Item1;
            double city1_Y = city1.Value.Item2;
            double city2_X = city2.Value.Item1;
            double city2_Y = city2.Value.Item2;

            // d = sqrt( (x2-x1)^2 + (y2-y1)^2 )
            return Math.Sqrt(Math.Pow((city2_X = city1_X), 2) + Math.Pow((city2_Y - city1_Y), 2));
        }

        private static double distanceBetween(List<City> cityList, int city1ID, int city2ID)
        {
            City city1 = getCityFromListByID(cityList, city1ID);
            City city2 = getCityFromListByID(cityList, city2ID);
            double city1_X = city1.Value.Item1;
            double city1_Y = city1.Value.Item2;
            double city2_X = city2.Value.Item1;
            double city2_Y = city2.Value.Item2;

            // d = sqrt( (x2-x1)^2 + (y2-y1)^2 )
            return Math.Sqrt(Math.Pow((city2_X = city1_X), 2) + Math.Pow((city2_Y - city1_Y), 2));
        }

        private bool areCititesConnected(City sourceCity, City destinationCity)
        {
            foreach (Tuple<int, double> connection in sourceCity.Value.Item3)
            {
                if (connection.Item1 == destinationCity.Key)
                {
                    return true;
                }
            }
            return false;
        }

        private static City getCityFromListByID(List<City> list, int id)
        {
            foreach (City city in list)
            {
                if (city.Key == id)
                    return city;
            }
            return new City(); // Don't try this at work, kids...
        }

        private static Node getNodeFromListByID(List<Node> list, int id)
        {
            foreach (Node node in list)
            {
                if (node.city.Key == id)
                    return node;
            }
            return new Node(); // Don't try this at work, kids...
        }

        private string routeToString(List<City> route)
        {
            string result = "";
            foreach (City city in route)
            {
                result += city.Key + " -> ";
            }
            return result.Substring(0, result.Length - 4); ;
        }

        private struct Node
        {
            public City city;
            public double distance;
            public int parent;
        }
    }
}
