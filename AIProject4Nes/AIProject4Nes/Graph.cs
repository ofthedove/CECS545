using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIProject4Nes
{
    class Graph
    {
        private Map map;
        //private List<Edge> edges;

        public Graph(Map mapIn, int[] solution)
        {
            map = mapIn;
            /*
            Edge edge;
            for (int i = 1; i < solution.Length; i++)
            {
                edge = new Edge();
                edge.startID = solution[i - 1];
                edge.endID = solution[i];
                edges.Add(edge);
            }
            edge = new Edge();
            edge.startID = solution[solution.Length - 1];
            edge.endID = solution[0];
            edges.Add(edge);*/
        }

        public static Bitmap GenerateGraphImage(Map map, int[] solution)
        {
            Bitmap bmp = new Bitmap(1000, 1000);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                // Draw Edges
                DrawEdges(map, solution, g, bmp.Width, bmp.Height, 10);

                // Draw Nodes
                map.DrawCities(g, bmp.Width, bmp.Height, 10);
            }
            return bmp;
        }

        public static void DrawEdges(Map map, int[] solution, Graphics g, double width, double height, int margin)
        {
            SolidBrush b = new SolidBrush(Color.Red);
            Pen p = new Pen(b, 2);

            double xCoordFactor = (width - margin - margin) / 100D;
            double xCoordOffset = margin;
            double yCoordFactor = (height - margin - margin) / 100D;
            double yCoordOffset = margin;

            if(solution.Length < 2)
            {
                return; // need at least two for this to work
            }

            int i = 0;
            City city1 = map.GetCityByID(solution[i++]);
            City city2 = map.GetCityByID(solution[i++]);
            while (i <= solution.Length)
            {
                int startXCoord = (int)((city1.X * xCoordFactor) + xCoordOffset);
                int startYCoord = (int)((city1.Y * yCoordFactor) + yCoordOffset);
                int endXCoord = (int)((city2.X * xCoordFactor) + xCoordOffset);
                int endYCoord = (int)((city2.Y * yCoordFactor) + yCoordOffset);

                g.DrawLine(p, startXCoord, startYCoord, endXCoord, endYCoord);

                city1 = city2;
                city2 = map.GetCityByID(solution[i++ % solution.Length]);
            }
        }

        private struct Edge
        {
            public int startID;
            public int endID;
        }
    }
}
