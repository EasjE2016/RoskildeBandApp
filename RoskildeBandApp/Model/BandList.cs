using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace RoskildeBandApp.Model
{
    public class BandList : ObservableCollection<Band>
    {
        public BandList()
            :base()
        {
            //sample data
            Band band1 = new Band();
            band1.BandNavn = "";


            this.Add(
                new Band() { anmeldelse = "",
                            BandNavn = "Police",
                            Scene = "Orange",
                            Tid = DateTime.Now });

            this.Add(new Band() { anmeldelse = "", BandNavn = "U2", Scene = "Orange", Tid = DateTime.Now });
            this.Add(new Band() {anmeldelse = "", BandNavn = "Pink Floyd", Scene = "Orange", Tid = new DateTime(2010,5,6) });

        }

        /// <summary>
        /// Giver mig Jsonformat for BandList object
        /// </summary>
        /// <returns></returns>
        public string GetJson()
        {
            string json = JsonConvert.SerializeObject(this);
            return json;
        }

    }
}
