using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Map_Bul_App.Annotations;
using TK.CustomMap;
using Xamarin.Forms.Maps;

namespace Map_Bul_App.Models
{
    public  class LoadedPins:INotifyPropertyChanged
    {
        public LoadedPins()
        {
            _pins= new List<PinDescriptor>();
            _mapRegions= new List<MapSpan>();

        }
        private  List<PinDescriptor> _pins;
        private List<MapSpan> _mapRegions;

        public List<PinDescriptor> Pins
        {
            get => _pins;
            set
            {
                if (_pins == value) return;
                _pins = value;
                OnPropertyChanged(nameof(Pins));
            }
        }
        public List<MapSpan> MapRegions
        {
            get => _mapRegions;
            set
            {
                if (_mapRegions == value) return;
                _mapRegions = value;
                OnPropertyChanged();
            }
        }

        
        public List<TKCustomMapPin> MapPins => Pins.Select(item => item.Pin).ToList(); 

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
