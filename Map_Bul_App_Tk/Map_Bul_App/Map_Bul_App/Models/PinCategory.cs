using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Map_Bul_App.Annotations;
using Xamarin.Forms;

namespace Map_Bul_App.Models
{
    public class PinCategory : INotifyPropertyChanged
    {
        public event EventHandler<int> ItemTapped;
        public int Id { get; set; }
        public int? ParentId { get; set; }
        private bool _itemSelected;
        private bool _isVisible;
        public bool IsVisible 
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value) return;
                _isVisible = value;
                OnPropertyChanged();
            }
        }
        public bool ItemSelected
        {
            get { return _itemSelected; }
            set
            {
                if (_itemSelected == value) return;
                _itemSelected = value;
                OnPropertyChanged();
            }
        }

        private ImageSource _icon;

        public ImageSource Icon {
            get { return _icon; }
            set
            {
                if(value==_icon) return;
                _icon = value;
                OnPropertyChanged();
            } }

        private ImageSource _pinIcon;

        public ImageSource PinIcon
        {
            get { return _pinIcon; }
            set
            {
                if (value == _pinIcon) return;
                _pinIcon = value;
                OnPropertyChanged();
            }
        }
        public Color Color { get; set; }
        public string Name { get; set; }
        public List<int> CategoriesBranch { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnItemTapped()
        {
            if (IsVisible)
            ItemTapped?.Invoke(this, Id);
        }
    }
}
