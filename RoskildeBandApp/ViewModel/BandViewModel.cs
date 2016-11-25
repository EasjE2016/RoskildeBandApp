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


        public AddBandCommand AddBandCommand { get; set; }

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

        private string _listeJsonText;

        public string ListeJsonText
        {
            get { return _listeJsonText; }
            set
            {
                _listeJsonText = value;
                OnPropertyChanged(nameof(ListeJsonText));
            }
        }


        private Band selectedBand;
        public Band SelectedBand
        {
            get { return selectedBand; }
            set
            {
                selectedBand = value;
                OnPropertyChanged(nameof(SelectedBand));
            }
        }

        public string BandNavn { get; set; }
        public string Scene { get; set; }
        public DateTime Tid { get; set; }
        public string anmeldelse { get; set; }
        public string Band { get; set; }

        public RelayCommand DeleteBandCommand { get; private set; }
        public RelayCommand SaveBandCommand { get; private set; }
        public RelayCommand HentBandCommand { get; private set; }
        public RelayCommand DeleteAllBandCommand { get; private set; }
        public RelayCommand HentDataCommand { get; private set; }

        StorageFolder localfolder = null;
        private readonly string filnavn = "JsonText.json";

        /// <summary>
        /// default constructor 
        /// </summary>
        public BandViewModel()
        {
            Tid = new DateTime();
            Bandliste = new BandList();
            selectedBand = new Band();
            AddBandCommand = new AddBandCommand(AddNewBand);
            DeleteBandCommand = new RelayCommand(DeleteBand);
            SaveBandCommand = new RelayCommand(GemDataTilDiskAsync);

            //bruger en anonym metode i min relaycommand
            DeleteAllBandCommand = new RelayCommand(()=>this.Bandliste.Clear());
            HentDataCommand = new RelayCommand(HentdataFraDiskAsync);
            localfolder = ApplicationData.Current.LocalFolder;

            this.ListeJsonText = this.Bandliste.GetJson();
        }

        /// <summary>
        /// Henter data fra localfolder
        /// Der skrives en fejlmeddelse i en 
        /// messageDialog hvis filen ikke findes
        /// </summary>
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
            this.ListeJsonText = this.Bandliste.GetJson();
            StorageFile file = await localfolder.CreateFileAsync(filnavn, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, this.ListeJsonText);
        }


        /// <summary>
        /// Tak til Filips far :-) og Filip som havde løsningen og Rudi som 
        /// insisterede på at der var en fejl !
        /// </summary>
        public void AddNewBand()
        {
            //nyt objekt ud fra de værdier som er i de fire properties som
            //er bundet til mit view
            Band addBand = new Band()
            {
                BandNavn = this.BandNavn,
                Scene = this.Scene,
                anmeldelse = this.anmeldelse,
             };
            Bandliste.Add(addBand);
            this.ListeJsonText = this.Bandliste.GetJson();
        }

        public void DeleteBand()
        {
            Bandliste.Remove(SelectedBand);
            this.ListeJsonText = this.Bandliste.GetJson();
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
