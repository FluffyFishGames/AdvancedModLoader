using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdvancedModLoader.Extensions;

namespace AdvancedModLoader.Configuration
{
    public class Paths
    {
        private static DirectoryInfo? _RootDirectory;
        public static DirectoryInfo? RootDirectory
        {
            get
            {
                if (_RootDirectory == null)
                {
                    // should return ~/.local/share for linux and AppData/LocalLow for windows.
                    _RootDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "AdvancedModLoader"));
                    // ensure directory exists
                    if (!_RootDirectory.Exists)
                        _RootDirectory.Create();
                }
                return _RootDirectory;
            }
        }

        public static DirectoryInfo? CacheDirectory
        {
            get
            {
                return _RootDirectory?.GetOrCreateSubdirectory("cache") ?? null;
            }
        }

        public static DirectoryInfo? GetCacheDirectory(string cacheName)
        {
            return CacheDirectory?.GetOrCreateSubdirectory(cacheName) ?? null;
        }
    }
}
