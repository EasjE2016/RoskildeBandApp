using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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

        public Model.BandList Bandliste { get; set; }

        private Model.Band selectedBand;


        public Model.Band SelectedBand
        {
            get { return selectedBand; }
            set { selectedBand = value;
                OnPropertyChanged(nameof(SelectedBand));
            }
        }

        public Model.Band NewBand { get; set; }


        public BandViewModel()
        {
            Bandliste = new Model.BandList();
            selectedBand = new Model.Band();
            AddBandCommand = new RelayCommand(AddNewBand);
        }
        public RelayCommand AddBandCommand { get; set; }

        public void AddNewBand()
        {
            Bandliste.Add(NewBand);
        }

    }
}
