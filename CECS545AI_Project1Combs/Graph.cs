using System;
using System.Collections.Generic;
using System.Drawing;
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

        public bool RemoveEdge(Edge edge)
        {
            if (Edges.Contains(edge))
            {
                // Remove the edge
                Edges.Remove(edge);

                // If city 1 is now not in use, add it to unused cities
                bool cityIsInUse = false;
                foreach(Edge curEdge in Edges)
                {
                    if(edge.City1 == curEdge.City2)
                    {
                        cityIsInUse = true;
                        break;
                    }
                }
                if(!cityIsInUse)
                {
                    UnusedCities.Add(edge.City1);
                }

                // If city 2 is now not in use, add it to unused cities
                foreach (Edge curEdge in Edges)
                {
                    if (edge.City2 == curEdge.City1)
                    {
                        cityIsInUse = true;
                        break;
                    }
                }
                if (!cityIsInUse)
                {
                    UnusedCities.Add(edge.City2);
                }

                // Return success
                return true;
            }
            else
            {
                // The edge cannot be removed because it is not here. Return failure
                return false;
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
            RemoveEdge(edge);
            AddEdge(new Edge(edge.City1, city));
            AddEdge(new Edge(city, edge.City2));
        }

        public bool RemoveCityFromEdges(Edge edge1, Edge edge2)
        {
            if(edge1.City2 != edge2.City1)
            {
                return false;
            }
            else
            {
                RemoveEdge(edge1);
                RemoveEdge(edge2);
                AddEdge(new Edge(edge1.City1, edge2.City2));
                return true;
            }
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

        public void DrawCities(Graphics g, double width, double height, double vertOffset, int margin)
        {
            SolidBrush outlineBrush = new SolidBrush(Color.Black);
            SolidBrush fillBrush = new SolidBrush(Color.White);
            Pen p = new Pen(outlineBrush, 2);
            Font f = new Font(FontFamily.GenericMonospace, 10, GraphicsUnit.Pixel);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            int cityRadius = (int)f.Size;
            double xCoordFactor = (width - margin - margin) / 100D;
            double xCoordOffset = margin - cityRadius;
            double yCoordFactor = (height - vertOffset - margin - margin) / 100D;
            double yCoordOffset = vertOffset + margin - cityRadius;

            foreach (City city in Cities)
            {
                double cityXCoord = city.X * xCoordFactor;
                double cityYCoord = city.Y * yCoordFactor;
                Rectangle rect = new Rectangle((int)(cityXCoord + xCoordOffset), (int)(cityYCoord + yCoordOffset), cityRadius * 2, cityRadius * 2);

                g.FillEllipse(fillBrush, rect);
                g.DrawEllipse(p, rect);
                rect.Height = cityRadius;
                rect.Y += cityRadius / 2;
                g.DrawString(city.ID.ToString(), f, outlineBrush, rect, sf);
            }
        }

        public void DrawEdges(Graphics g, double width, double height, double vertOffset, int margin)
        {
            SolidBrush b = new SolidBrush(Color.Red);
            Pen p = new Pen(b, 2);
            
            double xCoordFactor = (width - margin - margin) / 100D;
            double xCoordOffset = margin;
            double yCoordFactor = (height - vertOffset - margin - margin) / 100D;
            double yCoordOffset = vertOffset + margin;

            foreach (Edge edge in Edges)
            {
                int startXCoord = (int)((edge.City1.X * xCoordFactor) + xCoordOffset);
                int startYCoord = (int)((edge.City1.Y * yCoordFactor) + yCoordOffset);
                int endXCoord = (int)((edge.City2.X * xCoordFactor) + xCoordOffset);
                int endYCoord = (int)((edge.City2.Y * yCoordFactor) + yCoordOffset);

                g.DrawLine(p, startXCoord, startYCoord, endXCoord, endYCoord);
            }
        }
    }
}
