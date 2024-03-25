using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedModLoader.i18n
{
    public class LanguageObject : AvaloniaObject
    {
        public static readonly DirectProperty<LanguageObject, object?> Value0Property = AvaloniaProperty.RegisterDirect<LanguageObject, object?>(nameof(Value0),
            o => o.Value0,
            (o, v) => o.Value0 = v);

        private object? _Value0;
        public object? Value0
        {
            get => _Value0;
            private set 
            { 
                SetAndRaise(Value0Property, ref _Value0, value);
                this.RaisePropertyChanged<string>(TextProperty, "", Text);
            }
        }

        public static readonly DirectProperty<LanguageObject, object?> Value1Property = AvaloniaProperty.RegisterDirect<LanguageObject, object?>(nameof(Value1),
            o => o.Value1,
            (o, v) => o.Value1 = v);

        private object? _Value1;
        public object? Value1
        {
            get => _Value1;
            private set => SetAndRaise(Value1Property, ref _Value1, value);
        }


        public static readonly DirectProperty<LanguageObject, object?> Value2Property = AvaloniaProperty.RegisterDirect<LanguageObject, object?>(nameof(Value2),
            o => o.Value2,
            (o, v) => o.Value2 = v);

        private object? _Value2;
        public object? Value2
        {
            get => _Value2;
            private set => SetAndRaise(Value2Property, ref _Value2, value);
        }

        public static readonly DirectProperty<LanguageObject, object?> Value3Property = AvaloniaProperty.RegisterDirect<LanguageObject, object?>(nameof(Value3),
            o => o.Value3,
            (o, v) => o.Value3 = v);

        private object? _Value3;
        public object? Value3
        {
            get => _Value3;
            private set => SetAndRaise(Value3Property, ref _Value3, value);
        }

        public string? Key { get; set; }

        public static readonly DirectProperty<LanguageObject, string> TextProperty = AvaloniaProperty.RegisterDirect<LanguageObject, string>(nameof(Text),
            o => o.Text);

        public string Text
        {
            get
            {
                var lines = Language.Instance[Key ?? ""];
                var line = lines.Length > 0 ? (lines[0] ?? "") : (Key ?? "");
                line = line.Replace("{0}", Value0?.ToString() ?? "");
                line = line.Replace("{1}", Value1?.ToString() ?? "");
                line = line.Replace("{2}", Value2?.ToString() ?? "");
                line = line.Replace("{3}", Value3?.ToString() ?? "");
                return line;
            }
        }


        public void Dispose()
        {
            Language.Instance.PropertyChanged -= Update;
        }

        public LanguageObject()
        {
            Language.Instance.PropertyChanged += Update;
        }

        private void Update(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveLanguage")
            {
                this.RaisePropertyChanged<string>(TextProperty, "", Text);
            }
        }
    }
}
