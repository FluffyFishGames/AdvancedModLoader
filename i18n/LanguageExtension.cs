using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Converters;
using Avalonia.Markup.Xaml.XamlIl.Runtime;
using Avalonia.Styling;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Data;
using System.ComponentModel;

namespace AdvancedModLoader.i18n
{
    public class LanguageExtension : MarkupExtension
    {
        ~LanguageExtension()
        {
            if (Object != null)
                Object.Dispose();
        }

        public LanguageExtension(string key)
        {
            this.Key = key;
        }
        public LanguageExtension(string key, Binding value0)
        {
            this.Key = key;
            this.Value0 = value0;
        }
        public LanguageExtension(string key, Binding value0, Binding value1)
        {
            this.Key = key;
            this.Value0 = value0;
            this.Value1 = value1;
        }
        public LanguageExtension(string key, Binding value0, Binding value1, Binding value2)
        {
            this.Key = key;
            this.Value0 = value0;
            this.Value1 = value1;
            this.Value2 = value2;
        }
        public LanguageExtension(string key, Binding value0, Binding value1, Binding value2, Binding value3)
        {
            this.Key = key;
            this.Value0 = value0;
            this.Value1 = value1;
            this.Value2 = value2;
            this.Value3 = value3;
        }

        public string? Key { get; set; }
        public Binding? Value0 { get; set; }
        public Binding? Value1 { get; set; }
        public Binding? Value2 { get; set; }
        public Binding? Value3 { get; set; }
        private LanguageObject? Object;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Object != null)
                Object.Dispose();
            Object = new LanguageObject()
            {
                Key = Key
            };
            if (Value0 != null)
                Value0.Initiate(languageObject, LanguageObject.Value0Property);
            if (Value1 != null)
                Value1.Initiate(languageObject, LanguageObject.Value1Property);
            if (Value2 != null)
                Value2.Initiate(languageObject, LanguageObject.Value2Property);
            if (Value3 != null)
                Value3.Initiate(languageObject, LanguageObject.Value3Property);

            var binding = new ReflectionBindingExtension($"Text")
            {
                Mode = BindingMode.OneWay,
                Source = Object
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
