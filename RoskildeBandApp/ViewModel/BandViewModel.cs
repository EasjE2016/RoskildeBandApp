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

            //bruger en anonym metode i min relaycommand
            DeleteAllBandCommand = new RelayCommand(() => this.Bandliste.Clear());
            AddTestDataCommand = new RelayCommand(() => this.Bandliste.AddTestData());

            SaveBandCommand = new RelayCommand(() => GemTilDisk());


            localFolder = ApplicationData.Current.LocalFolder;
        }

        public void AddNewBand()
        {
            Bandliste.Add(NewBand);
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




    }
}
