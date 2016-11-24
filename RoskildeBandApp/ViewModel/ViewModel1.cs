using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoskildeBandApp.Model;
using Newtonsoft.Json;

namespace RoskildeBandApp.ViewModel
{
    public class ViewModel1
    {
        public BandList minNyeBandliste { get; set; }

        public ViewModel1()
        {
            string json = this.GetBandlisteasJson();

            string json2 = minNyeBandliste.GetJson();

        }


        public string GetBandlisteasJson()
        {
            string jsonText = JsonConvert.SerializeObject(minNyeBandliste);
            return jsonText;
        }

    }


}
}
