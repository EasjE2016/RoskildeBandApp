﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.ComponentModel;

namespace RoskildeBandApp.Model
{
    public class BandList : ItemsChangeObservableCollection<Band> 
    {

        public BandList()
            :base()
        {
            //sample data
            Band band1 = new Band();
            band1.BandNavn = "";


            this.Add(
                new Band() { Anmeldelse = "Wauuvvv ******",
                            BandNavn = "Police",
                            Scene = "Orange",
                            Tid = DateTime.Now });

            this.Add(new Band() { Anmeldelse = "Rigtig God ****", BandNavn = "U2", Scene = "Orange", Tid = DateTime.Now });
            this.Add(new Band() {Anmeldelse = "Fantastisk *****", BandNavn = "Pink Floyd", Scene = "Orange", Tid = new DateTime(2010,5,6) });

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

        /// <summary>
        /// metode som modtager en string af json og desorialiserer til objekter af band
        /// </summary>
        /// <param name="jsonText"></param>
        public void IndsætJson(string jsonText)
        {
            List<Band> nyListe = JsonConvert.DeserializeObject<List<Band>>(jsonText);

            foreach (var band in nyListe)
            {
                this.Add(band);
            }
        }


        /// <summary>
        /// metode til at finde om et band allerede findes i listen
        /// </summary>
        /// <param name="navn"></param>
        /// <returns></returns>
        public bool FindesNavn(string navn)
        {
            foreach (var band in this)
            {
                if (band.BandNavn == navn)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
