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
            //Band band1 = new Band();
            //band1.BandNavn = "";

            AddTestData();

        }

        /// <summary>
        /// tilføjer testdata
        /// </summary>
        public void AddTestData()
        {
            this.Add(
                new Band()
                {
                    anmeldelse = "",
                    BandNavn = "Police",
                    Scene = "Orange",
                    Tid = DateTime.Now
                });

            this.Add(new Band() { anmeldelse = "", BandNavn = "U2", Scene = "Orange", Tid = DateTime.Now });
            this.Add(new Band() { anmeldelse = "", BandNavn = "Pink Floyd", Scene = "Orange", Tid = new DateTime(2010, 5, 6) });
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// sender en json text som bliver indsat i listen
        /// </summary>
        /// <param name="jsonText">json text</param>
        public void InjectJson(string jsonText)
        {

            //TODO fejlhåndtering 
            //this./*ClearItems*/();
            this.Clear();

            foreach (var band in JsonConvert.DeserializeObject<List<Band>>(jsonText))
            {
                this.Add(band);
            }




        }

    }
}
