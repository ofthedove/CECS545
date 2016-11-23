using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIProject4Nes
{
    class MapBuilder
    {
        protected List<City> Cities;
        
        public MapBuilder()
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

        public Map ToMap()
        {
            return new Map(Cities); ;
        }
    }
}
