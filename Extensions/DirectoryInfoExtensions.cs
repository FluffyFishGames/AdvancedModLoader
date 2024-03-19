using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedModLoader.Extensions
{
    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo GetOrCreateSubdirectory(this DirectoryInfo directory, string directoryName)
        {
            var newDirectory = new DirectoryInfo(Path.Combine(directory.FullName, directoryName));
            if (!newDirectory.Exists)
                newDirectory.Create();
            return newDirectory;
        }
    }
}
