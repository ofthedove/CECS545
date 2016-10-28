using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    class Map
    {
        private List<City> Cities;

        public int NumCities
        {
            get { return Cities.Count; }
        }

        public Map()
        {
            Cities = new List<City>();
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
