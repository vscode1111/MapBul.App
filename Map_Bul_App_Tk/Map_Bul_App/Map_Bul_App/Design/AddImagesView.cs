using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Map_Bul_App.ResX;
using Map_Bul_App.Settings;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public class AddImagesView : Grid
    {
        private const int Spacing = 5;
        private NewPinImage[] _images;

        public AddImagesView()
        {
            PropertyChanged += AddImagesView_PropertyChanged;
            ImagePaths = new ObservableCollection<string>
            {
                string.Empty
            };
            //ImagePaths.CollectionChanged += ImagePaths_CollectionChanged;
            InitilizeGrid();
            AddImagesToGrid();
            InitilizeFirstImage();
        }

        private Dictionary<string, string> _photosBase64Dictionary = new Dictionary<string, string>();

        private void ImagePaths_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ImagePaths.Count(i => !string.IsNullOrEmpty(i)) > 0)
            {
                if (ImagePaths.Count(i => !string.IsNullOrEmpty(i)) > _photosBase64Dictionary.Count)
                {
                    var fileProvider = DependencyService.Get<IFileSystemWork>();
                    foreach (
                        var newPhotoPath in ImagePaths.Where(path => _photosBase64Dictionary.All(b64 => b64.Key != path)))
                    {
                        if (!newPhotoPath.Contains("http://185.76.145.214/") && !string.IsNullOrEmpty(newPhotoPath))
                        {
                            var tempBase64Photo = Convert.ToBase64String(fileProvider.GetFile(newPhotoPath));
                            _photosBase64Dictionary.Add(newPhotoPath, tempBase64Photo);
                            PhotosBase64Dictionary.Add(tempBase64Photo);
                        }
                        else if(!string.IsNullOrEmpty(newPhotoPath))
                        {
                            _photosBase64Dictionary.Add(newPhotoPath, newPhotoPath);
                            PhotosBase64Dictionary.Add(newPhotoPath);
                        }
                    }
                }
                else if (ImagePaths.Count(i=>!string.IsNullOrEmpty(i)) < _photosBase64Dictionary.Count)
                {
                    var tempToRemove = _photosBase64Dictionary.Where(b64 => !ImagePaths.Contains(b64.Key))
                        .Select(b64 => b64.Key);
                    foreach (var itemToDelete in tempToRemove)
                    {
                        PhotosBase64Dictionary.Remove(_photosBase64Dictionary.First(i => i.Key == itemToDelete).Value);
                        _photosBase64Dictionary.Remove(itemToDelete);
                    }
                }
            }
            else
            {
                _photosBase64Dictionary.Clear();
                PhotosBase64Dictionary.Clear();
            }
        }

        private void AddImagesView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Width":
                    if (Width > 0)
                    {
                        var tempWidthOneCell = (Width - Spacing*6)/5;
                        HeightRequest = tempWidthOneCell*2 + Spacing * 3;
                    }
                    break;
                case "OldImagePaths":
                    if (OldImagePaths?.Count > 0)
                    {
                        for (int i = 0; i < OldImagePaths.Count && i < 9; i++)
                        {
                            ImagePaths.Insert(1, OldImagePaths[i]);
                            _images[i+1].SetUri(OldImagePaths[i]);
                        }
                        if (OldImagePaths.Count >= 10)
                        {
                            ImagePaths[0] = OldImagePaths[9];
                            _images[0].SetUri(OldImagePaths[9]);
                            _images[0].CanUseLongClick = true;
                        }
                    }
                    break;
            }
        }

        private void InitilizeGrid()
        {
            RowSpacing = Spacing;
            ColumnSpacing = Spacing;
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition
                {
                    Height = new GridLength(0.5f, GridUnitType.Star)
                },
                new RowDefinition
                {
                    Height = new GridLength(0.5f, GridUnitType.Star)
                }
            };
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition
                {
                    Width = new GridLength(0.2f,GridUnitType.Star)
                },
                new ColumnDefinition
                {
                    Width = new GridLength(0.2f,GridUnitType.Star)
                },
                new ColumnDefinition
                {
                    Width = new GridLength(0.2f,GridUnitType.Star)
                },
                new ColumnDefinition
                {
                    Width = new GridLength(0.2f,GridUnitType.Star)
                },
                new ColumnDefinition
                {
                    Width = new GridLength(0.2f,GridUnitType.Star)
                }
            };
        }

        private void AddImagesToGrid()
        {
            _images = new NewPinImage[RowDefinitions.Count*ColumnDefinitions.Count];
            for (int row = 0; row < RowDefinitions.Count; row++)
            {
                for (int column = 0; column < ColumnDefinitions.Count; column++)
                {
                    _images[row*ColumnDefinitions.Count + column] = new NewPinImage();
                    _images[row * ColumnDefinitions.Count + column].LongClick += AddImagesView_LongClick;
                    
                    Children.Add(_images[row * ColumnDefinitions.Count + column], column, row);
                }
            }
            _images[0].LongClick -= AddImagesView_LongClick;
        }

        private void AddImagesView_LongClick(object sender, PropertyChangedEventArgs e)
        {
            var senderObject = sender as NewPinImage;
            if (senderObject != null)
            {
                ImagePaths.Remove(senderObject.ImagePath);
                if (!string.IsNullOrEmpty(senderObject.ImageUri))
                    ImagePaths.Remove(senderObject.ImageUri);
                if (ImagePaths.Count == 9 && !string.IsNullOrEmpty(ImagePaths[0]))
                {
                    ImagePaths.Insert(0, string.Empty);
                    _images[0].ClearImage();
                    _images[0].Source = "plus_icon.png";
                    _images[0].CanUseLongClick = false;
                }
                for (int i = 1; i < _images.Length; i++)
                {
                    _images[i].ClearImage();
                }
                for (int i = 1; i < ImagePaths.Count; i++)
                {
                    _images[i].SetPath(ImagePaths[i]);
                }

                OnPropertyChanged("HaveChange");
            }
        }

        private void InitilizeFirstImage()
        {
            _images[0].Source = "plus_icon.png";
            _images[0].CanUseLongClick = false;
            _images[0].LongClick += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(_images[0].ImagePath))
                {
                    _images[0].ClearImage();
                    _images[0].Source = "plus_icon.png";
                    _images[0].CanUseLongClick = false;
                    ImagePaths[0] = string.Empty;
                }
            };
            _images[0].Click += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (ImagePaths.Count < 10 || (ImagePaths.Count==10 && string.IsNullOrEmpty(ImagePaths[0])))
                    {
                        var take = TextResource.AddPinTakePhoto;
                        var select = TextResource.AddPinSelectPhoto;
                        var action = await Application.Current.MainPage.DisplayActionSheet(
                            TextResource.PinPhoto,
                            TextResource.Cancel,
                            null,
                            take,
                            select);
                        string tempPhotoPath = string.Empty;
                        if (action == take)
                        {
                            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                SaveToAlbum = true,
                                Directory = "X-Iland"
                            });
                            if (photo == null) return;
                            tempPhotoPath = photo.Path;
                        }
                        else if (action == select)
                        {
                            //IMediaPicker mediaPicker = Resolver.Resolve<IMediaPicker>();
                            //var mediaFile = await mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions
                            //{
                            //    MaxPixelDimension = 400
                            //});
                            //if (mediaFile == null) return;
                            //tempPhotoPath= mediaFile.Path;
                            var photo = await CrossMedia.Current.PickPhotoAsync();
                            if (photo == null) return;
                            tempPhotoPath = photo.Path;

                        }
                        if (!string.IsNullOrEmpty(tempPhotoPath))
                        {
                            SetNewPhoto(tempPhotoPath);
                        }
                    }
                });
            };
        }

        private void SetNewPhoto(string photoPath)
        {
            if (ImagePaths.Count < 10)
            {
                ImagePaths.Insert(1, photoPath);
                for (int i = 1; i < ImagePaths.Count; i++)
                {
                    _images[i].SetPath(ImagePaths[i]);
                }
            }
            else if(ImagePaths.Count == 10)
            {
                ImagePaths[0] = photoPath;
                _images[0].SetPath(photoPath);
                _images[0].CanUseLongClick = true;
            }
            OnPropertyChanged("HaveChange");
        }

        public static readonly BindableProperty ImagePathsProperty =
            BindableProperty.Create<AddImagesView, ObservableCollection<string>>(p => p.ImagePaths, new ObservableCollection<string>(), BindingMode.OneWayToSource);

        public ObservableCollection<string> ImagePaths
        {
            get => (ObservableCollection<string>)GetValue(ImagePathsProperty);
            set => SetValue(ImagePathsProperty, value);
        }


        public static readonly BindableProperty PhotosBase64DictionaryProperty =
            BindableProperty.Create<AddImagesView, ObservableCollection<string>>(p => p.PhotosBase64Dictionary, new ObservableCollection<string>(), BindingMode.OneWayToSource);

        public ObservableCollection<string> PhotosBase64Dictionary
        {
            get => (ObservableCollection<string>)GetValue(PhotosBase64DictionaryProperty);
            set => SetValue(PhotosBase64DictionaryProperty, value);
        }

        public static readonly BindableProperty OldImagePathsProperty =
            BindableProperty.Create<AddImagesView, ObservableCollection<string>>(p => p.OldImagePaths, new ObservableCollection<string>(), BindingMode.OneWay);

        public ObservableCollection<string> OldImagePaths
        {
            get => (ObservableCollection<string>)GetValue(OldImagePathsProperty);
            set => SetValue(OldImagePathsProperty, value);
        }
    }

    public class NewPinImage : Image
    {
        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (value != _imagePath)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _imageUri;
        public string ImageUri
        {
            get => _imageUri;
            set
            {
                if (value != _imageUri)
                {
                    _imageUri = value;
                    OnPropertyChanged();
                }
            }
        }

        public NewPinImage()
        {
            Aspect = Aspect.AspectFill;
        }

        public bool CanUseLongClick = true;

        public event PropertyChangedEventHandler LongClick;
        public virtual void OnLongClick()
        {
            if (CanUseLongClick)
            {
                LongClick?.Invoke(this,
                    new PropertyChangedEventArgs("LongClick"));
            }
        }

        public event PropertyChangedEventHandler Click;
        public virtual void OnClick()
        {
            Click?.Invoke(this,
                new PropertyChangedEventArgs("Click"));
        }

        public void SetPath(string photoPath)
        {
            if (!string.IsNullOrEmpty(photoPath))
            {
                if (photoPath.Contains("http://185.76.145.214/"))
                {
                    SetUri(photoPath);
                }
                else
                {
                    ImagePath = photoPath;
                }
                //Source = ImageSource.FromFile(photoPath);
            }
            else
            {
                ClearImage();
            }
        }
        public void SetUri(string photoUri)
        {
            if (!string.IsNullOrEmpty(photoUri))
            {
                ImageUri = photoUri;
                Source = new UriImageSource
                {
                    Uri = new Uri(photoUri),
                    CachingEnabled = true,
                    CacheValidity = new TimeSpan(0, 2, 0, 0)
                };
            }
            else
            {
                ClearImage();
            }
        }

        public void ClearImage()
        {
            Source = null;
            ImagePath = string.Empty;
            ImageUri = string.Empty;
        }
    }
}
