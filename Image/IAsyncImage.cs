using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedModLoader.Image
{
    public interface IAsyncImage : INotifyPropertyChanged
    {
        bool IsLoaded();
        bool IsLoading();
        IImage LoadImage();
        Task<IImage> LoadImageAsync(CancellationTokenSource token);
    }
}
