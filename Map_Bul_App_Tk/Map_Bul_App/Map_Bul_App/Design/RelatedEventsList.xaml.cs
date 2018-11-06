using System.Collections.Generic;
using Map_Bul_App.ViewModel;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class RelatedEventsList
    {
        public RelatedEventsList()
        {
            InitializeComponent();
            PropertyChanged += RelatedEventsList_PropertyChanged;
        }

        private void RelatedEventsList_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                if (ItemsSource.Count > 0)
                {
                    int i = 0;
                    foreach (var item in ItemsSource)
                    {
                        MainGrid.RowDefinitions.Add(new RowDefinition
                        {
                            Height = GridLength.Auto
                        });
                        MainGrid.Children.Add(
                            new ItemMediumLabel
                            {
                                Text = item.StartDate,
                                VerticalOptions = LayoutOptions.Start,
                                VerticalTextAlignment = TextAlignment.Start
                            }, 0, i);
                        MainGrid.Children.Add(
                            new ItemMediumLabel
                            {
                                Text = item.Name,
                                VerticalOptions = LayoutOptions.Start,
                                VerticalTextAlignment = TextAlignment.Start,
                                IsUnderline = true,
                                GestureRecognizers =
                                {
                                    new TapGestureRecognizer
                                    {
                                        Command = item.GoToEventCommand
                                    }
                                }
                            }, 1, i);
                        i++;
                    }
                }
            }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<RelatedEventsList, List<RelatedEvent>>(p => p.ItemsSource, default(List<RelatedEvent>));

        public List<RelatedEvent> ItemsSource
        {
            get { return (List<RelatedEvent>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
    }
}
