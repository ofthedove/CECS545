using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CECS545AI_Project1Combs
{
    enum GeneStrandType
    {
        CityAsValue,
        OrderAsValue
    }

    class PopulationProperties
    {
        private Map map;
        private GeneStrandType geneStrandType;
        private bool individualsAreDoubleStrand;

        public Map Map
        {
            get { return map; }
        }
        public GeneStrandType GeneStrandType
        {
            get { return geneStrandType; }
        }
        public bool IndividualsAreDoubleStrand
        {
            get { return individualsAreDoubleStrand; }
        }
        public int MaxGeneValue
        {
            get
            {
                switch(GeneStrandType)
                {
                    case GeneStrandType.CityAsValue:
                        return map.NumCities;
                    case GeneStrandType.OrderAsValue:
                        return map.NumCities * 10;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
