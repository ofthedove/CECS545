using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIProject4Nes
{
    public class Map
    {
        private List<City> Cities;

        public int NumCities
        {
            get { return Cities.Count; }
        }

        public Map(List<City> citiesIn)
        {
            Cities = citiesIn;
        }

        public City GetCityByID(int id)
        {
            foreach (City city in Cities)
            {
                if (city.ID == id)
                {
                    return city;
                }
            }
            return null;
        }

        public List<City> GetListOfCities()
        {
            return Cities;
        }

        public void DrawCities(Graphics g, double width, double height, int margin)
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
            double yCoordFactor = (height - margin - margin) / 100D;
            double yCoordOffset = margin - cityRadius;

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
    }
}
