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
using System.Windows.Input;


namespace RoskildeBandApp.ModelView
{
    class BandViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public RelayCommand AddBandCommand { get; set; }

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

        public string jsonText
        {
            get { return _listeJsonText; }
            set
            {
                _listeJsonText = value;
                OnPropertyChanged(nameof(jsonText));
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

        private List<string> jsonstexts;

        public List<string> JsonTexts
        {
            get { return jsonstexts; }
            set
            {
                jsonstexts = value;
                OnPropertyChanged(nameof(JsonTexts));
            }
        }

        private string bandNavn;

        public string BandNavn
        {
            get { return bandNavn; }
            set
            {
                bandNavn = value;
                OnPropertyChanged(nameof(BandNavn));
            }
        }

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

        public DateTime Tid { get; set; }
        //public string Band { get; set; }

        public ICommand DeleteBandCommand { get; private set; }
        public ICommand SaveBandCommand { get; private set; }
        public ICommand HentBandCommand { get; private set; }
        public ICommand DeleteAllBandCommand { get; private set; }
        public ICommand HentDataCommand { get; private set; }

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
            AddBandCommand = new RelayCommand(AddNewBand,AddBandCanExecute);
            DeleteBandCommand = new RelayCommand(DeleteBand,DeleteBanCanExecute);
            SaveBandCommand = new RelayCommand(GemDataTilDiskAsync);

            //bruger en anonym metode i min relaycommand DeleteAllBandCommand
            DeleteAllBandCommand = new RelayCommand(()=>this.Bandliste.Clear(),()=>this.Bandliste.Count > 0);

            HentDataCommand = new RelayCommand(HentdataFraDiskAsync);
            localfolder = ApplicationData.Current.LocalFolder;

            this.jsonText = this.Bandliste.GetJson();
            this.bandNavn = string.Empty;

            //splitter json text strengen på "}"
            string delimStr = "}";
            char[] delimiter = delimStr.ToCharArray();

            this.JsonTexts = this.jsonText.Split(delimiter).ToList();

        }

        /// <summary>
        /// metode som returnerer false hvis listen er tom
        /// bruges i mit kald til relaycommand så kanppen "Slet"
        /// er inaktiv hvis listen er tom
        /// </summary>
        /// <returns></returns>
        private bool DeleteBanCanExecute()
        {
            return this.Bandliste.Count > 0;
        }

        private bool AddBandCanExecute()
        {
            return this.bandNavn != string.Empty;
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
            this.jsonText = this.Bandliste.GetJson();
            StorageFile file = await localfolder.CreateFileAsync(filnavn, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, this.jsonText);
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
                anmeldelse = this.Anmeldelse,
             };
            Bandliste.Add(addBand);
            this.jsonText = this.Bandliste.GetJson();
            this.BandNavn = string.Empty;
            this.Anmeldelse = string.Empty;
            this.Scene = string.Empty;
            
        }

        public void DeleteBand()
        {
            Bandliste.Remove(SelectedBand);
            this.jsonText = this.Bandliste.GetJson();
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
