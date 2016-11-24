using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using RoskildeBandApp.Model;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Popups;

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
        public RelayCommand HentDataCommand { get; private set; }

        StorageFolder localfolder = null;

        private readonly string filnavn = "JsonText.jsonNY1";

        public BandViewModel()
        {
            Bandliste = new Model.BandList();
            selectedBand = new Model.Band();
            AddBandCommand = new AddBandCommand(AddNewBand);
            NewBand = new Model.Band();
            DeleteBandCommand = new RelayCommand(DeleteBand);

            SaveBandCommand = new RelayCommand(GemDataTilDiskAsync);

            //bruger en anonym metode i min relaycommand
            DeleteAllBandCommand = new RelayCommand(()=>this.Bandliste.Clear());

            HentDataCommand = new RelayCommand(HentdataFraDiskAsync);

            localfolder = ApplicationData.Current.LocalFolder;

        }


        public async void HentdataFraDiskAsync()
        {
            try
            {
                StorageFile file = await localfolder.GetFileAsync(filnavn);
                string jsonText = await FileIO.ReadTextAsync(file);
                this.Bandliste.Clear();
                Bandliste.IndsætJson(jsonText);
            }
            catch (Exception)
            {
                MessageDialog messageDialog = new MessageDialog("Ændret filnavn eller har du ikke gemt ?", "File not found");
                await messageDialog.ShowAsync();
            }

        }

        /// <summary>
        /// Gemmer json data fra liste i localfolder
        /// </summary>
        public async void GemDataTilDiskAsync()
        {


            string jsonText = this.Bandliste.GetJson();
            StorageFile file = await localfolder.CreateFileAsync(filnavn, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, jsonText);
        }

        public void AddNewBand()
        {
            Bandliste.Add(NewBand);
        }

        public void DeleteBand()
        {
            Bandliste.Remove(SelectedBand);
        }

        private class MessageDialogHelper
        {
            public static async void Show(string content, string title)
            {
                MessageDialog messageDialog = new MessageDialog(content, title);
                await messageDialog.ShowAsync();
            }
        }



    }
}
