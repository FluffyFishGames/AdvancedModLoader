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
using Avalonia.Markup.Xaml.MarkupExtensions.CompiledBindings;
using Avalonia.Data.Core;

namespace AdvancedModLoader.i18n
{
    public class LangExtension : MarkupExtension
    {
        ~LangExtension()
        {
            if (Object != null)
                Object.Dispose();
        }

        public LangExtension(string key)
        {
            this.Key = key;
        }
        public LangExtension(string key, CompiledBindingExtension value0)
        {
            this.Key = key;
            this.Value0 = value0;
        }
        public LangExtension(string key, CompiledBindingExtension value0, CompiledBindingExtension value1)
        {
            this.Key = key;
            this.Value0 = value0;
            this.Value1 = value1;
        }
        public LangExtension(string key, CompiledBindingExtension value0, CompiledBindingExtension value1, CompiledBindingExtension value2)
        {
            this.Key = key;
            this.Value0 = value0;
            this.Value1 = value1;
            this.Value2 = value2;
        }
        public LangExtension(string key, CompiledBindingExtension value0, CompiledBindingExtension value1, CompiledBindingExtension value2, CompiledBindingExtension value3)
        {
            this.Key = key;
            this.Value0 = value0;
            this.Value1 = value1;
            this.Value2 = value2;
            this.Value3 = value3;
        }

        public string? Key { get; set; }
        public CompiledBindingExtension? Value0 { get; set; }
        public CompiledBindingExtension? Value1 { get; set; }
        public CompiledBindingExtension? Value2 { get; set; }
        public CompiledBindingExtension? Value3 { get; set; }
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
            {
                var instanced = Value0.Initiate(Object, LanguageObject.Value0Property);
                if (instanced != null)
                    BindingOperations.Apply(Object, LanguageObject.Value0Property, instanced, null);
            }
            if (Value1 != null)
            {
                var instanced = Value1.Initiate(Object, LanguageObject.Value1Property);
                if (instanced != null)
                    BindingOperations.Apply(Object, LanguageObject.Value0Property, instanced, null);
            }
            if (Value2 != null)
            {
                var instanced = Value2.Initiate(Object, LanguageObject.Value2Property);
                if (instanced != null)
                    BindingOperations.Apply(Object, LanguageObject.Value2Property, instanced, null);
            }
            if (Value3 != null)
            {
                var instanced = Value3.Initiate(Object, LanguageObject.Value3Property);
                if (instanced != null)
                    BindingOperations.Apply(Object, LanguageObject.Value3Property, instanced, null);
            }

            var property = new ClrPropertyInfo("Text", obj => { if (obj is LanguageObject o) return o.Text; return obj.GetType().FullName; }, null, typeof(string));
            var path = new CompiledBindingPathBuilder().SetRawSource(Object).Property(property, PropertyInfoAccessorFactory.CreateInpcPropertyAccessor).Build();
            var binding = new CompiledBindingExtension(path)
            {
                Mode = BindingMode.OneWay
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
