using AdvancedModLoader.Image;
using Avalonia;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedModLoader.Models
{
    public class GameCover : AvaloniaObject, IAsyncImageSource
    {
        private IImage? CoverImage;

        public bool IsLoaded()
        {
            throw new NotImplementedException();
        }

        public bool IsLoading()
        {
            throw new NotImplementedException();
        }

        public IImage LoadImage()
        {
            Thread.Sleep(5000);
        }

        public Task<IImage> LoadImageAsync(CancellationTokenSource token)
        {
            return Task<IImage>.Run(() =>
            {
                return LoadImage();
            });
        }
    }
}
