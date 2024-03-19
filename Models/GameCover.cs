using AdvancedModLoader.Image;
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
    public class GameCover : IAsyncImage
    {
        private IImage? CoverImage;

        public event PropertyChangedEventHandler? PropertyChanged;

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
            throw new NotImplementedException();
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
