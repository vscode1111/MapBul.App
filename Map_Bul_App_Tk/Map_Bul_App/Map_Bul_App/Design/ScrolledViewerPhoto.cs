using System.Collections.Generic;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public class ScrolledViewerPhoto : ScrollView
    {
        private readonly StackLayout _contentLayout;
        public ScrolledViewerPhoto()
        {
            Orientation = ScrollOrientation.Horizontal;
            _contentLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            Content = _contentLayout;
            PropertyChanged += ScrolledViewerPhoto_PropertyChanged;
        }

        private void ScrolledViewerPhoto_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ImagesSourceProperty.PropertyName)
            {
                if (ImagesSource?.Count > 0)
                {
                    //if (Width > 0)
                    //{
                    //    HeightRequest = Width/5;
                    //}
                    foreach (var imageSource in ImagesSource)
                    {
                        var tempImage = new CustomImageView
                        {
                            Aspect = Aspect.AspectFill,
                            Source = imageSource,
                            WebImagePath = imageSource,
                            WidthRequest = Width
                        };
                        tempImage.GestureRecognizers.Add(new TapGestureRecognizer
                        {
                            Command = new Command(() =>
                            {
                                SelectedImage = tempImage.WebImagePath;
                            })
                        });
                        _contentLayout.Children.Add(tempImage);
                    }
                }
            }
        }

        public static readonly BindableProperty ImagesSourceProperty =
            BindableProperty.Create<ScrolledViewerPhoto, List<string>>(p => p.ImagesSource, null);

        public static readonly BindableProperty SelectedImageProperty =
            BindableProperty.Create<ScrolledViewerPhoto, string>(p => p.SelectedImage, null, BindingMode.TwoWay);

        public List<string> ImagesSource
        {
            get { return (List<string>) GetValue(ImagesSourceProperty); }
            set { SetValue(ImagesSourceProperty, value); }
        }

        public string SelectedImage
        {
            get { return (string)GetValue(SelectedImageProperty); }
            set { SetValue(SelectedImageProperty, value); }
        }
    }
}
