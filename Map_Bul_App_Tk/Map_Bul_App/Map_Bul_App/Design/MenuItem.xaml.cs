using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class MenuItem 
    {
        public MenuItem()
        {
            InitializeComponent();
        }


        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create<MenuItem, string>(p => p.LabelText, default(string));

        public static readonly BindableProperty IconSourceProperty =
            BindableProperty.Create<MenuItem, string>(p => p.IconSource, default(string));
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext == null) return;
            TitleLabel.Text = LabelText;
            IconImage.Source = IconSource;
        }

        public string LabelText
        {
            get
            {
                return (string)GetValue(LabelTextProperty);
            }
            set
            {
                SetValue(LabelTextProperty, value);
            }
        }

        public string IconSource
        {
            get
            {
                return (string)GetValue(IconSourceProperty);
            }
            set
            {
                SetValue(IconSourceProperty, value);
            }
        }
    }
}
