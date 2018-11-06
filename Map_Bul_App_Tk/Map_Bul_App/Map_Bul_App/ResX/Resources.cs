using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Map_Bul_App.ResX
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Text == null ? null : TextResource.ResourceManager.GetString(Text, CultureInfo.CurrentCulture);
        }
    }

    [ContentProperty("Text")]
    public class WithRequiedTranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return Text == null ? null : TextResource.ResourceManager.GetString(Text, CultureInfo.CurrentCulture)+"*";
        }
    }
}
