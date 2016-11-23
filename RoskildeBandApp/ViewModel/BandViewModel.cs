using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using RoskildeBandApp.Model;
using Newtonsoft.Json;
using Windows.Storage;

namespace RoskildeBandApp.ModelView
{
    class BandViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        StorageFolder localFolder = null;
        public AddBandCommand AddBandCommand { get; set; }

        //public BandList Bandliste { get; set; }
        //i et backing field for en property er det ok at bruge _(underscore)
        private BandList _bandliste;
        public BandList Bandliste
        {
            get { return _bandliste; }
            set
            {
                _bandliste = value;
                OnPropertyChanged(nameof(Bandliste));
            }
        }


        private Band selectedBand;
        private readonly string filename = "JsonText.json";

        public Model.Band SelectedBand
        {
            get { return selectedBand; }
            set { selectedBand = value;
                OnPropertyChanged(nameof(SelectedBand));
            }
        }


        public Band NewBand { get; set; }
        public RelayCommand DeleteBandCommand { get; private set; }
        public RelayCommand SaveBandCommand { get; private set; }
        public RelayCommand HentBandCommand { get; private set; }
        public RelayCommand DeleteAllBandCommand { get; private set; }
        public RelayCommand GemBandCommand { get; private set; }
        public RelayCommand AddTestDataCommand { get; private set; }

        public BandViewModel()
        {
            Bandliste = new Model.BandList();
            selectedBand = new Model.Band();
            AddBandCommand = new AddBandCommand(AddNewBand);
            NewBand = new Model.Band();
            DeleteBandCommand = new RelayCommand(DeleteBand);

            //bruger en anonym metode i min relaycommand "()=>GemTilDisk()"
            DeleteAllBandCommand = new RelayCommand(ClearList);
            AddTestDataCommand = new RelayCommand(() => this.AddTestData());
            HentBandCommand = new RelayCommand(() => this.HentFraDisk());
            SaveBandCommand = new RelayCommand(() => GemTilDisk());


            localFolder = ApplicationData.Current.LocalFolder;

            //AddTestData();
        }

        public void AddNewBand()
        {
            Bandliste.Add(NewBand);
        }

        public void ClearList()
        {
            this.Bandliste.Clear();
        }

        public void DeleteBand()
        {
            Bandliste.Remove(SelectedBand);
        }

        public async void GemTilDisk()
        {
            string json = Bandliste.GetJson();
            StorageFile file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        public async void HentFraDisk()
        {
            this.Bandliste.Clear();
            StorageFile file = await localFolder.GetFileAsync(filename);
            string jsonText = await FileIO.ReadTextAsync(file);
            List<Band> nylist = JsonConvert.DeserializeObject<List<Band>>(jsonText);
            Bandliste.InjectJson(jsonText);
        }

        public void AddTestData()
        {
            this.Bandliste.Add(
                new Band()
                {
                    anmeldelse = "",
                    BandNavn = "Police",
                    Scene = "Orange",
                    Tid = DateTime.Now
                });

            this.Bandliste.Add(new Band() { anmeldelse = "", BandNavn = "U2", Scene = "Orange", Tid = DateTime.Now });
            this.Bandliste.Add(new Band() { anmeldelse = "", BandNavn = "Pink Floyd", Scene = "Orange", Tid = new DateTime(2010, 5, 6) });
        }


    }
}
