using System;
using System.Collections.Generic;
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
    }
}
