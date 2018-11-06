using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TK.CustomMap
{
    /// <summary>
    /// A custom map pin
    /// </summary>
    public class TKCustomMapPin : TKBase
    {
        #region [Kopytin]
        private int _id; 
        private string _type;
        private string _infoImage;
        private bool _isSmall;
        private int _scale;

        
        private string _workTime;

        private string _tags;

        #endregion
        private bool _isVisible;
        private string _title;
        private string _subtitle;
        private bool _showCallout;
        private Position _position;

        



        private bool _isDraggable;
        private Color _defaultPinColor;
        private ImageSource _image;
        public const string IdPropertyName = "Id";
        public const string TypePropertyName = "Type";
        public const string InfoImagePropertyName = "InfoImage";
        public const string IsSmallPropertyName = "IsSmall";
        public const string ScalePropertyName = "Scale";
        public const string TitlePropertyName = "Title";
        public const string SubititlePropertyName = "Subtitle";
        public const string PositionPropertyName = "Position";
        public const string ImagePropertyName = "Image";
        public const string IsVisiblePropertyName = "IsVisible";
        public const string IsDraggablePropertyName = "IsDraggable";
        public const string ShowCalloutPropertyName = "ShowCallout";
        public const string DefaultPinColorPropertyName = "DefaultPinColor";
        public const string WorkTimePropertyName = "WorkTime";
        public const string TagsPropertyName = "Tags";

        /// <summary>
        /// Gets/Sets ID of a pin
        /// </summary>
        public int Id
        {
            get { return this._id; }
            set { this.SetField(ref this._id, value); }
        }

        /// <summary>
        /// Gets/Sets ID of a pin
        /// </summary>
        public string Type
        {
            get { return this._type; }
            set { this.SetField(ref this._type, value); }
        }

        /// <summary>
        /// Gets/Sets visibility of a pin
        /// </summary>
        public bool IsVisible 
        {
            get { return this._isVisible; }
            set { this.SetField(ref this._isVisible, value); }
        }
        /// <summary>
        /// Gets/Sets title of the pin displayed in the callout
        /// </summary>
        public string Title 
        {
            get { return this._title; }
            set { this.SetField(ref this._title, value); }
        }
        /// <summary>
        /// Gets/Sets the subtitle of the pin displayed in the callout
        /// </summary>
        public string Subtitle 
        {
            get { return this._subtitle; }
            set { this.SetField(ref this._subtitle, value); }
        }
        /// <summary>
        /// Gets/Sets if the callout should be displayed when a pin gets selected
        /// </summary>
        public bool ShowCallout 
        {
            get { return this._showCallout; }
            set { this.SetField(ref this._showCallout, value); }
        }
        /// <summary>
        /// Gets/Sets the position of the pin
        /// </summary>
        public Position Position 
        {
            get { return this._position; }
            set { this.SetField(ref this._position, value); }
        }
        /// <summary>
        /// Gets/Sets the image of the pin. If null the default is used
        /// </summary>
        public ImageSource Image 
        {
            get { return this._image; }
            set { this.SetField(ref this._image, value); }
        }

        /// <summary>
        /// Gets/Sets the image for the Info Window
        /// </summary>
        public string  InfoImage
        {
            get { return this._infoImage; }
            set { this.SetField(ref this._infoImage, value); }
        }

        public bool IsSmall
        {
            get { return this._isSmall; }
            set { this.SetField(ref this._isSmall, value); }
        }


        public int Scale
        {
            get { return this._scale; }
            set { this.SetField(ref this._scale, value); }
        }


        /// <summary>
        /// Gets/Sets if the pin is draggable
        /// </summary>
        public bool IsDraggable 
        {
            get { return this._isDraggable; }
            set { this.SetField(ref this._isDraggable, value); }
        }
        /// <summary>
        /// Gets/Sets the color of the default pin. Only applies when no <see cref="Image"/> is set
        /// </summary>
        public Color DefaultPinColor
        {
            get { return this._defaultPinColor; }
            set { this.SetField(ref this._defaultPinColor, value); }
        }

        public string WorkTime
        {
            get { return this._workTime; }
            set { this.SetField(ref this._workTime, value); }
        }
        public string Tags
        {
            get { return this._tags; }
            set { this.SetField(ref this._tags, value); }
        }
        /// <summary>
        /// Creates a new instance of <see cref="TKCustomMapPin" />
        /// </summary>
        public TKCustomMapPin()
        {
            this.IsVisible = true;
        }
    }
}
