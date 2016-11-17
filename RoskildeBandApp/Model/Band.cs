using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoskildeBandApp.Model
{
    /// <summary>
    /// Klasse som indeholder fata om bands på roskildefestival 
    /// </summary>
    public class Band
    {
        public string BandNavn { get; set; }
        public string Scene { get; set; }
        public DateTime Tid { get; set; }
        public string anmeldelse { get; set; }


        public override string ToString()
        {
            return BandNavn + " " + Scene; 
        }
    }
}
