using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
 * */

namespace CECS545AI_Project1Combs
{
    class City
    {
        private int id;
        private double xCoord;
        private double yCoord;
        private List<Edge> edges;
        private bool locked;

        public int ID
        {
            get
            {
                return id;
            }
        }
        public int X
        {
            get
            {
                return xCoord;
            }
        }
        public int Y
        {
            get
            {
                return yCoord;
            }
        }
        public List<Edge> Edges
        {
            get
            {
                return edges;
            }
        }

        public City(int idIn, double xCoordIn, double yCoordIn)
        {

        }

        public void Lock()
        {
            locked = true;
        }

        public bool AddEdge(City city)
        {
            if(!locked)
            {
                Edge edge = new Edge();
                return AddEdge(edge);
            }
            return false;
        }

        public bool AddEdge(Edge edge)
        {
            if (!locked)
            {
                edges.Add(edge);
                return true;
            }
            return false;
        }

        public void SelectEdge(City city)
        {
            Edge edge = new Edge(this, city);
            SelectEdge(edge);
        }

        public void SelectEdge(Edge edge)
        {
            
        }

        public void DeselectEdge(City city)
        {
            Edge edge = new Edge();
            DeselectEdge(edge);
        }

        public void DeselectEdge(Edge edge)
        {

        }
    }
}
