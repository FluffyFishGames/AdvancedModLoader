using Avalonia.Controls;
using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedModLoader.Controls
{
    public class ColoredImage : Avalonia.Controls.Image
    {
        public static readonly StyledProperty<IBrush?> ColorProperty =
            AvaloniaProperty.Register<ColoredImage, IBrush?>(nameof(Color), null, false, Avalonia.Data.BindingMode.OneWay, (b) => { Console.WriteLine(b); return true; });
        static ColoredImage()
        {
            AffectsRender<ColoredImage>(ColorProperty);
        }
        public IBrush? Color
        {
            get => GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

    }
}
