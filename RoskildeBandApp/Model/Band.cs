using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoskildeBandApp.Model
{
    /// <summary>
    /// Klasse som indeholder fakta om bands på roskildefestival 
    /// </summary>
    public class Band: INotifyPropertyChanged
    {
        //public string BandNavn { get; set; }
        private string _bandNavn;

        public string BandNavn
        {
            get { return _bandNavn; }
            set
            {
                _bandNavn = value;
                OnPropertyChanged(nameof(BandNavn));
            }
        }

        //public string Scene { get; set; }
        private string _scene;

        public string Scene
        {
            get { return _scene; }
            set
            {
                _scene = value;
                OnPropertyChanged(nameof(Scene));
            }
        }

        public DateTime Tid { get; set; }
        //public string Anmeldelse { get; set; }
        private string _anmeldelse;

        public string Anmeldelse
        {
            get { return _anmeldelse; }
            set
            {
                _anmeldelse = value;
                OnPropertyChanged(nameof(Anmeldelse));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return BandNavn + " " + Scene + " " + Anmeldelse;
        }

        protected virtual void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
