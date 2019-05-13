using System;
using System.Windows.Input;
using Xamarin.Forms;


namespace Map_Bul_App.Design
{
    [Flags]
    public enum SwipeDirection
    {
        Right = 1,
        Left = 2,
        Up = 4,
        Down = 8
    }

    public class SwipedEventArgs : EventArgs
    {
        public SwipedEventArgs(object parameter, SwipeDirection direction)
        {
            Parameter = parameter;
            Direction = direction;
        }

        public object Parameter { get; private set; }

        public SwipeDirection Direction { get; private set; }
    }

    public interface ISwipeGestureController
    {
        void SendSwipe(Element sender, double totalX, double totalY);
        bool DetectSwipe(View sender, SwipeDirection direction);
    }
    public class SwipeGestureRecognizer : GestureRecognizer, ISwipeGestureController
    {
        // public SwipeGestureRecognizer(): base(null) { }

        // Default threshold in pixels before a swipe is detected.
        const uint DefaultSwipeThreshold = 100;

        double _totalX, _totalY;

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SwipeGestureRecognizer), null);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(SwipeGestureRecognizer), null);

        public static readonly BindableProperty DirectionProperty = BindableProperty.Create("Direction", typeof(SwipeDirection), typeof(SwipeGestureRecognizer), default(SwipeDirection));

        public static readonly BindableProperty ThresholdProperty = BindableProperty.Create("Threshold", typeof(uint), typeof(SwipeGestureRecognizer), DefaultSwipeThreshold);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public SwipeDirection Direction
        {
            get => (SwipeDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        public uint Threshold
        {
            get => (uint)GetValue(ThresholdProperty);
            set => SetValue(ThresholdProperty, value);
        }

        public event EventHandler<SwipedEventArgs> Swiped;

        void ISwipeGestureController.SendSwipe(Element sender, double totalX, double totalY)
        {
            _totalX = totalX;
            _totalY = totalY;
        }

        bool ISwipeGestureController.DetectSwipe(View sender, SwipeDirection direction)
        {
            var detected = false;
            var threshold = Threshold;

            if (direction.IsLeft())
            {
                detected |= _totalX < -threshold;
            }

            if (direction.IsRight())
            {
                detected |= _totalX > threshold;
            }

            if (direction.IsDown())
            {
                detected |= _totalY > threshold;
            }

            if (direction.IsUp())
            {
                detected |= _totalY < -threshold;
            }

            if (detected)
            {
                SendSwiped(sender, direction);
            }

            return detected;
        }

        public void SendSwiped(View sender, SwipeDirection direction)
        {
            ICommand cmd = Command;
            if (cmd != null && cmd.CanExecute(CommandParameter))
                cmd.Execute(CommandParameter);

            Swiped?.Invoke(sender, new SwipedEventArgs(CommandParameter, direction));
        }
    }

    static class SwipeDirectionExtensions
    {
        public static bool IsLeft(this SwipeDirection self)
        {
            return (self & SwipeDirection.Left) == SwipeDirection.Left;
        }
        public static bool IsRight(this SwipeDirection self)
        {
            return (self & SwipeDirection.Right) == SwipeDirection.Right;
        }
        public static bool IsUp(this SwipeDirection self)
        {
            return (self & SwipeDirection.Up) == SwipeDirection.Up;
        }
        public static bool IsDown(this SwipeDirection self)
        {
            return (self & SwipeDirection.Down) == SwipeDirection.Down;
        }
    }
}
