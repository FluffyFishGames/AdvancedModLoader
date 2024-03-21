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

namespace AdvancedModLoader.i18n
{
    public class Language : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged; 
        private const string IndexerName = "Item";
        private const string IndexerArrayName = "Item[]";
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
                using (var stream = AssetLoader.Open(new Uri($"avares://AvaloniaLocalizationExample/Assets/i18n/{language}.json")))
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
                Invalidate();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string this[string key]
        {
            get
            {
                if (Lines != null && Lines.TryGetValue(key, out var res))
                    return res[0]?.Replace("\\n", "\n") ?? $"{ActiveLanguage}:{key}";

                return $"{ActiveLanguage}:{key}";
            }
        }

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
        }
    }
}
