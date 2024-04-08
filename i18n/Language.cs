using Avalonia.Platform;
using Avalonia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Nodes;
using Avalonia.Controls.Shapes;
using System.Text.RegularExpressions;
using SoloX.ExpressionTools.Parser.Impl;

namespace AdvancedModLoader.i18n
{
    public class Language : INotifyPropertyChanged
    {
        private static Regex PlaceholderRegex = new Regex(@"{([0-9]+)(?::([^}]+))?}");
        private static Func<int, int>? PluralDelegate;
        public event PropertyChangedEventHandler? PropertyChanged; 
        private Dictionary<string, string?[]>? Lines = null;
        public string? ActiveLanguage { get; private set; }
        public static Language Instance { get; set; } = new Language();

        public Language()
        {
            LoadLanguage("en-EN");
        }

        public bool LoadLanguage(string language)
        {
            try
            {
                var newLines = new Dictionary<string, string?[]>();
                using (var stream = AssetLoader.Open(new Uri($"avares://AdvancedModLoader/Assets/i18n/{language}.json")))
                using (var json = JsonDocument.Parse(stream))
                {
                    foreach (var obj in json.RootElement.EnumerateObject())
                    {
                        if (obj.Value.ValueKind == JsonValueKind.Array)
                        {
                            var strs = new string?[obj.Value.GetArrayLength()];
                            var i = 0;
                            foreach (var n in obj.Value.EnumerateArray())
                                strs[i++] = n.GetString();
                            newLines.Add(obj.Name, strs);
                        }
                        else
                        {
                            newLines.Add(obj.Name, [obj.Value.GetString()]);
                        }
                    }
                }
                ActiveLanguage = language;
                Lines = newLines;
                PluralDelegate = null;
                if (Lines.ContainsKey("_PLURAL"))
                {
                    var pluralRule = $"(int i) => {Lines["_PLURAL"][0]}";
                    var expressionParser = new ExpressionParser();
                    var expression = expressionParser.Parse<Func<int, int>>(pluralRule);
                    PluralDelegate = expression.Compile();
                }
                Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public string GetString(string key, object? value1, object? value2, object? value3, object? value4)
        {
            if (Lines != null && Lines.TryGetValue(key, out var lines))
            {
                if (lines != null)
                {
                    if (lines.Length == 0)
                        return "";
                    int form = GetPluralForm(value1);
                    if (lines.Length <= form)
                        form = 0;

                    var line = lines[form];
                    return PlaceholderRegex.Replace(line ?? "", (match) =>
                    {
                        var num = match.Groups[1].Value;
                        var format = match.Groups[2].Value;
                        switch (num)
                        {
                            case "0":
                                return Format(value1, format) ?? "";
                            case "1":
                                return Format(value2, format) ?? "";
                            case "2":
                                return Format(value3, format) ?? "";
                            case "3":
                                return Format(value4, format) ?? "";
                            default:
                                return match.Value;
                        }
                    });
                }
            }
            return $"{key}";
        }

        public string? Format(object? obj, string? format)
        {
            if (obj == null) return "";
            if (obj is int i)
                return i.ToString(format);
            if (obj is float f)
                return f.ToString(format);
            return obj.ToString();
        }

        public int GetPluralForm(object? obj)
        {
            if (obj == null) return 0;
            if (PluralDelegate != null)
            {
                if (obj is int i)
                    return PluralDelegate(i);
            }
            return 0;
        }

        public string?[] this[string key]
        {
            get
            {
                if (Lines != null && Lines.TryGetValue(key, out var res))
                    return res;

                return [$"{ActiveLanguage}:{key}"];
            }
        }

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ActiveLanguage"));
        }
    }
}
