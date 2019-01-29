using System;
using Xamarin.Forms;

namespace Map_Bul_App.Design
{
    public partial class CustomStackLayoutWithEntry
    {
        public CustomStackLayoutWithEntry()
        {
            InitializeComponent();
            EntryOnStack.TextChanged += EntryOnStack_TextChanged;
        }

        public event EventHandler TextChanged; 
        public static readonly BindableProperty LabelTextProperty =
            BindableProperty.Create<CustomStackLayoutWithEntry, string>(p => p.LabelText, default(string));

        public static BindableProperty EntryTextProperty =
            BindableProperty.Create<CustomStackLayoutWithEntry, string>(p => p.EntryText, default(string), BindingMode.TwoWay, null, EntryTextPropertyChanged);


        public static readonly BindableProperty EntryPlaceholderProperty =
            BindableProperty.Create<CustomStackLayoutWithEntry, string>(p => p.EntryPlaceholder, default(string));

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create<CustomStackLayoutWithEntry, Keyboard>(p => p.Keyboard, Keyboard.Default);

        private static void EntryTextPropertyChanged(BindableObject bindable, string oldValue, string newValue)
        {
            var control = bindable as CustomStackLayoutWithEntry;
            if (control == null) return;
            control.EntryOnStack.Text = newValue;
        }

        private void EntryOnStack_TextChanged(object sender, TextChangedEventArgs e)
        {
            EntryText = e.NewTextValue;
            OnTextChanged();

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext == null) return;
            LabelOnStack.Text = LabelText;
            EntryOnStack.Text = EntryText;
            EntryOnStack.Placeholder = EntryPlaceholder;
            EntryOnStack.Keyboard = Keyboard;
        }

        public Keyboard Keyboard
        {
            get => (Keyboard)GetValue(KeyboardProperty);
            set => SetValue(KeyboardProperty, value);
        }

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public string EntryText
        {
            get => (string)GetValue(EntryTextProperty);
            set => SetValue(EntryTextProperty, value);
        }

        public string EntryPlaceholder
        {
            get => (string)GetValue(EntryPlaceholderProperty);
            set => SetValue(EntryPlaceholderProperty, value);
        }


        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
